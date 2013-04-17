//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using Rock.Extension;

namespace Rock.Reporting
{
    /// <summary>
    /// MEF Container class for dashboard "widgets"
    /// </summary>
    class DashboardContainer : Container<DashboardComponent, IComponentData>
    {
        private static DashboardContainer instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static DashboardContainer Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new DashboardContainer();
                }
                return instance;
            }
        }

        private DashboardContainer()
        {
            Refresh();
        }

        /// <summary>
        /// Gets the component with the matching Entity Type Name
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns></returns>
        public static DashboardComponent GetComponent( string entityTypeName )
        {
            foreach ( var serviceEntry in Instance.Components )
            {
                var component = serviceEntry.Value.Value;
                if ( component.TypeName == entityTypeName )
                {
                    return component;
                }
            }

            return null;
        }

        // MEF Import Definition
#pragma warning disable
        [ImportMany( typeof( DashboardComponent ) )]
        protected override IEnumerable<Lazy<DashboardComponent, IComponentData>> MEFComponents { get; set; }
#pragma warning restore

    }
}
