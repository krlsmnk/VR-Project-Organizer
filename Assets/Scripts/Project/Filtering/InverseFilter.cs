using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class InverseFilter : Filter
    {

        Filter filterToInverse;

        public InverseFilter(Filter filterToInverse)
        {
            this.filterToInverse = filterToInverse;
        }

        public override bool FilterItem(Item item)
        {
            return !this.filterToInverse.FilterItem(item);
        }
    }

}