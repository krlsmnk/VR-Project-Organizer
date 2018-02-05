using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.Filtering;
using CAVS.ProjectOrganizer.Project.Aggregations.Spiral;

namespace CAVS.ProjectOrganizer.Scenes.Testing.FilteringTesting
{

    /// <summary>
    /// Example class with how'd you would create your own custom node renders
    /// </summary>
    public class ColorGameMediator : GameMediator
    {

        private GameObject palace;

        private ItemBehaviour itemBuilder(Item item, Dictionary<Filter, bool> filters)
        {
            GameObject fakeNode = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            int currentlyFiltered = 0;

            foreach (KeyValuePair<Filter, bool> b in filters)
            {
                if (b.Value == false)
                {
                    currentlyFiltered++;
                }
            }

            fakeNode.transform.localScale = Vector3.one * (1f / (float)(currentlyFiltered + 1));

            return fakeNode.AddComponent<ItemBehaviour>();
        }

        protected override void displayPalace()
        {
            if (palace != null)
            {
                Destroy(palace);
            }
            palace = new ItemSpiral(allItems, new AggregateFilter(appliedFilters.ToArray()), itemBuilder).BuildPalace();
        }
    }

}