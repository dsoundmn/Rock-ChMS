﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

using Rock.Model;
using Rock.Web.UI.Controls;

namespace Rock.PersonProfile.Badge
{
    /// <summary>
    /// Campus Badge
    /// </summary>
    [Description( "Campus Badge" )]
    [Export( typeof( BadgeComponent ) )]
    [ExportMetadata("ComponentName", "Campus")]
    public class Campus : TextBadge
    {
        /// <summary>
        /// Gets the attribute value defaults.
        /// </summary>
        /// <value>
        /// The attribute defaults.
        /// </value>
        public override System.Collections.Generic.Dictionary<string, string> AttributeValueDefaults
        {
            get
            {
                var defaults = base.AttributeValueDefaults;
                defaults["Order"] = "1";
                return defaults;
            }
        }

        /// <summary>
        /// Gets the badge label
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public override HighlightLabel GetLabel( Person person )
        {
            if ( ParentPersonBlock != null )
            {
                // Campus is associated with the family group(s) person belongs to.
                var families = ParentPersonBlock.PersonGroups( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.Guid );
                if ( families != null )
                {
                    var label = new HighlightLabel();
                    label.LabelType = LabelType.Campus;

                    var campusNames = new List<string>();
                    foreach ( int campusId in families
                        .Where( g => g.CampusId.HasValue )
                        .Select( g => g.CampusId )
                        .Distinct()
                        .ToList() )
                        campusNames.Add( Rock.Web.Cache.CampusCache.Read( campusId ).Name );

                    label.Text = campusNames.OrderBy( n => n ).ToList().AsDelimited( ", " );

                    return label;
                }
            }

            return null;

        }

    }
}
