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
        /// Gets or sets the Entity Type Id.
        /// </summary>
        /// <value>
        /// Entity Type Id.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the start column.
        /// </summary>
        /// <value>
        /// The start column.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int StartColumn { get; set; }

        /// <summary>
        /// Gets or sets the start row.
        /// </summary>
        /// <value>
        /// The start row.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int StartRow { get; set; }

        /// <summary>
        /// Gets or sets the width of the widget.
        /// </summary>
        /// <value>
        /// The width of the widget.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int WidgetWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the widget.
        /// </summary>
        /// <value>
        /// The height of the widget.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int WidgetHeight { get; set; }

        /// <summary>
        /// Gets or sets the widget title.
        /// </summary>
        /// <value>
        /// The widget title.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public string WidgetTitle { get; set; }

        /// <summary>
        /// Gets or sets the widget description.
        /// </summary>
        /// <value>
        /// The widget description.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public string WidgetDescription { get; set; }

        /// <summary>
        /// Gets or sets the Person Id.
        /// </summary>
        /// <value>
        /// Person Id.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is system.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is system; otherwise, <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }

        // Metric component specific fields
        public int MetricTypeId { get; set; }

        public DateTime MetricStartDate { get; set; }

        public DateTime MetricEndDate { get; set; }

        // Twitter component specific fields
        // Facebook component specific fields

        #endregion
        
        #region Virtual Properties

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        [DataMember]
        public virtual Model.EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        [DataMember]
        public virtual Model.Person Person { get; set; }

        /// <summary>
        /// Gets or sets the metric.
        /// </summary>
        /// <value>
        /// The metric.
        /// </value>
        [DataMember]
        public virtual Model.Metric Metric { get; set; }

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
            this.HasRequired( p => p.EntityType ).WithMany().HasForeignKey( p => p.EntityTypeId ).WillCascadeOnDelete( false );
            this.HasRequired( p => p.Metric ).WithMany().HasForeignKey( p => p.MetricTypeId ).WillCascadeOnDelete( false );
            this.HasRequired( p => p.Person ).WithMany().HasForeignKey( p => p.PersonId ).WillCascadeOnDelete( false );
        }
    }

    #endregion
}
