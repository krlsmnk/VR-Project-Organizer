using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Project.Aggregations.Spiral;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    /// <summary>
    /// A filter prunes lists of items to make navigation of the informational 
    /// space more managable 
    /// </summary>
    public abstract class Filter
    {

        /// <summary>
        /// Determine whether or not the item would pass the filter
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool FilterItem(Item item);

        /// <summary>
        /// Builds a list of items that satisfy the filter
        /// </summary>
        /// <param name="unfilteredItems"></param>
        /// <returns></returns>
        public Item[] FilterItems(Item[] unfilteredItems)
        {
            if (unfilteredItems == null)
            {
                return null;
            }
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

		public GameObject Build(){
            GameObject thisFilter = GameObject.Instantiate(Resources.Load<GameObject> ("Cube Container"));
            thisFilter.gameObject.GetComponent<SprialPreviewBehavior>().SetFilter(this);
            return thisFilter;
		}//end of build

    }//end of class



}