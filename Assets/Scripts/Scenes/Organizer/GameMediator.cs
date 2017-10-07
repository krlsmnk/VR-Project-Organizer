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
        /// All items currentely being interacted with inside of the scene.
        /// </summary>
        List<ItemBehaviour> items;

        // Use this for initialization
        void Start()
        {
            items = new List<ItemBehaviour>();
			items.Add (new TextItem("Example", "This is an example node").BuildItem());
        }

        void CreateAndAddItem(Item item)
        {

        }

        void Save(string name)
        {

        }

    }

}