//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Reporting
{

    public partial class DashboardDetail : RockBlock
    {
        #region Control Methods

        public enum GridSizes { 1x1, 1x2, 1x3, 1x4, 2x1, 2x2, 2x3, 2x4, 3x1, 3x2, 3x3, 3x4, 4x1, 4x2, 4x3, 4x4 };

        //protected void Page_Load( object sender, EventArgs e )
        //{
        //}

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            pnlDetails.Visible = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string itemId = PageParameter( "dashboardId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "dashboardId", int.Parse( itemId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }

            //if ( pnlDefinedValueEditor.Visible )
            //{
            //    if ( !string.IsNullOrWhiteSpace( hfDefinedTypeId.Value ) )
            //    {
            //        var definedValue = new DefinedValue { DefinedTypeId = hfDefinedTypeId.ValueAsInt() };
            //        definedValue.LoadAttributes();
            //        phDefinedValueAttributes.Controls.Clear();
            //        Rock.Attribute.Helper.AddEditControls( definedValue, phDefinedValueAttributes, false );
            //    }
            //}
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            if ( !itemKey.Equals( "dashboardId" ) )
            {
                return;
            }

            pnlDetails.Visible = true;
            Dashboard dashboard = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                
            }
            //if ( !itemKeyValue.Equals( 0 ) )
            //{
            //    definedType = new DefinedTypeService().Get( itemKeyValue );
            //}
            //else
            //{
            //    definedType = new DefinedType { Id = 0 };
            //}

            //hfDefinedTypeId.SetValue( definedType.Id );

            //// render UI based on Authorized and IsSystem
            //bool readOnly = false;

            //nbEditModeMessage.Text = string.Empty;
            //if ( !IsUserAuthorized( "Edit" ) )
            //{
            //    readOnly = true;
            //    nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( DefinedType.FriendlyTypeName );
            //}

            //if ( definedType.IsSystem )
            //{
            //    readOnly = true;
            //    nbEditModeMessage.Text = EditModeMessage.ReadOnlySystem( DefinedType.FriendlyTypeName );
            //}

            //if ( readOnly )
            //{
            //    btnEdit.Visible = false;
            //    ShowReadonlyDetails( definedType );
            //}
            //else
            //{
            //    btnEdit.Visible = true;
            //    if ( definedType.Id > 0 )
            //    {
            //        ShowReadonlyDetails( definedType );
            //    }
            //    else
            //    {
            //        ShowEditDetails( definedType );
            //    }
            //}

            //BindDefinedTypeAttributesGrid();
            //BindDefinedValuesGrid();
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="definedType">Type of the defined.</param>
        private void ShowEditDetails( DefinedType definedType )
        {
            if ( definedType.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( DefinedType.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( DefinedType.FriendlyTypeName );
            }

            //SetEditMode( true );

            //tbTypeName.Text = definedType.Name;
            //tbTypeCategory.Text = definedType.Category;
            //tbTypeDescription.Text = definedType.Description;
            //ddlTypeFieldType.SetValue( definedType.FieldTypeId );
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the btnSaveMetricAttribute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSaveMetricAttribute_Click( object sender, EventArgs e )
        {
            //Attribute attribute = null;

            //AttributeService attributeService = new AttributeService();
            //if ( edtDefinedTypeAttributes.AttributeId.HasValue )
            //{
            //    attribute = attributeService.Get( edtDefinedTypeAttributes.AttributeId.Value );
            //}

            //if ( attribute == null )
            //{
            //    attribute = new Attribute();
            //}

            //edtDefinedTypeAttributes.GetAttributeProperties( attribute );

            //// Controls will show warnings
            //if ( !attribute.IsValid )
            //{
            //    return;
            //}

            //RockTransactionScope.WrapTransaction( () =>
            //{
            //    if ( attribute.Id.Equals( 0 ) )
            //    {
            //        attribute.EntityTypeId = Rock.Web.Cache.EntityTypeCache.Read( new DefinedValue().TypeName ).Id;
            //        attribute.EntityTypeQualifierColumn = "DefinedTypeId";
            //        attribute.EntityTypeQualifierValue = hfDefinedTypeId.Value;
            //        attributeService.Add( attribute, CurrentPersonId );
            //    }

            //    Rock.Web.Cache.AttributeCache.Flush( attribute.Id );
            //    attributeService.Save( attribute, CurrentPersonId );
            //} );

            //pnlDetails.Visible = true;
            //pnlDefinedTypeAttributes.Visible = false;

            //BindDefinedTypeAttributesGrid();
            //BindDefinedValuesGrid();
        }

        /// <summary>
        /// Handles the Click event of the btnCancelMetricAttribute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancelMetricAttribute_Click( object sender, EventArgs e )
        {
            //pnlDetails.Visible = true;
            //pnlDefinedTypeAttributes.Visible = false;
        }

    }
}