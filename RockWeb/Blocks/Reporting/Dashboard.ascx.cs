//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Text;


using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Reporting
{
    [DetailPage]
    public partial class Dashboard : RockBlock
    {
        
        /*protected void Page_Load(object sender, EventArgs e)
        {
        }*/

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e ); 
            //lbAdd.Click += lbAdd_Click;
            Rock.Web.UI.RockPage.AddCSSLink( this.Page, "~/css/jquery.gridster.min.css" );      // only load the CSS & JS for Gridster on this page. 
            Rock.Web.UI.RockPage.AddScriptLink( this.Page, "~/Scripts/jquery.gridster.js" );
        }

        public void lbAdd_Click( object sender, EventArgs e )
        {
            NavigateToDetailPage( "dashboardId", 0 );
        }

    }
}