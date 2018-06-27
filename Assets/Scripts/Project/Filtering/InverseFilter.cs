using System;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class InverseFilter : Filter, IEquatable<InverseFilter>
    {

        Filter filterToInverse;

        public InverseFilter(Filter filterToInverse)
        {
            this.filterToInverse = filterToInverse;
        }

        public override bool FilterItem(Item item)
        {
            return !this.filterToInverse.FilterItem(item);
        }

        public bool Equals(InverseFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.filterToInverse.Equals(other.filterToInverse);
        }

    }

}