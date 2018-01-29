using System.Collections;
using System.Collections.Generic;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class AggregateFilter : Filter
    {

        Filter[] filters;

        public AggregateFilter(Filter[] filters)
        {
            this.filters = filters;
        }

        public override bool FilterItem(Item item)
        {
            bool passed = true;
            for (int filterIndex = 0; filterIndex < filters.Length; filterIndex++)
            {
                passed = filters[filterIndex].FilterItem(item);
                if (!passed)
                {
                    return false;
                }
            }
			return true;
        }

    }

}