//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
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
using System.Collections;
using System.Xml;

namespace RockWeb.Blocks.Reporting
{
    public partial class DashboardDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            pnlDashboardGridBlockEditor.Visible = true;

            //setup the data in the drop downs
            MetricService metricService = new MetricService();
            var items2 = metricService.Queryable().OrderBy( a => a.Title ).ThenBy( a => a.Id ).Select( a => a.Title ).Distinct().ToList();
            foreach ( var item in items2 )
            {
                if ( !string.IsNullOrWhiteSpace( item ) )
                {
                    ddlGridBlockMetric.Items.Add( item );
                }
            }

            DefinedValueService valueService = new DefinedValueService();
            var items = valueService.Queryable().Where( a => a.DefinedTypeId == 19 ).OrderBy( a => a.Name ).Select( a => a.Name ).Distinct().ToList();
            foreach ( var item in items )
            {
                if ( !string.IsNullOrWhiteSpace( item ) )
                {
                    ddlGridBlockSize.Items.Add( item );
                }
            }
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
                    /// <summary>
                    /// Guid for Dashboard Block Sizes
                    /// </summary>
                    //public const string DASHBOARD_BLOCK_SIZES = "07EA2F91-4D17-4F7D-A61E-57A9D780C59A";
                }
                else
                {
                    pnlDashboardGridBlockEditor.Visible = false;
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
        /// Handles the Click event of the btnSaveDashboardGridBlock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSaveDashboardGridBlock_Click( object sender, EventArgs e )
        {
            // ---------------- THIS IS THE PART THAT'S GOOD -----------------
            Rock.Model.Dashboard dashboard = null;
            DashboardService dashboardService = new DashboardService();

            int dashboardId = hfDashboardGridBlockId.ValueAsInt();

            // Get the dashboard block or create a new one if needed
            if ( dashboardId == 0 )
            {
                dashboard = new Rock.Model.Dashboard();
                dashboard.IsSystem = false;
                dashboard.Order = 0;
                dashboardService.Add( dashboard, CurrentPersonId );
            }
            else
            {
                dashboard = dashboardService.Get( dashboardId );
            }

            //This is where we set all the fields
            dashboard.MetricId = Convert.ToInt32( ddlGridBlockMetric.SelectedIndex );
            dashboard.Description = txtGridBlockDescription.Text;
            dashboard.StartDate = Convert.ToDateTime(dtpGridBlockStartDate.Text);
            dashboard.EndDate = Convert.ToDateTime( dtpGridBlockEndDate.Text );
            dashboard.Size = Convert.ToInt32(ddlGridBlockSize.SelectedValue);

            if ( !Page.IsValid )
            {
                return;
            }

            if ( !dashboard.IsValid )
            {
                // field controls render error messages
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                dashboardService.Save(dashboard, CurrentPersonId);

                // get it back to make sure we have a good Id
                dashboard = dashboardService.Get(dashboard.Guid);
            } );
            
            // -------------------

            //ShowReadonlyDetails( definedType );
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

            pnlDashboardGridBlockEditor.Visible = true;
            Rock.Model.Dashboard dashboard = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                dashboard = new DashboardService().Get( itemKeyValue );
            }
            else
            {
                dashboard = new Rock.Model.Dashboard { Id = 0 };
            }

            hfDashboardGridBlockId.SetValue( dashboard.Id );

            if ( dashboard.Id > 0 )
            {
                lActionTitleDashboardGridBlock.Text = ActionTitle.Edit( Rock.Model.Dashboard.FriendlyTypeName );
            }
            else
            {
                lActionTitleDashboardGridBlock.Text = ActionTitle.Add( Rock.Model.Dashboard.FriendlyTypeName );
            }
            
            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( DefinedType.FriendlyTypeName );
            }

            if ( dashboard.IsSystem )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlySystem( DefinedType.FriendlyTypeName );
            }

            if ( readOnly )
            {
                ddlGridBlockMetric.Enabled = false;
                txtGridBlockDescription.Enabled = false;
                dtpGridBlockStartDate.Enabled = false;
                dtpGridBlockEndDate.Enabled = false;
                ddlGridBlockSize.Enabled = false;
                btnSaveDashboardGridBlock.Enabled = false;
            }

        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="definedType">Type of the defined.</param>
        private void ShowEditDetails( DefinedType definedType )
        {

            //SetEditMode( true );

            //tbTypeName.Text = definedType.Name;
            //tbTypeCategory.Text = definedType.Category;
            //tbTypeDescription.Text = definedType.Description;
            //ddlTypeFieldType.SetValue( definedType.FieldTypeId );
        }

        #endregion

        public void btnCancelDashboardGridBlock_Click( object sender, EventArgs e )
        {
            NavigateToParentPage();
        }


    }
}