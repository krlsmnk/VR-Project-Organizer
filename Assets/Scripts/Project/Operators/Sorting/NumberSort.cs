using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project.Operators.Sorting
{

    public class NumberSort : Sort, IComparer<Item>
    {

        Order ordertoSortBy;

        string fieldToSortOn;

        public enum Order
        {
            Ascending,
            Descending
        }

        public NumberSort(Order order, string field)
        {
            this.ordertoSortBy = order;
            this.fieldToSortOn = field;
        }

        public override List<Item> SortItems(Item[] items)
        {
            List<Item> sortedArray = new List<Item>(items);
            sortedArray.Sort(this);
            return sortedArray;
        }

        public int Compare(Item x, Item y)
        {
            return GetNumber(x) - GetNumber(y);
        }

        private int GetNumber(Item item)
        {
            if (item == null)
            {
                return NullNumber();
            }
            string val = item.GetValue(this.fieldToSortOn);
            if (val == null)
            {
                return NullNumber();
            }
            float result;
            if (float.TryParse(val, out result))
            {
                return (int)result;
            }
            else
            {
                return NullNumber();
            }
        }

        private int NullNumber()
        {
            return this.ordertoSortBy == Order.Ascending ? -1 : 1;
        }

    }

}