using System;
using System.Collections.Generic;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    /// <summary>
    /// An aggregation of multiple filters, an easy way to apply multiple filters
    /// to a list of items.
    /// </summary>
    public class AggregateFilter : Filter, IEquatable<AggregateFilter>
    {

        private List<Filter> filters;

        public AggregateFilter(Filter[] filters)
        {
            this.filters = new List<Filter>(filters);
        }


        public override bool FilterItem(Item item)
        {
            bool passed = true;
            for (int filterIndex = 0; filterIndex < filters.Count; filterIndex++)
            {
                passed = filters[filterIndex].FilterItem(item);
                if (!passed)
                {
                    return false;
                }
            }
            return true;
        }

        public Dictionary<Item, Dictionary<Filter, bool>> FiltersPassed(Item[] unfilteredItems)
        {
            if (unfilteredItems == null)
            {
                return null;
            }

            Dictionary<Item, Dictionary<Filter, bool>> applied = new Dictionary<Item, Dictionary<Filter, bool>>();
            foreach (Item item in unfilteredItems)
            {
                applied.Add(item, new Dictionary<Filter, bool>());
                foreach (Filter filter in this.filters)
                {
                    applied[item].Add(filter, filter.FilterItem(item));
                }

            }
            return applied;
        }

        public bool Equals(AggregateFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if(this.filters.Count != other.filters.Count)
            {
                return false;
            }

            foreach(var filter in this.filters)
            {
                if (!other.filters.Contains(filter))
                {
                    return false;
                }
            }

            return true;
        }

    }

}