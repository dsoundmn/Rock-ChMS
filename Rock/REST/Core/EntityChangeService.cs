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
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Rock.REST.Core
{
	/// <summary>
	/// REST WCF service for EntityChanges
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "Core/EntityChange")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class EntityChangeService : IEntityChangeService, IService
    {
		/// <summary>
		/// Gets a EntityChange object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Core.DTO.EntityChange Get( string id )
        {
            var currentUser = Rock.CMS.User.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
				Rock.Core.EntityChange EntityChange = EntityChangeService.Get( int.Parse( id ) );
				if ( EntityChange.Authorized( "View", currentUser ) )
					return EntityChange.DataTransferObject;
				else
					throw new WebFaultException<string>( "Not Authorized to View this EntityChange", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Gets a EntityChange object
		/// </summary>
		[WebGet( UriTemplate = "{id}/{apiKey}" )]
        public Rock.Core.DTO.EntityChange ApiGet( string id, string apiKey )
        {
            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
					Rock.Core.EntityChange EntityChange = EntityChangeService.Get( int.Parse( id ) );
					if ( EntityChange.Authorized( "View", user.UserName ) )
						return EntityChange.DataTransferObject;
					else
						throw new WebFaultException<string>( "Not Authorized to View this EntityChange", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Updates a EntityChange object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateEntityChange( string id, Rock.Core.DTO.EntityChange EntityChange )
        {
            var currentUser = Rock.CMS.User.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
				Rock.Core.EntityChange existingEntityChange = EntityChangeService.Get( int.Parse( id ) );
				if ( existingEntityChange.Authorized( "Edit", currentUser ) )
				{
					uow.objectContext.Entry(existingEntityChange).CurrentValues.SetValues(EntityChange);
					
					if (existingEntityChange.IsValid)
						EntityChangeService.Save( existingEntityChange, currentUser.PersonId );
					else
						throw new WebFaultException<string>( existingEntityChange.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this EntityChange", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Updates a EntityChange object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}/{apiKey}" )]
        public void ApiUpdateEntityChange( string id, string apiKey, Rock.Core.DTO.EntityChange EntityChange )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
					Rock.Core.EntityChange existingEntityChange = EntityChangeService.Get( int.Parse( id ) );
					if ( existingEntityChange.Authorized( "Edit", user.UserName ) )
					{
						uow.objectContext.Entry(existingEntityChange).CurrentValues.SetValues(EntityChange);
					
						if (existingEntityChange.IsValid)
							EntityChangeService.Save( existingEntityChange, user.PersonId );
						else
							throw new WebFaultException<string>( existingEntityChange.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this EntityChange", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Creates a new EntityChange object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateEntityChange( Rock.Core.DTO.EntityChange EntityChange )
        {
            var currentUser = Rock.CMS.User.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
				Rock.Core.EntityChange existingEntityChange = new Rock.Core.EntityChange();
				EntityChangeService.Add( existingEntityChange, currentUser.PersonId );
				uow.objectContext.Entry(existingEntityChange).CurrentValues.SetValues(EntityChange);

				if (existingEntityChange.IsValid)
					EntityChangeService.Save( existingEntityChange, currentUser.PersonId );
				else
					throw new WebFaultException<string>( existingEntityChange.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
            }
        }

		/// <summary>
		/// Creates a new EntityChange object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "{apiKey}" )]
        public void ApiCreateEntityChange( string apiKey, Rock.Core.DTO.EntityChange EntityChange )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
					Rock.Core.EntityChange existingEntityChange = new Rock.Core.EntityChange();
					EntityChangeService.Add( existingEntityChange, user.PersonId );
					uow.objectContext.Entry(existingEntityChange).CurrentValues.SetValues(EntityChange);

					if (existingEntityChange.IsValid)
						EntityChangeService.Save( existingEntityChange, user.PersonId );
					else
						throw new WebFaultException<string>( existingEntityChange.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a EntityChange object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteEntityChange( string id )
        {
            var currentUser = Rock.CMS.User.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
				Rock.Core.EntityChange EntityChange = EntityChangeService.Get( int.Parse( id ) );
				if ( EntityChange.Authorized( "Edit", currentUser ) )
				{
					EntityChangeService.Delete( EntityChange, currentUser.PersonId );
					EntityChangeService.Save( EntityChange, currentUser.PersonId );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this EntityChange", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a EntityChange object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}/{apiKey}" )]
        public void ApiDeleteEntityChange( string id, string apiKey )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.EntityChangeService EntityChangeService = new Rock.Core.EntityChangeService();
					Rock.Core.EntityChange EntityChange = EntityChangeService.Get( int.Parse( id ) );
					if ( EntityChange.Authorized( "Edit", user.UserName ) )
					{
						EntityChangeService.Delete( EntityChange, user.PersonId );
						EntityChangeService.Save( EntityChange, user.PersonId );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this EntityChange", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

    }
}
