using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    /// <summary>
    /// A filter prunes lists of items to make navigation of the informational space more managable 
    /// </summary>
    public abstract class Filter
    {


        public abstract bool FilterItem(Item item);

        public Item[] FilterItems(Item[] unfilteredItems)
        {
            List<Item> filterdItems = new List<Item>();
            foreach (Item item in unfilteredItems)
            {
                if (FilterItem(item))
                {
                    filterdItems.Add(item);
                }
            }
            return filterdItems.ToArray();
        }

    }

}