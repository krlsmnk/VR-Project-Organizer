using System;
using UnityEngine;
using System.Collections.Generic;
using CAVS.ProjectOrganizer.Project.Filtering;

namespace CAVS.ProjectOrganizer.Project.Aggregations
{

    /// <summary>
    /// Graphs consits of plots.
    /// 
    /// First we built the item we're going to plot
    ///     itemBuilder
    /// 
    /// Then we change how that plot looks based on the filters it passes
    ///     filterGraphing
    /// </summary>
    public abstract class Graph
    {

        private static GameObject nodeReference;

        private static GameObject GetNodeReference()
        {
            if(nodeReference == null)
            {
                nodeReference = Resources.Load<GameObject>("Node");
            }
            return nodeReference;
        }

        protected Item[] items;

        protected Dictionary<Filter, Action<bool, GameObject>> filterGraphing;

        /// <summary>
        /// Function called to get our original item
        /// </summary>
        private Func<GameObject> itemBuilder;

        protected List<Filter> Filters
        {
            get
            {
                List<Filter> filters = new List<Filter>();
                foreach (var filter in filterGraphing.Keys)
                {
                    filters.Add(filter);
                }
                return filters;
            }
        }

        public Graph(Item[] items, Dictionary<Filter, Action<bool, GameObject>> filterGraphing, Func<GameObject> itemBuilder)
        {
            this.items = items;
            this.filterGraphing = filterGraphing;
            this.itemBuilder = itemBuilder;
        }

        public Graph(Item[] items, Filter[] filters)
        {
            this.items = items;
            filterGraphing = BuildNullMapping(filters);
        }

        protected GameObject Plot(Item item, Vector3 position)
        {
            GameObject resultingObject = itemBuilder != null? itemBuilder() : DefaultItemBuilder();

            foreach (var filterAndGrapher in filterGraphing)
            {
                if (filterAndGrapher.Value == null) {
                    DefaultFilterGrapher(filterAndGrapher.Key.FilterItem(item), resultingObject);
                } else {
                    filterAndGrapher.Value(filterAndGrapher.Key.FilterItem(item), resultingObject);
                }
            }

            if(filterGraphing.Count == 0)
            {
                DefaultFilterGrapher(true, resultingObject);
            }

            if (ObjectIsEmpty(resultingObject))
            {
                UnityEngine.Object.Destroy(resultingObject);
                return null;
            }

            return resultingObject;
        }

        private Dictionary<Filter, Action<bool, GameObject>> BuildNullMapping(Filter[] filters)
        {
            var mappings = new Dictionary<Filter, Action<bool, GameObject>>();
            foreach (var filter in filters)
            {
                mappings.Add(filter, null);
            }
            return mappings;
        }

        private bool ObjectIsEmpty(GameObject obj)
        {
            return obj == null ? true : obj.GetComponents<Component>().Length == 1 && obj.transform.childCount == 0;
        }

        private void DefaultFilterGrapher(bool passed, GameObject gameobject)
        {
            if(gameobject == null)
            {
                return;
            }

            if (!passed)
            {
                UnityEngine.Object.Destroy(gameobject);
            }
        }

        private GameObject DefaultItemBuilder()
        {
            return UnityEngine.Object.Instantiate(GetNodeReference());
        }

    }

}