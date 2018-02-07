using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project;

namespace CAVS.ProjectOrganizer.Project.ParameterView
{

    public static class ControllerFactory
    {

        private static GameObject parameterViewRefernce = null;

        private static GameObject GetReference()
        {
            if (parameterViewRefernce == null)
            {
                parameterViewRefernce = Resources.Load<GameObject>("Parameter View");
            }
            return parameterViewRefernce;
        }

        public static ParameterViewBehavior CreateParameterView(Dictionary<string, string> parameters)
        {
            ParameterViewBehavior pv = GameObject
                .Instantiate(
                    GetReference(),
                    Vector3.zero,
                    Quaternion.identity)
                .GetComponent<ParameterViewBehavior>();

            pv.SetParameters(parameters);
            return pv;
        }

        public static ParameterViewBehavior CreateParameterValueComparison(string fieldNames, Item[] items)
        {
            ParameterViewBehavior pv = GameObject
                .Instantiate(
                    GetReference(),
                    Vector3.zero,
                    Quaternion.identity)
                .GetComponent<ParameterViewBehavior>();

            //pv.SetParameters(parameters);
            return pv;
        }

    }

}