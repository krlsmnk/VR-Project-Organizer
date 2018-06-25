using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CAVS.ProjectOrganizer.Netowrking
{

    public class ShowcaseData
    {

        public struct User {

            private Vector3 position;

            private Vector3 rotation;

            public User(Vector3 position, Vector3 rotation)
            {
                this.position = position;
                this.rotation = rotation;
            }

        }

        private List<User> otherUsersInScene;

        public ShowcaseData(List<object> dataFromServer)
        {
            otherUsersInScene = new List<User>();

            foreach (var item in dataFromServer)
            {
                if(item.GetType() == typeof(Dictionary<string, object>))
                {
                    var dict = item as Dictionary<string, object>;

                    var posDict = dict["position"] as Dictionary<string, object>;
                    Vector3 position = new Vector3(
                        (float)posDict["x"],
                        (float)posDict["y"],
                        (float)posDict["z"] 
                        );

                    var rotDict = dict["rotation"] as Dictionary<string, object>;
                    Vector3 rotation = new Vector3(
                        (float)rotDict["x"],
                        (float)rotDict["y"],
                        (float)rotDict["z"]
                        );

                    otherUsersInScene.Add(new User(position, rotation));

                } else
                {
                    throw new System.Exception("Data from server not in correct format!");
                }
                Debug.Log(item);
            }
        }

        public List<User> UsersInScene()
        {
            return otherUsersInScene;
        }

    }

}