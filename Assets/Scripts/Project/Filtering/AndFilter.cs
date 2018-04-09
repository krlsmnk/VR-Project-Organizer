using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project.Filtering
{

    public class AndFilter : Filter
    {

        Filter a;

        Filter b;

        public AndFilter(Filter a, Filter b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool FilterItem(Item item)
        {
            return this.a.FilterItem(item) && this.b.FilterItem(item);
        }

    }

}