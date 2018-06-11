using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CAVS.ProjectOrganizer.Project.Filtering;

namespace CAVS.ProjectOrganizer.Project.Aggregations.Spiral
{

    class ItemFilterComparer : IComparer
    {
        Dictionary<Item, Dictionary<Filter, bool>> itemFilterMappings;
        public ItemFilterComparer(Dictionary<Item, Dictionary<Filter, bool>> itemFilterMappings)
        {
            this.itemFilterMappings = itemFilterMappings;
        }

        public int Compare(System.Object x, System.Object y)
        {
            return FilterPassCount(itemFilterMappings[(Item)y]) - FilterPassCount(itemFilterMappings[(Item)x]);
        }

        private int FilterPassCount(Dictionary<Filter, bool> item)
        {
            int aCount = 0;
            foreach (var value in item.Values)
            {
                if (value)
                {
                    aCount++;
                }
            }
            return aCount;
        }

    }

    public class ItemSpiral : Graph
    {

        public ItemSpiral(Item[] items, Dictionary<Filter, Action<bool, GameObject>> filterGraphing, Func<GameObject> itemBuilder) : base(items, filterGraphing, itemBuilder)
        {
        }

        private GameObject GetSpiralContainerReference()
        {
            return Resources.Load<GameObject>("Cube Container");
        }

        public GameObject BuildPreview(Vector3 positionForPreview)
        {
            GameObject palace = GameObject.Instantiate(GetSpiralContainerReference(), Vector3.zero, Quaternion.identity);
            Filter f = (Filters.Count == 1 ? new AggregateFilter(Filters.ToArray()) : Filters[0]);
			
            Item[] filteredItems = f.FilterItems(items);
			if(f!= null) palace.GetComponent<SprialPreviewBehavior>().SetFilter(f);
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
            int itemsCreated = 0;
            float radius = 10;
            foreach (Item item in items)
            {
                Vector3 position = new Vector3(Mathf.Sin(itemsCreated) * radius, itemsCreated / 5, Mathf.Cos(itemsCreated) * radius);
                GameObject itemInstances = Plot(item, position);

                if (itemInstances != null)
                {
                    itemInstances.transform.position = position;
                    itemInstances.transform.parent = palace.transform;
                    itemInstances.transform.LookAt(Vector3.zero);
                    itemsCreated++;
                }
            }
            return palace;
        }

        /// <summary>
        /// UP FOR POTENTIAL DELETION
        /// </summary>
        /// <param name="itemsToSort"></param>
        /// <param name="filtersApplied"></param>
        /// <returns></returns>
        public Item[] SortItems(Item[] itemsToSort, Dictionary<Item, Dictionary<Filter, bool>> filtersApplied)
        {
            Item[] copy = (Item[])itemsToSort.Clone();
            Array.Sort(copy, new ItemFilterComparer(filtersApplied));
            return copy;
        }

    }

}