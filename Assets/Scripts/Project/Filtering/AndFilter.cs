using System;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class AndFilter : Filter, IEquatable<AndFilter>
    {

        Filter a;

        Filter b;

        public AndFilter(Filter a, Filter b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool FilterItem(Item item)
        {
            return this.a.FilterItem(item) && this.b.FilterItem(item);
        }

        public bool Equals(AndFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return a.Equals(other.a) && b.Equals(other.b);
        }

    }

}