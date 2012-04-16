//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;

using Rock.Data;

namespace Rock.Core
{
	/// <summary>
	/// Attribute POCO Service Layer class
	/// </summary>
    public partial class AttributeService : Service<Rock.Core.Attribute>
    {
		/// <summary>
		/// Gets Attributes by Entity
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <returns>An enumerable list of Attribute objects.</returns>
	    public IEnumerable<Rock.Core.Attribute> GetByEntity( string entity )
        {
            return Repository.Find( t => ( t.Entity == entity || ( entity == null && t.Entity == null ) ) ).OrderBy( t => t.Order );
        }
		
		/// <summary>
		/// Gets Attribute by Entity And Entity Qualifier Column And Entity Qualifier Value And Key
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="entityQualifierColumn">Entity Qualifier Column.</param>
		/// <param name="entityQualifierValue">Entity Qualifier Value.</param>
		/// <param name="key">Key.</param>
		/// <returns>Attribute object.</returns>
	    public Rock.Core.Attribute GetByEntityAndEntityQualifierColumnAndEntityQualifierValueAndKey( string entity, string entityQualifierColumn, string entityQualifierValue, string key )
        {
            return Repository.FirstOrDefault( t => ( t.Entity == entity || ( entity == null && t.Entity == null ) ) && ( t.EntityQualifierColumn == entityQualifierColumn || ( entityQualifierColumn == null && t.EntityQualifierColumn == null ) ) && ( t.EntityQualifierValue == entityQualifierValue || ( entityQualifierValue == null && t.EntityQualifierValue == null ) ) && t.Key == key );
        }
		
    }
}