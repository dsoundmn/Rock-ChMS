﻿//
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

            if ( !this.IsPostBack )
            {
                ShowView();
            }
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
            pnlVersionGrid.Visible = false;
            mdEdit.Show();

            string entityValue = EntityValue();
            Rock.Model.HtmlContent content = new HtmlContentService().GetActiveContent( CurrentBlock.Id, entityValue );
            edtHtml.Text = content != null ? content.Content : string.Empty;
            edtHtml.MergeFields.Clear();
            edtHtml.MergeFields.Add( "GlobalAttribute" );
        }

        #endregion

        #region View methods

        /// <summary>
        /// Shows the view.
        /// </summary>
        protected void ShowView()
        {
            mdEdit.Hide();
            pnlEdit.Visible = false;
            pnlVersionGrid.Visible = false;
            string entityValue = EntityValue();
            string html = string.Empty;

            string cachedContent = GetCacheItem( entityValue ) as string;

            // if content not cached load it from DB
            if ( cachedContent == null )
            {
                Rock.Model.HtmlContent content = new HtmlContentService().GetActiveContent( CurrentBlock.Id, entityValue );

                if ( content != null )
                {
                    html = content.Content.ResolveMergeFields( GetGlobalMergeFields() );
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
        private void BindGrid()
        {
            var htmlContentService = new HtmlContentService();
            var content = htmlContentService.GetContent( CurrentBlock.Id, EntityValue() );

            var versions = content.
                Select( v => new
                {
                    v.Id,
                    v.Version,
                    VersionText = "Version " + v.Version.ToString(),
                    v.Content,
                    ModifiedDateTime = v.LastModifiedDateTime.ToElapsedString(),
                    ModifiedByPerson = v.LastModifiedPerson,
                    Approved = v.IsApproved,
                    ApprovedByPerson = v.ApprovedByPerson,
                    v.StartDateTime,
                    v.ExpireDateTime
                } ).ToList();

            gVersions.DataSource = versions;
            gVersions.GridRebind += gVersions_GridRebind;
            gVersions.DataBind();
        }

        /// <summary>
        /// Handles the GridRebind event of the gVersions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gVersions_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
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

        /// <summary>
        /// Gets the global merge fields.
        /// </summary>
        /// <returns></returns>
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
                    globalAttributeValues.Add( attributeCache.Key,
                        attributeCache.FieldType.Field.FormatValue( this, globalAttributes.AttributeValues[attributeCache.Key].Value, attributeCache.QualifierValues, false ) );
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
            bool supportVersioning = GetAttributeValue( "SupportVersions" ).AsBoolean();
            bool requireApproval = GetAttributeValue( "RequireApproval" ).AsBoolean();
            
            Rock.Model.HtmlContent content = null;
            HtmlContentService service = new HtmlContentService();

            // get settings
            string entityValue = EntityValue();

            // get current  content
            int version = hfVersion.ValueAsInt(); ;
            content = service.GetByBlockIdAndEntityValueAndVersion( CurrentBlock.Id, entityValue, version );

            // if the existing content changed, and the overwrite option was not checked, create a new version
            if ( content != null && supportVersioning && content.Content != edtHtml.Text && !cbOverwriteVersion.Checked )
            {
                content = null;
            }

            // if a record doesn't exist then create one
            if ( content == null )
            {
                content = new Rock.Model.HtmlContent();
                content.BlockId = CurrentBlock.Id;
                content.EntityValue = entityValue;

                if ( supportVersioning )
                {
                    int? maxVersion = service.Queryable()
                        .Where( c => c.BlockId == CurrentBlock.Id && c.EntityValue == entityValue )
                        .Select( c => (int?)c.Version ).Max();

                    content.Version = maxVersion.HasValue ? maxVersion.Value + 1 : 1;
                }
                else
                {
                    content.Version = 0;
                }

                service.Add( content, CurrentPersonId );
            }

            if ( supportVersioning )
            {
                content.StartDateTime = pDateRange.LowerValue;
                content.ExpireDateTime = pDateRange.UpperValue;
            }
            else
            {
                content.StartDateTime = null;
                content.ExpireDateTime = null;
            }

            if ( !requireApproval || IsUserAuthorized( "Approve" ) )
            {
                content.IsApproved = !requireApproval || chkApproved.Checked;
                if ( content.IsApproved )
                {
                    content.ApprovedByPersonId = CurrentPersonId;
                    content.ApprovedDateTime = DateTime.Now;
                }
            }

            content.Content = edtHtml.Text;
            content.LastModifiedPersonId = this.CurrentPersonId;
            content.LastModifiedDateTime = DateTime.Now;

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

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            ShowView();
        }

        #endregion

        protected void SelectVersion_Click( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
            // TODO
        }

        /// <summary>
        /// Handles the Click event of the btnShowVersionGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnShowVersionGrid_Click( object sender, EventArgs e )
        {
            BindGrid();
            pnlVersionGrid.Visible = true;
            pnlEdit.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the btnReturnToEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnReturnToEdit_Click( object sender, EventArgs e )
        {
            pnlVersionGrid.Visible = false;
            pnlEdit.Visible = true;
        }
    }
}

