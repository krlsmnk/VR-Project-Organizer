using System;

namespace CAVS.ProjectOrganizer.Project.Filtering

{

    public class RangeFilterString : Filter, IEquatable<RangeFilterString>
    {

        public enum Operator
        {
            /// <summary>
            /// min < x < max
            /// </summary>
            InsideExclusive,

            /// <summary>
            /// min <= x <= max
            /// </summary>
            InsideInclusive,

            /// <summary>
            /// x < min OR x > max
            /// </summary>
            Outside
        }

        /// <summary>
        /// How we're going to perform the filter
        /// </summary>
        Operator op;

        string min;
        string max;

        /// <summary>
        /// The field the item might contain that we want to determine whether or not it get's pruned 
        /// </summary>
        protected string fieldToFilterOn;


        public RangeFilterString(string fieldToFilterOn, Operator op, string min, string max)
        {
            this.fieldToFilterOn = fieldToFilterOn;
            this.op = op;
            this.min = min;
            this.max = max;
        }

        public override bool FilterItem(Item item)
        {
            // gets the field in question
            string val = item.GetValue(this.fieldToFilterOn);
            if (val == null)
            {
                return false;
            }

            // actually check the result
            return Passes(val);
        }

        private bool Passes(string x)
        {
            string[] testArray = new string[3];
            testArray[0] = min;
            testArray[1] = x;
            testArray[2] = max;

            // sorts the 3 strings alphabetically
            Array.Sort(testArray, StringComparer.InvariantCulture);
            
            // gets the index of our string after sorting
            // NOTE: since IndexOf returns the FIRST occurence of the value, x could be equal to min, so we must check both
            int xIndex = Array.IndexOf(testArray, x);

            switch (op)
            {
                case Operator.InsideExclusive:
                    // if our string is in the middle position, it's inside the range
                    return xIndex == 1;

                case Operator.InsideInclusive:
                    // if our string is in first position, but is equal to the "min", it's inside the range
                    if (xIndex == 0 && testArray[0].Equals(testArray[1]))
                    {
                        return true;
                    }
                    // if our string is in the middle position, it's inside the range
                    else return xIndex == 1;

                case Operator.Outside:
                    // if our string is in first position, but is equal to the "min", not outside the range
                    if (xIndex == 0 && testArray[0].Equals(testArray[1]))
                    {
                        return false;
                    }
                    else
                    {
                        return xIndex == 0 || xIndex == 2;
                    }
            }

            return false;
        }

        public bool Equals(RangeFilterString other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.op.Equals(other.op) &&
                this.min.Equals(other.min) &&
                this.max.Equals(other.max) &&
                this.fieldToFilterOn.Equals(other.fieldToFilterOn);
        }

    }

}