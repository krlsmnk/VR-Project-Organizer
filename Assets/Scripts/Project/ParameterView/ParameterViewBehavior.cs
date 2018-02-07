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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Set the parameters that get displayed for this view
        /// </summary>
        public void SetParameters(Dictionary<string, string> parameters)
        {
            foreach (KeyValuePair<string, string> param in parameters)
            {
				RectTransform btn = Instantiate(parameterButton, paramterView);
				btn.GetComponentInChildren<Text>().text = string.Format("{0}: {1}", param.Key, param.Value);
            }
        }

    }

}