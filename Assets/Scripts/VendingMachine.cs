using System;
using System.Globalization;

using UnityEngine;
using UnityEngine.UI;

using CAVS.ProjectOrganizer.Project.Filtering;
using CAVS.ProjectOrganizer.Project;

namespace CAVS.ProjectOrganizer
{

    public class VendingMachine : MonoBehaviour
    {

        [SerializeField]
        private AudioClip badBeep;

        [SerializeField]
        private AudioClip goodBeep;

        /// <summary>
        /// make a GameObject reference to assign later
        /// this will be used to get the text values from the vending machine
        /// </summary>
        private GameObject currentGameObject;

        private Item[] nodes;

        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            nodes = ProjectFactory.BuildItemsFromCSV("CarData.csv");
        }
        

        public void MakeFilterButtonPressed()
        {

            //get the filter name from the scene
            currentGameObject = GameObject.Find("Filter Name");
            string filterName = currentGameObject.transform.Find("Text").GetComponent<Text>().text;

            //get the filter type from the scene
            currentGameObject = GameObject.Find("Filter Type");
            string filterType = currentGameObject.transform.Find("Label").GetComponent<Text>().text;

            //get the filter value from the scene
            currentGameObject = GameObject.Find("Filter Value");
            string filterValue = currentGameObject.transform.Find("Text").GetComponent<Text>().text;

            //get the field from the scene
            currentGameObject = GameObject.Find("Field");
            string field = currentGameObject.transform.Find("Text").GetComponent<Text>().text;

            //get the operator from the scene
            currentGameObject = GameObject.Find("Operator");
            string op = currentGameObject.transform.Find("Label").GetComponent<Text>().text;

            //get the min and max from the scene
            currentGameObject = GameObject.Find("Min");
            string min = currentGameObject.transform.Find("Text").GetComponent<Text>().text;
            currentGameObject = GameObject.Find("Max");
            string max = currentGameObject.transform.Find("Text").GetComponent<Text>().text;

            //get pass/fail behavior from scene
            currentGameObject = GameObject.Find("Filter Behavior");
            string behavior = currentGameObject.transform.Find("Label").GetComponent<Text>().text;
            currentGameObject = GameObject.Find("Behavior Value");
            string behaviorValue = currentGameObject.transform.Find("Text").GetComponent<Text>().text;


            MakeFilterOperands(filterName, filterType, filterValue, field, op, min, max, behavior, behaviorValue);

        }

        public void MakeFilterOperands(string filterName, string filterType, string filterValue, string field, string op, string min, string max, string behavior, string behaviorValue)
        {
            Filter constructedFilter = null;

            switch (filterType)
            {
                case "Number Filter":
                    float result = 0f;
                    if (float.TryParse(filterValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                    {
                        constructedFilter = new NumberFilter(field, ParseEnum<NumberFilter.Operator>(op), result);
                    }
                    break;

                case "String Filter":
                    constructedFilter = new StringFilter(field, ParseEnum<StringFilter.Operator>(op), filterValue);
                    break;

                case "Range Filter (Number)":
                    float minParsed;
                    float maxParsed;
                    if (float.TryParse(min, NumberStyles.Any, CultureInfo.InvariantCulture, out minParsed) &&
                        float.TryParse(max, NumberStyles.Any, CultureInfo.InvariantCulture, out maxParsed))
                    {
                        constructedFilter = new RangeFilterNum(field, ParseEnum<RangeFilterNum.Operator>(op), minParsed, maxParsed);
                    }
                    break;

                case "Range Filter (String)":
                    constructedFilter = new RangeFilterString(field, ParseEnum<RangeFilterString.Operator>(op), min, max);
                    break;

                default:
                    constructedFilter = null;
                    break;
            }

            if (constructedFilter != null)
            {
                GameObject platform = GameObject.Find("Filter Platform");
                GameObject currentFilter = constructedFilter.Build(BehaviorNameToPlotModifierAction(behavior, behaviorValue));
                currentFilter.transform.position = platform.transform.position + Vector3.up;

                audioSource.PlayOneShot(goodBeep, 7);
            } else
            {
                audioSource.PlayOneShot(badBeep, 7);
            }

        }

        private Action<bool, GameObject> BehaviorNameToPlotModifierAction(string behaviorName, string unsantizedValue)
        {
            float santizedValue = .6f;
            try {
                santizedValue = float.Parse(unsantizedValue);
            } catch(Exception e){
                Debug.Log(string.Format("Error parsing behavior value: {0}", e.Message));
            } 
            switch(behaviorName)
            {
                case "Change Size":
                    return delegate(bool passed, GameObject point)
                    {
                        if (point != null && !passed)
                        {
                            point.transform.localScale *= santizedValue;
                        }
                    };

                case "Change Color":
                    return delegate(bool passed, GameObject point)
                    {
                        if (point != null && !passed)
                        {
                            var curColor = point.transform.GetComponent<MeshRenderer>().material.color;
                            curColor.r = Mathf.Clamp01(curColor.r + santizedValue);
                        }
                    };

                case "Become More Transparent":
                    return delegate(bool passed, GameObject point)
                    {
                        if (point != null && !passed)
                        {
                            var curColor = point.transform.GetComponent<MeshRenderer>().material.color;
                            curColor.a = Mathf.Clamp01(curColor.a - santizedValue);
                        }
                    };

                default:
                    return null;
            }
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

}