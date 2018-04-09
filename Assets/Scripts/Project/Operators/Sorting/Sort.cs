using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project.Operators.Sorting
{

    public abstract class Sort 
    {

        public abstract List<Item> SortItems(Item[] items);

    }

}