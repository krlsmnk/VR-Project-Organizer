using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project.Operators.Sorting
{
    public class StringSort : Sort, IComparer<Item>
    {

        Order ordertoSortBy;

        string fieldToSortOn;

        public enum Order
        {
            Ascending,
            Descending
        }

        public StringSort(Order order, string field)
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
            string xString = x != null ? x.GetValue(fieldToSortOn) : null;
            string yString = y != null ? y.GetValue(fieldToSortOn) : null;
            return this.ordertoSortBy == Order.Ascending ? string.Compare(xString, yString) : string.Compare(yString, xString);
        }

        
    }

}