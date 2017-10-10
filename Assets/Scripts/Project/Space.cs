using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project
{

    public class Space
    {

        List<Item> itemsInProject;

        Dictionary<Item, Vector3> itemPostitionsInSpace;

        /// <summary>
        /// Private so only it can instantiate itself..
        /// </summary>
        private Space() { }

        /// <summary>
        /// Load a project from a XML file stored on the computer
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Space LoadFromFile(string path)
        {
            Space p = new Space();
            
            // Add example item
            p.itemsInProject = new List<Item>();
            p.itemsInProject.Add(new TextItem("Example :P", "This is an example node"));
            
            // Set it's position
            p.itemPostitionsInSpace = new Dictionary<Item, Vector3>();
            p.itemPostitionsInSpace.Add(p.itemsInProject[0], new Vector3(0, 5, 0));

            return p;
        }

        public List<Item> GetItems()
        {
            return this.itemsInProject;
        }

        public Dictionary<Item, Vector3> GetItemPositions()
        {
            return this.itemPostitionsInSpace;
        }

    }

}