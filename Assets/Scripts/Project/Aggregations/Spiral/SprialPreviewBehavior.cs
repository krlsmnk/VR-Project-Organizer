using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.ProjectOrganizer.Project.Filtering;

namespace CAVS.ProjectOrganizer.Project.Aggregations.Spiral
{

    public class SprialPreviewBehavior : MonoBehaviour
    {

        private Filter filter;

        Action<bool, GameObject> plotModifier;

        public void SetFilter(Filter filter)
        {
            this.filter = filter;
            this.plotModifier = null;
        }

        public void SetFilter(Filter filter, Action<bool, GameObject> plotModifier)
        {
			this.filter = filter;
            this.plotModifier = plotModifier;
        }

        public Filter GetFilter()
        {
            return filter;
        }

        public Action<bool, GameObject> GetPlotModifier()
        {
            return this.plotModifier;
        }

    }

}