using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project.Filtering;

namespace CAVS.ProjectOrganizer.Project.Aggregations.Spiral
{

    public class ItemSpiral
    {

        private Filter filter;

        private Item[] itemsToDisplay;

        public ItemSpiral(Item[] itemsToDisplay, Filter filter)
        {
            this.itemsToDisplay = itemsToDisplay;
            this.filter = filter;
        }

        private GameObject getSpiralContainerReference()
        {
            return Resources.Load<GameObject>("Spiral Container");
        }

        public GameObject BuildPreview(Vector3 positionForPreview)
        {
            GameObject palace = GameObject.Instantiate(getSpiralContainerReference(), Vector3.zero, Quaternion.identity);
            int i = 0;
            Item[] filteredItems = filter.FilterItems(itemsToDisplay);
            foreach (Item item in filteredItems)
            {
                GameObject node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                node.transform.parent = palace.transform;
                node.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                node.transform.position = new Vector3(Mathf.Sin(i) * .2f, -.35f + ((float)i / 400f), Mathf.Cos(i) * .2f);
                i++;
            }
            palace.GetComponent<SprialPreviewBehavior>().SetFilter(this.filter);
            palace.transform.position = positionForPreview;
            return palace;
        }


        /// <summary>
        /// Creates a large construction that some one can walk through.
        /// </summary>
        /// <returns>The palace that was constructed</returns>
        public GameObject BuildPalace()
        {
            GameObject palace = new GameObject("Palace");
            int i = 0;
            Item[] filteredItems = filter.FilterItems(itemsToDisplay);
            foreach (Item item in filteredItems)
            {
                ItemBehaviour itemBehavior = item.Build(new Vector3(Mathf.Sin(i) * 10, i / 5, Mathf.Cos(i) * 10), Vector3.zero);
                itemBehavior.transform.parent = palace.transform;
                itemBehavior.transform.LookAt(Vector3.zero);
                i++;
            }
            return palace;
        }

    }

}