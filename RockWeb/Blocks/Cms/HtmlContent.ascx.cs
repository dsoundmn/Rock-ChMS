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
using Rock.Web.Cache;

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
    public partial class HtmlContent : Rock.Web.UI.RockBlock
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

<<<<<<< HEAD
        /// <summary>
        /// Sets the current panel.
        /// </summary>
        /// <param name="pnl">The PNL.</param>
        private void SetCurrentPanel( Panel pnl )
        {
            pnlView.Visible = pnl == pnlView;
            pnlEdit.Visible = pnl == pnlEdit;
            pnlPreview.Visible = pnl == pnlPreview;
            pnlVersionHistory.Visible = pnl == pnlVersionHistory;
=======
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            hfAction.Value = string.Empty;
        }

        protected void lbEdit_Click( object sender, EventArgs e )
        {
            HtmlContentService service = new HtmlContentService();
            Rock.Model.HtmlContent content = service.GetActiveContent( CurrentBlock.Id, EntityValue() );
            if ( content == null )
                content = new Rock.Model.HtmlContent();

            if ( _supportVersioning )
            {
                phCurrentVersion.Visible = true;
                pnlVersioningHeader.Visible = true;
                cbOverwriteVersion.Visible = true;

                hfVersion.Value = content.Version.ToString();
                lVersion.Text = content.Version.ToString();
                tbStartDate.Text = content.StartDateTime.HasValue ? content.StartDateTime.Value.ToShortDateString() : string.Empty;
                tbExpireDate.Text = content.ExpireDateTime.HasValue ? content.ExpireDateTime.Value.ToShortDateString() : string.Empty;

                if ( _requireApproval )
                {
                    cbApprove.Checked = content.IsApproved;
                    cbApprove.Enabled = IsUserAuthorized( "Approve" );
                    cbApprove.Visible = true;
                }
                else
                    cbApprove.Visible = false;
            }
            else
            {
                phCurrentVersion.Visible = false;
                pnlVersioningHeader.Visible = false;
                cbOverwriteVersion.Visible = false;
            }

            htmlContent.Toolbar = "RockCustomConfigFull";
            htmlContent.Text = content.Content;
            htmlContent.MergeFields.Clear();
            htmlContent.MergeFields.Add( "GlobalAttribute" );
            mpeContent.Show();
            htmlContent.Visible = true;
            HtmlContentModified = false;
            htmlContent.TextChanged += htmlContent_TextChanged;

            BindGrid();

            hfAction.Value = "Edit";
        }

        /// <summary>
        /// Handles the TextChanged event of the htmlContent control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void htmlContent_TextChanged( object sender, EventArgs e )
        {
            HtmlContentModified = true;
        }

        protected override void OnPreRender( EventArgs e )
        {
            aClose.Attributes["onclick"] = string.Format(
                "$find('{0}').hide();return false;", mpeContent.BehaviorID );

            base.OnPreRender( e );
        }

        void HtmlContent_AttributesUpdated( object sender, EventArgs e )
        {
            lPreText.Text = GetAttributeValue( "PreText" );
            lPostText.Text = GetAttributeValue( "PostText" );
        }

        protected void btnSave_Click( object sender, EventArgs e )
        {
            if ( IsUserAuthorized( "Edit" ) || IsUserAuthorized( "Administrate" ) )
            {
                Rock.Model.HtmlContent content = null;
                HtmlContentService service = new HtmlContentService();

                // get settings
                string entityValue = EntityValue();

                // get current  content
                int version = 0;
                if ( !Int32.TryParse( hfVersion.Value, out version ) )
                    version = 0;
                content = service.GetByBlockIdAndEntityValueAndVersion( CurrentBlock.Id, entityValue, version );

                // if the existing content changed, and the overwrite option was not checked, create a new version
                if ( content != null &&
                    _supportVersioning &&
                    content.Content != htmlContent.Text &&
                    !cbOverwriteVersion.Checked )
                    content = null;

                // if a record doesn't exist then  create one
                if ( content == null )
                {
                    content = new Rock.Model.HtmlContent();
                    content.BlockId = CurrentBlock.Id;
                    content.EntityValue = entityValue;

                    if ( _supportVersioning )
                    {
                        int? maxVersion = service.Queryable().
                            Where( c => c.BlockId == CurrentBlock.Id &&
                                c.EntityValue == entityValue ).
                            Select( c => (int?)c.Version ).Max();

                        content.Version = maxVersion.HasValue ? maxVersion.Value + 1 : 1;
                    }
                    else
                        content.Version = 0;

                    service.Add( content, CurrentPersonId );
                }

                if ( _supportVersioning )
                {
                    DateTime startDate;
                    if ( DateTime.TryParse( tbStartDate.Text, out startDate ) )
                        content.StartDateTime = startDate;
                    else
                        content.StartDateTime = null;

                    DateTime expireDate;
                    if ( DateTime.TryParse( tbExpireDate.Text, out expireDate ) )
                        content.ExpireDateTime = expireDate;
                    else
                        content.ExpireDateTime = null;
                }
                else
                {
                    content.StartDateTime = null;
                    content.ExpireDateTime = null;
                }

                if ( !_requireApproval || IsUserAuthorized( "Approve" ) )
                {
                    content.IsApproved = !_requireApproval || cbApprove.Checked;
                    if ( content.IsApproved )
                    {
                        content.ApprovedByPersonId = CurrentPersonId;
                        content.ApprovedDateTime = DateTime.Now;
                    }
                }

                content.Content = htmlContent.Text;

                if ( service.Save( content, CurrentPersonId ) )
                {
                    // flush cache content 
                    this.FlushCacheItem( entityValue );
                    ShowView();
                }
                else
                {
                    // TODO: service.ErrorMessages;
                }

            }

            else
            {
                ShowView();
            }
>>>>>>> refs/heads/develop
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
            SetCurrentPanel( pnlEdit );
            mdEdit.Show();
            
            string entityValue = EntityValue();
            Rock.Model.HtmlContent content = new HtmlContentService().GetActiveContent( CurrentBlock.Id, entityValue );
            edtHtml.Text = content != null ? content.Content : string.Empty;
        }

        #endregion

        #region View methods

        /// <summary>
        /// Shows the view.
        /// </summary>
        protected void ShowView()
        {
            SetCurrentPanel( pnlView );

            string entityValue = EntityValue();
            string html = string.Empty;

            string cachedContent = GetCacheItem( entityValue ) as string;

            // if content not cached load it from DB
            if ( cachedContent == null )
            {
                Rock.Model.HtmlContent content = new HtmlContentService().GetActiveContent( CurrentBlock.Id, entityValue );

                if ( content != null )
<<<<<<< HEAD
                {
                    html = content.Content;
                }
=======
                    html = content.Content.ResolveMergeFields( GetGlobalMergeFields() );
>>>>>>> refs/heads/develop
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
        private void BindGrid()
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

            //rGrid.DataSource = versions;
            //rGrid.DataBind();
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

        public Dictionary<string, object> GetGlobalMergeFields()
        {
            var configValues = new Dictionary<string, object>();

            var globalAttributeValues = new Dictionary<string, object>();
            var globalAttributes = Rock.Web.Cache.GlobalAttributesCache.Read();
            foreach ( var attribute in globalAttributes.AttributeKeys.OrderBy( a => a.Value ) )
            {
                var attributeCache = AttributeCache.Read( attribute.Key );
                if ( attributeCache.IsAuthorized( "View", null ) )
                {
                    globalAttributeValues.Add( attributeCache.Key, globalAttributes.AttributeValues[attributeCache.Key].Value );
                }
            }

            configValues.Add( "GlobalAttribute", globalAttributeValues );

            return configValues;
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

        #region Preview

        /// <summary>
        /// Handles the Click event of the btnPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPreview_Click( object sender, EventArgs e )
        {
            lPreTextPreview.Text = GetAttributeValue( "PreText" );
            lHtmlContentPreview.Text = edtHtml.Text;
            lPostTextPreview.Text = GetAttributeValue( "PostText" );

            mdPreview.Show();
        }

        #endregion
        protected void btnShowVersionGrid_Click( object sender, EventArgs e )
        {

        }
}
}

