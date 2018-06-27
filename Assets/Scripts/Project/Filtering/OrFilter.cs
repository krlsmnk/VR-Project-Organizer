using System;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class OrFilter : Filter, IEquatable<OrFilter>
    {

        Filter a;

        Filter b;

        public OrFilter(Filter a, Filter b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool FilterItem(Item item)
        {
            return this.a.FilterItem(item) || this.b.FilterItem(item);
        }

        public bool Equals(OrFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.a.Equals(other.a) && this.b.Equals(other.b);
        }
    }

}