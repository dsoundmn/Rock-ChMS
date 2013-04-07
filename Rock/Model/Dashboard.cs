//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    [Table( "Dashboard" )]
    [DataContract( IsReference = true )]
    public partial class Dashboard : Model<Dashboard>
    {
        #region Entity Properties
        
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public int MetricId { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public int PersonId { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public string Description { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public DateTime? EndDate { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public int Size { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public int Order { get; set; }
        
        #endregion
        
        #region Virtual Properties
        
        [DataMember]
        public virtual Model.Metric Metric { get; set; }

        [DataMember]
        public virtual Model.Person Person { get; set; }
        
        #endregion
        
        #region Public Methods
        #endregion

    }

    #region Entity Configuration
    
    public partial class DashboardConfiguration : EntityTypeConfiguration<Dashboard>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardConfiguration"/> class.
        /// </summary>
        public DashboardConfiguration()
        {
            //this.HasRequired( p => p.Metric ).WithMany( p => p.Dashboard ).HasForeignKey( p => p.MetricId ).WillCascadeOnDelete( true );
            //this.HasRequired( p => p.Person ).WithMany( p => p.Dashboard ).HasForeignKey( p => p.PersonId ).WillCascadeOnDelete( true );
            this.HasRequired( p => p.Metric ).WithMany().HasForeignKey( p => p.MetricId ).WillCascadeOnDelete( true );
            this.HasRequired( p => p.Person ).WithMany().HasForeignKey( p => p.PersonId ).WillCascadeOnDelete( true );
        }
    }

    #endregion

}
