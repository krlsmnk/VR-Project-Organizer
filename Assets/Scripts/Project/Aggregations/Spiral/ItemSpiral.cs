using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
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
            return filterPassCount(itemFilterMappings[(Item)y]) - filterPassCount(itemFilterMappings[(Item)x]);
        }

        private int filterPassCount(Dictionary<Filter, bool> item)
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

    public class ItemSpiral
    {

        private AggregateFilter filter;

        private Item[] itemsToDisplay;

        private Func<Item, Dictionary<Filter, bool>, ItemBehaviour> itemBuilder;

        public ItemSpiral(Item[] itemsToDisplay, Filter filter) : this(itemsToDisplay, new AggregateFilter(new Filter[] { filter }), null) { }

        public ItemSpiral(Item[] itemsToDisplay, AggregateFilter filter) : this(itemsToDisplay, filter, null) { }

        public ItemSpiral(Item[] itemsToDisplay, AggregateFilter filter, Func<Item, Dictionary<Filter, bool>, ItemBehaviour> itemBuilder)
        {
            this.itemsToDisplay = itemsToDisplay;
            this.filter = filter;
            this.itemBuilder = itemBuilder;
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
                node.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
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
            int itemsCreated = 0;
            Dictionary<Item, Dictionary<Filter, bool>> filtersPassedForItems = filter.FiltersPassed(itemsToDisplay);
            Item[] sortedItems = sortItems(itemsToDisplay, filtersPassedForItems);
            foreach (Item item in sortedItems)
            {
                ItemBehaviour itemBehavior = null;
                Vector3 position = new Vector3(Mathf.Sin(itemsCreated) * 10, itemsCreated / 5, Mathf.Cos(itemsCreated) * 10);
                Dictionary<Filter, bool> appliedFiltersToItem = filtersPassedForItems[item];
                if (itemBuilder != null)
                {
                    itemBehavior = itemBuilder(item, appliedFiltersToItem);
                }
                else if (appliedFiltersToItem.ContainsValue(true))
                {
                    itemBehavior = item.Build(position, Vector3.zero);
                }

                if (itemBehavior != null)
                {
                    itemBehavior.transform.position = position;
                    itemBehavior.transform.parent = palace.transform;
                    itemBehavior.transform.LookAt(Vector3.zero);
                    itemsCreated++;
                }
            }
            return palace;
        }

        public Item[] sortItems(Item[] itemsToSort, Dictionary<Item, Dictionary<Filter, bool>> filtersApplied)
        {
            Item[] copy = (Item[])itemsToSort.Clone();
            Array.Sort(copy, new ItemFilterComparer(filtersApplied));
            return copy;
        }

    }

}