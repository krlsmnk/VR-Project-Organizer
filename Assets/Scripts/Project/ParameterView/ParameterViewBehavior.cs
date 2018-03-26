using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CAVS.ProjectOrganizer.Project.ParameterView
{

    public class ParameterViewBehavior : MonoBehaviour
    {

        [SerializeField]
        private RectTransform paramterView;

        [SerializeField]
        private RectTransform parameterButton;

        private RectTransform[] buttonInstances;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public RectTransform[] GetButtons()
        {
            return this.buttonInstances;
        }

        /// <summary>
        /// Set the parameters that get displayed for this view
        /// </summary>
        public void SetParameters(Dictionary<string, string> parameters)
        {
            if (buttonInstances != null)
            {
                foreach (RectTransform btn in buttonInstances)
                {
                    Destroy(btn.gameObject);
                }
            }
            buttonInstances = new RectTransform[parameters.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> param in parameters)
            {
                RectTransform btn = Instantiate(parameterButton, paramterView);
                btn.transform.name = param.Key;
                btn.GetComponentInChildren<Text>().text = string.Format("{0}: {1}", param.Key, param.Value);
                buttonInstances[i] = btn;
                i++;
            }
        }

    }

}