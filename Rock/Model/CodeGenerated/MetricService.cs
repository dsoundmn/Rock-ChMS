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
    /// Represents a numerically measurable statistic or Metric in RockChMS.  An example of a metric can include 
    /// weekly membership count, number of children who check-in to preschool, etc.
    /// </summary>
    public partial class MetricService : Service<Metric>
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
        /// <param name="repository">The repository.</param>
        public MetricService(IRepository<Metric> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public MetricService(RockContext context) : base(context)
        {
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

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class MetricExtensionMethods
    {
        /// <summary>
        /// Clones this Metric object to a new Metric object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Metric Clone( this Metric source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Metric;
            }
            else
            {
                var target = new Metric();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another Metric object to this Metric object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Metric target, Metric source )
        {
            target.IsSystem = source.IsSystem;
            target.Type = source.Type;
            target.Category = source.Category;
            target.Title = source.Title;
            target.Subtitle = source.Subtitle;
            target.Description = source.Description;
            target.MinValue = source.MinValue;
            target.MaxValue = source.MaxValue;
            target.CollectionFrequencyValueId = source.CollectionFrequencyValueId;
            target.LastCollectedDateTime = source.LastCollectedDateTime;
            target.Source = source.Source;
            target.SourceSQL = source.SourceSQL;
            target.Order = source.Order;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
