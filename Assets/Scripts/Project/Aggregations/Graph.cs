using System;
using UnityEngine;
using System.Collections.Generic;
using CAVS.ProjectOrganizer.Project.Filtering;

namespace CAVS.ProjectOrganizer.Project.Aggregations
{

    public abstract class Graph
    {

        protected Item[] items;

        protected Dictionary<Filter, Func<bool, GameObject, GameObject>> filterGraphing;

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

        public Graph(Item[] items, Dictionary<Filter, Func<bool, GameObject, GameObject>> filtersWithGraphing)
        {
            this.items = items;
            this.filterGraphing = filtersWithGraphing;
        }

        public Graph(Item[] items, Filter[] filters)
        {
            this.items = items;
            this.filterGraphing = BuildNullMapping(filters);
        }

        protected GameObject Plot(Item item, Vector3 position)
        {
            GameObject resultingObject = new GameObject();

            foreach (var filterAndGrapher in filterGraphing)
            {
                resultingObject = filterAndGrapher.Value == null ? DefaultFilterGrapher(filterAndGrapher.Key.FilterItem(item), resultingObject) : filterAndGrapher.Value(filterAndGrapher.Key.FilterItem(item), resultingObject);
            }

            if(filterGraphing.Count == 0)
            {
                resultingObject = DefaultFilterGrapher(true, resultingObject);
            }

            if (ObjectIsEmpty(resultingObject))
            {
                GameObject.Destroy(resultingObject);
                return null;
            }

            return resultingObject;
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
            return obj == null ? true : obj.GetComponents<Component>().Length == 1 && obj.transform.childCount == 0;
        }

        private GameObject DefaultFilterGrapher(bool passed, GameObject gameobject)
        {
            if (passed && gameobject.transform.childCount == 0)
            {
                var child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                child.transform.parent = gameobject.transform;
                child.transform.localPosition = Vector3.zero;
            }
            return gameobject;
        }

    }

}