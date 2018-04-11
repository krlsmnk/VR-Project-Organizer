using System;
using UnityEngine;
using System.Collections.Generic;
using CAVS.ProjectOrganizer.Project.Filtering;

namespace CAVS.ProjectOrganizer.Project.Aggregations
{

    public abstract class Graph
    {

        protected Item[] items;

        private Dictionary<Filter, Func<bool, GameObject, GameObject>> filtersWithGraphing;

        public Graph(Item[] items, Dictionary<Filter, Func<bool, GameObject, GameObject>> filtersWithGraphing)
        {
            this.items = items;
            this.filtersWithGraphing = filtersWithGraphing;
        }

        public Graph(Item[] items, Filter[] filters)
        {
            this.items = items;
            this.filtersWithGraphing = BuildNullMapping(filters);
        }

        protected GameObject Plot(Item item, Vector3 position)
        {
            GameObject resultingObject = new GameObject();
            foreach (var filterAndGrapher in filtersWithGraphing)
            {
                resultingObject = filterAndGrapher.Value(filterAndGrapher.Key.FilterItem(item), resultingObject);
            }
            return ObjectIsEmpty(resultingObject) ? null : resultingObject;
        }

        private Dictionary<Filter, Func<bool, GameObject, GameObject>> BuildNullMapping(Filter[] filters)
        {
            var mappings = new Dictionary<Filter, Func<bool, GameObject, GameObject>>();
            foreach (var filter in filters)
            {
                mappings.Add(filter, null);
            }
            return mappings;
        }

        private bool ObjectIsEmpty(GameObject obj)
        {
            return obj == null ? true : obj.GetComponents<Component>().Length == 1;
        }
    }

}