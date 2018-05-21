using System;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class StringFilter : Filter, IEquatable<StringFilter>
    {

        public enum Operator
        {
            Equal,
            NotEqual
        }

        private string fieldName;

        private string stringToFilterOn;

        private Operator op;

        public StringFilter(string fieldName, Operator op, string stringToFilterOn)
        {
            this.fieldName = fieldName;
            this.stringToFilterOn = stringToFilterOn;
            this.op = op;
        }

        public override bool FilterItem(Item item)
        {
            if (item == null)
            {
                return false;
            }
            string val = item.GetValue(this.fieldName);
            if (val == null)
            {
                return false;
            }
            switch (op)
            {
                case Operator.Equal:
                    return val.ToLower().Equals(stringToFilterOn.ToLower());
                case Operator.NotEqual:
                    return !val.ToLower().Equals(stringToFilterOn.ToLower());
                default:
                    throw new System.Exception("Not Implemented");
            }
        }

        public bool Equals(StringFilter other)
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
                this.fieldName.Equals(other.fieldName) &&
                this.stringToFilterOn.Equals(other.stringToFilterOn);
        }

        public override string ToString()
        {
            return String.Format("String Filter( item[{0}] {1} {2} )", this.fieldName, this.op, this.stringToFilterOn);
        }

    }

}