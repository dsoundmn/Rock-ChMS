//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel;
using Rock;
using Rock.Attribute;
using Rock.Web.UI;

namespace RockWeb.Blocks.Reporting
{
    [DetailPage]
    [Description( "A dashboard widget page" )]
    public partial class Dashboard : RockBlock
    {        
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e ); 
            RockPage.AddCSSLink( this.Page, "~/css/jquery.gridster.min.css" );      // only load the CSS & JS for Gridster on this page. 
            RockPage.AddScriptLink( this.Page, "~/Scripts/jquery.gridster.js" );
        }

        /// <summary>
        /// Handles the Click event of the lbAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void lbAdd_Click( object sender, EventArgs e )
        {
            NavigateToDetailPage( "dashboardId", 0 );
        }
    }
}