using System.Collections;
using System.Collections.Generic;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

	public class RangeFilterNum : Filter
	{

		public enum Operator
		{
			// returns: min < x < max
			InsideExclusive,
			//returns: min <= x <= max
			InsideInclusive,
			//returns x < min OR x > max
			Outside
		}

		/// <summary>
		/// How we're going to perform the filter
		/// </summary>
		Operator op;

		float min;
		float max;

		/// <summary>
		/// The field the item might contain that we want to determine whether or not it get's pruned 
		/// </summary>
		protected string fieldToFilterOn;


		public RangeFilterNum(string fieldToFilterOn, Operator op, float min, float max)
		{
			this.fieldToFilterOn = fieldToFilterOn;
			this.op = op;
			this.min = min;
			this.max = max;
		}

		public override bool FilterItem(Item item)
		{
			//gets the field in question
			string val = item.GetValue(this.fieldToFilterOn);
			if (val == null)
			{
				return false;
			}
			//try to cast that string to a float for use in comparisons
			float result;
			float.TryParse(val, out result);

			//actually check the result
			return passes(result);
		}

		private bool passes(float x)
		{
			switch (op)
			{
			case Operator.InsideExclusive:
				return min < x && x < max;

			case Operator.InsideInclusive:
				return min <= x && x <= max;

			case Operator.Outside:
				return x < min || x > max;
			}
			return false;
		}

	}

}