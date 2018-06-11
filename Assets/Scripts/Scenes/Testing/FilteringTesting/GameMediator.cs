using System;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.Filtering;
using CAVS.ProjectOrganizer.Project.Aggregations.Spiral;

namespace CAVS.ProjectOrganizer.Scenes.Testing.FilteringTesting
{

    public class GameMediator : MonoBehaviour
    {

        protected Item[] allItems;

        protected List<Filter> appliedFilters;

        protected List<Action<bool, GameObject>> appliedModifiers;

        private GameObject currentPalace;

        // Use this for initialization
        void Start()
        {
            appliedFilters = new List<Filter>();
            appliedModifiers = new List<Action<bool, GameObject>>();
            allItems = ProjectFactory.BuildItemsFromCSV("CarData.csv", 7);
            Filter[] filters = new Filter[]{
                new NumberFilter("Year",  NumberFilter.Operator.GreaterThan, 1999),
                //new NumberFilter("Year",  NumberFilter.Operator.LessThan, 2007),
                //new StringFilter("Model", StringFilter.Operator.Equal, "ES")
            };

            var builder = new ItemSpiralBuilder()
                .AddItems(allItems)
                .AddFilter(filters[0])
                .Build()
                .BuildPreview(transform.position + (Vector3.up*2));
        }


        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
			if (!other.gameObject.name.Contains("Cube"))
				return;
			
				SprialPreviewBehavior preview = other.gameObject.GetComponent<SprialPreviewBehavior>();
	            if (preview != null)
	            {
	                preview.gameObject.GetComponent<MeshRenderer>().material.color = Color.green * .5f;
	                appliedFilters.Add(preview.GetFilter());
                    appliedModifiers.Add(preview.GetPlotModifier());
	                DisplayPalace();
	            }
		}


        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            SprialPreviewBehavior preview = other.gameObject.GetComponent<SprialPreviewBehavior>();
            if (preview != null)
            {
                preview.gameObject.GetComponent<MeshRenderer>().material.color = new Color(54.0f / 255.0f, 172.0f / 255.0f, 1, 100.0f / 255.0f);
                int indexToRemove = appliedFilters.IndexOf(preview.GetFilter());
                appliedFilters.RemoveAt(indexToRemove);
                appliedModifiers.RemoveAt(indexToRemove);
                DisplayPalace();
            }
        }

        protected virtual void DisplayPalace()
        {
            if (currentPalace != null)
            {
                Destroy(currentPalace);
            }

            var builder = new ItemSpiralBuilder().AddItems(allItems);
            
            for (int i = 0; i < appliedFilters.Count; i++)
            {
                builder.AddFilter(appliedFilters[i], appliedModifiers[i]);
            }

            currentPalace = builder
                .Build()
                .BuildPalace();
        }

    }

}