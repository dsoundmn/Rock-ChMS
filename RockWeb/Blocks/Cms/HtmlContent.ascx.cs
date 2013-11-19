//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Security;
using Rock.Web.UI;

namespace RockWeb.Blocks.Cms
{
    [AdditionalActions( new string[] { "Approve" } )]
    [TextField( "Pre-Text", "HTML text to render before the blocks main content.", false, "", "", 0, "PreText" )]
    [TextField( "Post-Text", "HTML text to render after the blocks main content.", false, "", "", 1, "PostText" )]
    [IntegerField( "Cache Duration", "Number of seconds to cache the content.", false, 0, "", 2 )]
    [TextField( "Context Parameter", "Query string parameter to use for 'personalizing' content based on unique values.", false, "", "", 3 )]
    [TextField( "Context Name", "Name to use to further 'personalize' content.  Blocks with the same name, and referenced with the same context parameter will share html values.", false )]
    [BooleanField( "Require Approval", "Require that content be approved?", false )]
    [BooleanField( "Support Versions", "Support content versioning?", false )]
    public partial class HtmlContent : RockBlock
    {
        #region Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            ShowView();
        }

        #endregion

        #region Admin bar methods

        public override List<Control> GetAdministrateControls( bool canConfig, bool canEdit )
        {
            List<Control> configControls = new List<Control>();

            // add edit icon to config controls if user has edit permission
            if ( canConfig || canEdit )
            {
                LinkButton btnEdit = new LinkButton();
                btnEdit.CssClass = "edit";
                btnEdit.ToolTip = "Edit HTML";
                btnEdit.Click += btnEdit_Click;
                configControls.Add( btnEdit );
                HtmlGenericControl iEdit = new HtmlGenericControl( "i" );
                btnEdit.Controls.Add( iEdit );
                btnEdit.CausesValidation = false;
                iEdit.Attributes.Add( "class", "icon-edit" );

                // will toggle the block config so they are no longer showing
                btnEdit.Attributes["onclick"] = "Rock.admin.pageAdmin.showBlockConfig()";

                ScriptManager.GetCurrent( this.Page ).RegisterAsyncPostBackControl( btnEdit );
            }

            configControls.AddRange( base.GetAdministrateControls( canConfig, canEdit ) );

            return configControls;
        }

        /// <summary>
        /// Handles the Click event of the lbEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            pnlEdit.Visible = true;
            mdEdit.Show();

            string entityValue = EntityValue();
            Rock.Model.HtmlContent content = new HtmlContentService().GetActiveContent( CurrentBlock.Id, entityValue );
            edtHtml.Text = content != null ? content.Content : string.Empty;

            LoadDropDowns();
        }

        #endregion

        #region View methods

        /// <summary>
        /// Shows the view.
        /// </summary>
        protected void ShowView()
        {
            pnlEdit.Visible = false;
            string entityValue = EntityValue();
            string html = string.Empty;

            string cachedContent = GetCacheItem( entityValue ) as string;

            // if content not cached load it from DB
            if ( cachedContent == null )
            {
                Rock.Model.HtmlContent content = new HtmlContentService().GetActiveContent( CurrentBlock.Id, entityValue );

                if ( content != null )
                {
                    html = content.Content;
                }
                else
                {
                    html = string.Empty;
                }

                // cache content
                int cacheDuration = GetAttributeValue( "CacheDuration" ).AsInteger() ?? 0;
                if ( cacheDuration > 0 )
                {
                    AddCacheItem( entityValue, html, cacheDuration );
                }
            }
            else
            {
                html = cachedContent;
            }

            // add content to the content window
            lPreText.Text = GetAttributeValue( "PreText" );
            lHtmlContent.Text = html;
            lPostText.Text = GetAttributeValue( "PostText" );
        }

        #endregion

        #region Version History

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void LoadDropDowns()
        {
            var HtmlService = new HtmlContentService();
            var content = HtmlService.GetContent( CurrentBlock.Id, EntityValue() );

            var personService = new Rock.Model.PersonService();
            var versionAudits = new Dictionary<int, Rock.Model.Audit>();
            var modifiedPersons = new Dictionary<int, string>();

            foreach ( var version in content )
            {
                var lastAudit = HtmlService.Audits( version )
                    .Where( a => a.AuditType == Rock.Model.AuditType.Add ||
                        a.AuditType == Rock.Model.AuditType.Modify )
                    .OrderByDescending( h => h.DateTime )
                    .FirstOrDefault();
                if ( lastAudit != null )
                    versionAudits.Add( version.Id, lastAudit );
            }

            foreach ( var audit in versionAudits.Values )
            {
                if ( audit.PersonId.HasValue && !modifiedPersons.ContainsKey( audit.PersonId.Value ) )
                {
                    var modifiedPerson = personService.Get( audit.PersonId.Value, true );
                    modifiedPersons.Add( audit.PersonId.Value, modifiedPerson != null ? modifiedPerson.FullName : string.Empty );
                }
            }

            var versions = content.
                Select( v => new
                {
                    v.Id,
                    v.Version,
                    v.Content,
                    ModifiedDateTime = versionAudits.ContainsKey( v.Id ) ? versionAudits[v.Id].DateTime.ToElapsedString() : string.Empty,
                    ModifiedByPerson = versionAudits.ContainsKey( v.Id ) && versionAudits[v.Id].PersonId.HasValue ? modifiedPersons[versionAudits[v.Id].PersonId.Value] : string.Empty,
                    Approved = v.IsApproved,
                    ApprovedByPerson = v.ApprovedByPerson != null ? v.ApprovedByPerson.FullName : "",
                    v.StartDateTime,
                    v.ExpireDateTime
                } ).ToList();

            ddlVersions.Items.Clear();
            ddlVersions.Items.Add( new ListItem( "Latest", "0" ) );
            foreach (var version in versions)
            {
                ddlVersions.Items.Add( new ListItem( string.Format( "Version {0} - {1} ", version.Version, version.ModifiedDateTime ), version.Id.ToString() ) );
            }
        }

        /// <summary>
        /// Entities the value.
        /// </summary>
        /// <returns></returns>
        private string EntityValue()
        {
            string entityValue = string.Empty;

            string contextParameter = GetAttributeValue( "ContextParameter" );
            if ( !string.IsNullOrEmpty( contextParameter ) )
            {
                entityValue = string.Format( "{0}={1}", contextParameter, PageParameter( contextParameter ) ?? string.Empty );
            }

            string contextName = GetAttributeValue( "ContextName" );
            if ( !string.IsNullOrEmpty( contextName ) )
            {
                entityValue += "&ContextName=" + contextName;
            }

            return entityValue;
        }

        #endregion

        #region Edit

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {

        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {

        }

        #endregion

        protected void ddlVersions_SelectedIndexChanged( object sender, EventArgs e )
        {
            // TODO
        }
    }
}

