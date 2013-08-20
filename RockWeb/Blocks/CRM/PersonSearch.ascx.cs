//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

using Rock;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Crm
{
    public partial class PersonSearch : Rock.Web.UI.RockBlock
    {
        #region Control Methods

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            BindGrid();
        }

        #endregion

        #region Internal Methods

        private void BindGrid()
        {
            string type = PageParameter( "SearchType" );
            string term = PageParameter( "SearchTerm" );

            var personSpouseList = new List<PersonService.PersonWithSpouse>();

            if ( !String.IsNullOrWhiteSpace( type ) && !String.IsNullOrWhiteSpace( term ) )
            {
                using ( var uow = new Rock.Data.UnitOfWorkScope() )
                {
                    var personService = new PersonService();
                    var personSpouseQuery = personService.QueryableWithSpouse();

                    switch ( type.ToLower() )
                    {
                        case ( "name" ):

                            var peopleIds = personService.GetByFullName( term, true ).Select( a => a.Id ).ToList();
                            personSpouseQuery = personSpouseQuery.Where( a => peopleIds.Contains( a.Person.Id ) );
                            personSpouseList = personSpouseQuery.OrderBy(a => a.Person.FullNameLastFirst).ToList();

                            break;

                        case ( "phone" ):

                            var phoneService = new PhoneNumberService();

                            var personIds = phoneService.Queryable().
                                Where( n => n.Number.Contains( term ) ).
                                Select( n => n.PersonId ).Distinct();

                            personSpouseQuery = personSpouseQuery.Where( p => personIds.Contains( p.Person.Id ) );
                            personSpouseList = personSpouseQuery.OrderBy(a => a.Person.FullNameLastFirst).ToList();

                            break;

                        case ( "address" ):

                            break;

                        case ( "email" ):

                            personSpouseQuery = personSpouseQuery.Where( p => p.Person.Email.Contains( term ) );

                            personSpouseList = personSpouseQuery.OrderBy(a => a.Person.FullNameLastFirst).ToList();

                            break;
                    }
                }
            }

            
            
            if ( personSpouseList.Count == 1 )
            {
                Response.Redirect( string.Format( "~/Person/{0}", personSpouseList[0].Person.Id ), false );
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                gPeople.DataSource = personSpouseList;
                gPeople.DataBind();
            }
        }

        #endregion

    }
}