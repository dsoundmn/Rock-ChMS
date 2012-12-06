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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Metric Service class
    /// </summary>
    public partial class MetricService : Service<Metric, MetricDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricService"/> class
        /// </summary>
        public MetricService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricService"/> class
        /// </summary>
        public MetricService(IRepository<Metric> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Metric CreateNew()
        {
            return new Metric();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<MetricDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<MetricDto> QueryableDto( IQueryable<Metric> items )
        {
            return items.Select( m => new MetricDto()
                {
                    IsSystem = m.IsSystem,
                    Type = m.Type,
                    Category = m.Category,
                    Title = m.Title,
                    Subtitle = m.Subtitle,
                    Description = m.Description,
                    MinValue = m.MinValue,
                    MaxValue = m.MaxValue,
                    CollectionFrequencyValueId = m.CollectionFrequencyValueId,
                    LastCollected = m.LastCollected,
                    Source = m.Source,
                    SourceSQL = m.SourceSQL,
                    Order = m.Order,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( Metric item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}