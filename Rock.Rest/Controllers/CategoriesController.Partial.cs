//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;

using Rock.Data;
using Rock.Model;
using Rock.Rest.Filters;
using Rock.Web.Cache;

namespace Rock.Rest.Controllers
{
    public partial class CategoriesController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "CategoriesGetChildren",
                routeTemplate: "api/Categories/GetChildren/{id}/{getCategorizedItems}/{entityTypeId}/{entityqualifier}/{entityqualifiervalue}",
                defaults: new
                {
                    controller = "Categories",
                    action = "GetChildren",
                    entityqualifier = RouteParameter.Optional,
                    entityqualifiervalue = RouteParameter.Optional
                } );
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <param name="getCategorizedItems">if set to <c>true</c> [get categorized items].</param>
        /// <returns></returns>
        [Authenticate]
        public IQueryable<CategoryItem> GetChildren( int id, bool getCategorizedItems, int entityTypeId )
        {
            return GetChildren(id, getCategorizedItems, entityTypeId, null, null);
        }

        [Authenticate]
        public IQueryable<CategoryItem> GetChildren( int id, bool getCategorizedItems, int entityTypeId, string entityQualifier )
        {
            return GetChildren(id, getCategorizedItems, entityTypeId, entityQualifier, null);
        }

        [Authenticate]
        public IQueryable<CategoryItem> GetChildren( int id, bool getCategorizedItems, int entityTypeId, string entityQualifier, string entityQualifierValue )
        {
            var user = CurrentUser();
            Person currentPerson = user != null ? user.Person : null;

            IQueryable<Category> qry;
            qry = Get().Where( a => ( a.ParentCategoryId ?? 0 ) == id );

            object serviceInstance = null;

            var cachedEntityType = EntityTypeCache.Read( entityTypeId );
            if ( cachedEntityType != null )
            {
                qry = qry.Where( a => a.EntityTypeId == entityTypeId );
                if (!string.IsNullOrWhiteSpace(entityQualifier))
                {
                    qry = qry.Where( a => string.Compare(a.EntityTypeQualifierColumn, entityQualifier, true) == 0);
                    if ( !string.IsNullOrWhiteSpace( entityQualifierValue ) )
                    {
                        qry = qry.Where( a => string.Compare( a.EntityTypeQualifierValue, entityQualifierValue, true ) == 0 );
                    }
                    else
                    {
                        qry = qry.Where( a => a.EntityTypeQualifierValue == null || a.EntityTypeQualifierValue == "" );
                    }
                }

                // Get the GetByCategory method
                if ( cachedEntityType.AssemblyName != null )
                {
                    Type entityType = cachedEntityType.GetEntityType();
                    if ( entityType != null )
                    {
                        Type[] modelType = { entityType };
                        Type genericServiceType = typeof( Rock.Data.Service<> );
                        Type modelServiceType = genericServiceType.MakeGenericType( modelType );

                        serviceInstance = Activator.CreateInstance( modelServiceType );
                    }
                }
            }

            List<Category> categoryList = qry.ToList();
            List<CategoryItem> categoryItemList = new List<CategoryItem>();

            var appPath = System.Web.VirtualPathUtility.ToAbsolute( "~" );
            string imageUrlFormat = Path.Combine( appPath, "GetImage.ashx?id={0}&width=15&height=15" );

            foreach ( var category in categoryList )
            {
                if ( category.IsAuthorized( "View", currentPerson ) )
                {
                    var categoryItem = new CategoryItem();
                    categoryItem.Id = category.Id.ToString();
                    categoryItem.Name = System.Web.HttpUtility.HtmlEncode( category.Name );
                    categoryItem.IsCategory = true;

                    // if there a IconCssClass is assigned, use that as the Icon.  Otherwise, use the SmallIcon (if assigned)
                    if ( !string.IsNullOrWhiteSpace( category.IconCssClass ) )
                    {
                        categoryItem.IconCssClass = category.IconCssClass;
                    }
                    else
                    {
                        categoryItem.IconSmallUrl = category.IconSmallFileId != null ? string.Format( imageUrlFormat, category.IconSmallFileId ) : string.Empty;
                    }

                    categoryItemList.Add( categoryItem );
                }
            }

            if ( getCategorizedItems )
            {
                IQueryable items = GetCategorizedItems( serviceInstance, id ) as IQueryable;
                if ( items != null )
                {
                    foreach ( var item in items )
                    {
                        ICategorized categorizedItem = item as ICategorized;
                        if ( categorizedItem != null && categorizedItem.IsAuthorized( "View", currentPerson ) )
                        {
                            var categoryItem = new CategoryItem();
                            categoryItem.Id = categorizedItem.Id.ToString();
                            categoryItem.Name = categorizedItem.Name;
                            categoryItem.IsCategory = false;
                            categoryItem.IconCssClass = "icon-list-ol";
                            categoryItem.IconSmallUrl = string.Empty;
                            categoryItemList.Add( categoryItem );
                        }
                    }
                }
            }

            // try to figure out which items have viewable children
            foreach ( var g in categoryItemList )
            {
                if ( g.IsCategory )
                {
                    int parentId = int.Parse( g.Id );

                    foreach ( var childCategory in Get().Where( c => c.ParentCategoryId == parentId ) )
                    {
                        if ( childCategory.IsAuthorized( "View", currentPerson ) )
                        {
                            g.HasChildren = true;
                            break;
                        }
                    }

                    if ( !g.HasChildren )
                    {
                        if ( getCategorizedItems )
                        {
                            IQueryable childItems = GetCategorizedItems( serviceInstance, parentId ) as IQueryable;
                            if ( childItems != null )
                            {
                                foreach ( var item in childItems )
                                {
                                    ICategorized categorizedItem = item as ICategorized;
                                    if ( categorizedItem != null && categorizedItem.IsAuthorized( "View", currentPerson ) )
                                    {
                                        g.HasChildren = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return categoryItemList.AsQueryable();
        }

        /// <summary>
        /// Gets the categorized items.
        /// </summary>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        private object GetCategorizedItems( object serviceInstance, int categoryId )
        {
            if ( serviceInstance != null )
            {
                MethodInfo getMethod = serviceInstance.GetType().GetMethod( "Get", new Type[] { typeof( ParameterExpression ), typeof( Expression ) } );
                if ( getMethod != null )
                {
                    var paramExpression = serviceInstance.GetType().GetProperty( "ParameterExpression" ).GetValue( serviceInstance ) as ParameterExpression;
                    var propertyExpreesion = Expression.Property( paramExpression, "CategoryId" );
                    var zeroExpression = Expression.Constant( 0 );
                    var coalesceExpression = Expression.Coalesce( propertyExpreesion, zeroExpression );
                    var constantExpression = Expression.Constant( categoryId );
                    var compareExpression = Expression.Equal( coalesceExpression, constantExpression );

                    return getMethod.Invoke( serviceInstance, new object[] { paramExpression, compareExpression } );
                }
            }

            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CategoryItem : Rock.Web.UI.Controls.TreeViewItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is category.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is category; otherwise, <c>false</c>.
        /// </value>
        public bool IsCategory { get; set; }
    }
}
