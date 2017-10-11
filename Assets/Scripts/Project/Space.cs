using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project
{

    public class Space
    {

        List<Item> itemsInProject;

		HashSet<string> tags;

        Dictionary<Item, Vector3> itemPostitionsInSpace;

		Dictionary<Item, HashSet<string>> itemTags;

        /// <summary>
        /// Private so only it can instantiate itself..
        /// </summary>
        private Space() 
		{ 
			this.itemsInProject = new List<Item> ();
			this.itemPostitionsInSpace = new Dictionary<Item, Vector3> ();
			this.tags = new HashSet<string>();
			this.itemTags = new Dictionary<Item, HashSet<string>>();
		}

        /// <summary>
        /// Load a project from a XML file stored on the computer
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Space LoadFromFile(string path)
        {
            Space p = new Space();
            
            // Add example item
			p.itemsInProject.Add(new TextItem(
				"Niave Bayes Classifier",
				"Assumes the independence of features to handle real and descrete data"
			));

			p.itemsInProject.Add(new TextItem(
				"Decision Trees", 
				"Supervised learning technique for predicting the likelyhood of a outcome given x features"
			));

			p.itemsInProject.Add(new TextItem(
				"Regression", 
				"Supervised learning for making best guess predictions in continuous sets"
			));
            
			p.itemsInProject.Add (new UrlItem (
				"Regression Example",
				"https://cdn-images-1.medium.com/max/600/1*iuqVEjdtEMY8oIu3cGwC1g.png"
			));

            // Set it's position
			p.itemPostitionsInSpace.Add(p.itemsInProject[0], new Vector3(0, 5, 0));
			p.itemPostitionsInSpace.Add(p.itemsInProject[1], new Vector3(-2, 3, 0));
			p.itemPostitionsInSpace.Add(p.itemsInProject[2], new Vector3(2, 3, 0));
			p.itemPostitionsInSpace.Add(p.itemsInProject[3], new Vector3(0, 1, 0));

			// Add some tags that a item can have in the project
			p.tags.Add("Learning");
			p.tags.Add("Continuous");
			p.tags.Add("Discrete");

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