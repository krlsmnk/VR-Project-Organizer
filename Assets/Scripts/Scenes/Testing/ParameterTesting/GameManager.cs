using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTK;

using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.ParameterView;
using CAVS.ProjectOrganizer.Project.Filtering;
using CAVS.ProjectOrganizer.Project.Aggregations.Spiral;

namespace CAVS.ProjectOrganizer.Scenes.Testing.ParameterTesting
{



    public class GameManager : MonoBehaviour
    {

        VRTK_ControllerEvents rightController;

        Item[] allItems;

        // Use this for initialization
        void Start()
        {
            allItems = ProjectFactory.BuildItemsFromCSV("CarData.csv");
            RectTransform[] buttons = ControllerFactory
                .CreateParameterView(allItems[0].GetValues())
                .GetButtons();

            foreach (RectTransform button in buttons)
            {
                ButtonBehavior btn = button.gameObject.AddComponent<ButtonBehavior>();
                btn.OnSelected(this.Selected);
                btn.OnUnselected(this.UnSelected);

                BoxCollider col = button.gameObject.AddComponent<BoxCollider>();
                col.size = new Vector3(183, 25, 1);
                col.isTrigger = true;

                button.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            }


        }

        void Update()
        {
            if (rightController == null)
            {
                rightController = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();
                initialize();
            }
        }

        void initialize()
        {
            if (rightController == null)
            {
                return;
            }
            rightController.TriggerClicked += delegate (object o, ControllerInteractionEventArgs e)
                 {
                     if (currentelySelected != null)
                     {
                         CreateFilterFromKey(currentelySelected.transform.name);
                     }  
                 };
        }

        private void CreateFilterFromKey(string key)
        {
            Filter filterCreated;
            string value = allItems[0].GetValue(key);

            float result;
            if (float.TryParse(value, out result))
            {
                filterCreated = new NumberFilter(key, NumberFilter.Operator.EqualTo, result);
            }
            else
            {
                filterCreated = new StringFilter(key, StringFilter.Operator.Equal, value);
            }
            new ItemSpiralBuilder()
                .AddItems(allItems)
                .AddFilter(filterCreated)
                .Build()
                .BuildPreview(Vector3.one * 2);
        }

        ButtonBehavior currentelySelected;

        void Selected(ButtonBehavior btn)
        {
            currentelySelected = btn;
        }

        void UnSelected(ButtonBehavior btn)
        {
            if (btn == currentelySelected)
            {
                currentelySelected = null;
            }
        }


    }

}