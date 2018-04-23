using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.Aggregations.Spiral;

namespace CAVS.ProjectOrganizer.Scenes.MergeTesting
{

    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        private MerBehavior merger;

        private GameObject oldPalace;

        Item[] setA;

        Item[] setB;

        // Use this for initialization
        void Start()
        {
            oldPalace = null;

            setA = new Item[]
            {
                new TextItem("A", "", new Dictionary<string, string> {
                    { "name", "a" },
                    { "height", "10" },
                    { "weight", "20" }
                }),
                new TextItem("B", "", new Dictionary<string, string> {
                    { "name", "b" },
                    { "height", "30" },
                    { "weight", "40" }
                })
            };

            setB = new Item[]
            {
                new TextItem("A", "", new Dictionary<string, string> {
                    { "name", "a" },
                    { "year", "2010" },
                    { "tireRadius", "20" }
                }),
                new TextItem("B", "", new Dictionary<string, string> {
                    { "name", "b" },
                    { "year", "2012" },
                    { "tireRadius", "40" }
                }),
                new TextItem("C", "", new Dictionary<string, string> {
                    { "name", "c" },
                    { "year", "2014" },
                    { "tireRadius", "50" }
                })
            };

            merger.SubscibeToInside(OnMergeInteraction);
        }

        void OnMergeInteraction(List<GameObject> objs) {
            bool hasA = false;
            bool hasB = false;
            foreach (var obj in objs)
            {
                if(obj.transform.name.ToLower() == "a")
                {
                    hasA = true;
                } else if (obj.transform.name.ToLower() == "b")
                {
                    hasB = true;
                }
            }
            ItemSpiral spiral = null;
            if (hasA && !hasB)
            {
                spiral = new ItemSpiral(setA);
            } else if (!hasA && hasB)
            {
                spiral = new ItemSpiral(setB);
            }
            else if (hasA && hasB)
            {
                spiral = new ItemSpiral(ProjectFactory.InnerJoin(setA, setB, "name"));
            }

            if(oldPalace != null)
            {
                Destroy(oldPalace);
            }

            if (spiral != null)
            {
                oldPalace = spiral.BuildPalace();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}