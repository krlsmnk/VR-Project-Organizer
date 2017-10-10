using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Project;


namespace CAVS.ProjectOrganizer.Scenes.Organizer
{

    /// <summary>
    /// Meant for keeping up with everything inside of the scene currently and
    /// providing a common interface for them all to talk with.
    /// 
    /// Design Pattern:
    /// https://sourcemaking.com/design_patterns/mediator
    /// </summary>
    public class GameMediator : MonoBehaviour
    {

        /// <summary>
        /// File path/name we used to load this scene.
        /// </summary>
        private string sceneLoaded = "test.vpo";
        
        /// <summary>
        /// All items currentely being interacted with inside of the scene.
        /// </summary>
        List<ItemBehaviour> items;

        // Use this for initialization
        void Start()
        {
            BuildSceneFromProject(Project.Space.LoadFromFile(sceneLoaded));
        }

        /// <summary>
        /// Called when the scene is unloaded (changing scenes, quiting applications, etc)
        /// </summary>
        void OnDestroy()
        {
            Save(sceneLoaded);
        }

        private void BuildSceneFromProject(Project.Space project)
        {
            items = new List<ItemBehaviour>();
            
            foreach(Item item in project.GetItems())
            {
                items.Add(item.Build(Vector3.up, Vector3.zero));
            }
        }

        void CreateAndAddItem(Item item)
        {

        }

        void Save(string name)
        {
            Debug.Log(string.Format("saving: {0}", name));
        }

    }

}