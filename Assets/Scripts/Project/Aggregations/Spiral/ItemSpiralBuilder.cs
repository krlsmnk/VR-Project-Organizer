using UnityEngine;
using System;
using System.Collections.Generic;
using CAVS.ProjectOrganizer.Project.Filtering;

namespace CAVS.ProjectOrganizer.Project.Aggregations.Spiral
{

    /// <summary>
    /// Builder class for Item Spiral.
    /// 
    /// Ensures no duplicate items
    /// </summary>
    public class ItemSpiralBuilder 
    {

        HashSet<Item> items;

        Func<GameObject> itemBuilder;

        Dictionary<Filter, Action<bool, GameObject>> filterGraphing;

        public ItemSpiralBuilder()
        {
            itemBuilder = null;
            filterGraphing = new Dictionary<Filter, Action<bool, GameObject>>();
            items = new HashSet<Item>();
        }

        public ItemSpiral Build()
        {
            Item[] finalizedItems = new Item[items.Count];
            items.CopyTo(finalizedItems);
            return new ItemSpiral(finalizedItems, filterGraphing, itemBuilder);
        }

        public ItemSpiralBuilder AddFilter(Filter filter, Action<bool, GameObject> plotModifier)
        {
            if (filterGraphing.ContainsKey(filter))
            {
                filterGraphing[filter] = plotModifier;
            } else
            {
                filterGraphing.Add(filter, plotModifier);
            }
            return this;
        }

        public ItemSpiralBuilder AddFilter(Filter filter)
        {
            if (filterGraphing.ContainsKey(filter))
            {
                filterGraphing[filter] = null;
            }
            else
            {
                filterGraphing.Add(filter, null);
            }
            return this;
        }

        public ItemSpiralBuilder AddFilters(Filter[] filters)
        {
            foreach(Filter f in filters)
            {
                if (filterGraphing.ContainsKey(f))
                {
                    filterGraphing[f] = null;
                }
                else
                {
                    filterGraphing.Add(f, null);
                }
            }
            return this;
        }

        public ItemSpiralBuilder AddItems(Item[] itemsToAdd)
        {
            foreach (Item item in itemsToAdd)
            {
                if(item != null)
                {
                    items.Add(item);
                }
            }
            return this;
        }

        public ItemSpiralBuilder AddItem(Item item)
        {
            if (item != null)
            {
                items.Add(item);
            }
            return this;
        }

        public ItemSpiralBuilder ClearFilters()
        {
            filterGraphing.Clear();
            return this;
        }

    }

}