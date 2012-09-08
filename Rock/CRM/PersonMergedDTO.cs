//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;

using Rock.Data;

namespace Rock.Crm
{
	/// <summary>
	/// Data Transfer Object for PersonMerged object
	/// </summary>
	public partial class PersonMergedDto : Dto<PersonMerged>
	{

#pragma warning disable 1591
		public int CurrentId { get; set; }
		public Guid CurrentGuid { get; set; }
#pragma warning restore 1591

		/// <summary>
		/// Instantiates a new DTO object
		/// </summary>
		public PersonMergedDto ()
		{
		}

		/// <summary>
		/// Instantiates a new DTO object from the model
		/// </summary>
		/// <param name="personMerged"></param>
		public PersonMergedDto ( PersonMerged personMerged )
		{
			CopyFromModel( personMerged );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="personMerged"></param>
		public override void CopyFromModel( PersonMerged personMerged )
		{
			this.CurrentId = personMerged.CurrentId;
			this.CurrentGuid = personMerged.CurrentGuid;
			this.CreatedDateTime = personMerged.CreatedDateTime;
			this.CreatedByPersonId = personMerged.CreatedByPersonId;
			this.Id = personMerged.Id;
			this.Guid = personMerged.Guid;
		}

		/// <summary>
		/// Copies the DTO property values to the model properties
		/// </summary>
		/// <param name="personMerged"></param>
		public override void CopyToModel ( PersonMerged personMerged )
		{
			personMerged.CurrentId = this.CurrentId;
			personMerged.CurrentGuid = this.CurrentGuid;
			personMerged.CreatedDateTime = this.CreatedDateTime;
			personMerged.CreatedByPersonId = this.CreatedByPersonId;
			personMerged.Id = this.Id;
			personMerged.Guid = this.Guid;
		}
	}
}