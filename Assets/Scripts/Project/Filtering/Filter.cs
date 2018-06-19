using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Project.Aggregations.Spiral;
using UnityEngine.UI;
using VRTK;


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

		public GameObject Build(string filterName){
			GameObject thisFilter = GameObject.Instantiate(Resources.Load<GameObject> ("Cube Container"));
			thisFilter.gameObject.GetComponent<SprialPreviewBehavior>().SetFilter(this);

			//add tooltip to filter
			GameObject tooltip = GameObject.Instantiate(Resources.Load<GameObject> ("ObjectTooltip"));

			//put the canvas on / near the filter
			tooltip.gameObject.transform.position = thisFilter.transform.position;
			//move it up a bit so we can see it
			tooltip.gameObject.transform.position += new Vector3(0, .25f, 0);

			//parent the canvas to the filter
			tooltip.gameObject.transform.parent = thisFilter.transform;

			//find all text objects
			Text[] arrayOfTexts = GameObject.FindObjectsOfType<Text>();

			foreach (Text thisText in arrayOfTexts)
			{	
				//find the Text object who belongs to the current filter
				if (thisText.gameObject.transform.IsChildOf (thisFilter.transform)) {
					//assign its name
					thisText.text = filterName;
				}
			}

			//Draw the line from the tooltip to the filter
			/*
			VRTK_ObjectTooltip thisScript = tooltip.GetComponent<VRTK_ObjectTooltip> ();
			thisScript.drawLineFrom = tooltip.transform;
			thisScript.drawLineTo = thisFilter.transform;
			*/

			return thisFilter;
		}//end of build



    }//end of class



}