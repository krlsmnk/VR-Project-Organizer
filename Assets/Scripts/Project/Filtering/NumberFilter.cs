using System;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class NumberFilter : Filter, IEquatable<NumberFilter>
    {

        public enum Operator
        {
            /// <summary>
            /// Return all items less than a certain value
            /// </summary>
            LessThan,

            /// <summary>
            /// Return all items greater than a certain value
            /// </summary>
            GreaterThan,

            /// <summary>
            /// Return all items equal to a certain value
            /// </summary>
            EqualTo
        }

        /// <summary>
        /// How we're going to perform the filter
        /// </summary>
        Operator op;

        float value;

        /// <summary>
        /// The field the item might contain that we want to determine whether or not it get's pruned 
        /// </summary>
        protected string fieldToFilterOn;


        public NumberFilter(string fieldToFilterOn, Operator op, float value)
        {
            this.fieldToFilterOn = fieldToFilterOn;
            this.op = op;
            this.value = value;
        }

        public override bool FilterItem(Item item)
        {
            if (item == null)
            {
                return false;
            }
            string val = item.GetValue(this.fieldToFilterOn);
            if (val == null)
            {
                return false;
            }
            float result;
            float.TryParse(val, out result);
            return passes(result);
        }

        private bool passes(float num)
        {
            switch (op)
            {
                case Operator.GreaterThan:
                    return num > value;

                case Operator.LessThan:
                    return num < value;

                case Operator.EqualTo:
                    return num == value;
            }
            return false;
        }

        public override string ToString()
        {
            string operatorSymbol = "";
            switch (this.op)
            {
                case Operator.EqualTo:
                    operatorSymbol = "=";
                    break;

                case Operator.GreaterThan:
                    operatorSymbol = ">";
                    break;

                case Operator.LessThan:
                    operatorSymbol = "<";
                    break;

                default:
                    operatorSymbol = "";
                    break;
            }
            return string.Format("Filtering {0} by numbers {1} {2}", fieldToFilterOn, operatorSymbol, this.value);
        }

        public bool Equals(NumberFilter other)
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
                this.value.Equals(other.value) &&
                this.fieldToFilterOn.Equals(other.fieldToFilterOn);
        }

    }

}