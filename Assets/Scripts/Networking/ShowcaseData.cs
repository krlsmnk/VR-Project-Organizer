using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CAVS.ProjectOrganizer.Netowrking
{

    public class ShowcaseData
    {


        private List<NetworkedObject> otherUsersInScene;

        public ShowcaseData(Dictionary<string, object> dataFromServer)
        {
            otherUsersInScene = new List<NetworkedObject>();
            Debug.LogFormat("Data length: {0}", dataFromServer.Count);

            foreach (var keyValPair in dataFromServer)
            {
                if(keyValPair.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    var playerDict = keyValPair.Value as Dictionary<string, object>;

                    Dictionary<string, object> posDict = playerDict["position"] as Dictionary<string, object>;
                    Vector3 position = new Vector3(
                        float.Parse(posDict["x"].ToString()),
                        float.Parse(posDict["y"].ToString()),
                        float.Parse(posDict["z"].ToString())
                    );

                    var rotDict = playerDict["rotation"] as Dictionary<string, object>;
                    Vector3 rotation = new Vector3(
                        float.Parse(rotDict["x"].ToString()),
                        float.Parse(rotDict["y"].ToString()),
                        float.Parse(rotDict["z"].ToString())
                    );

                    otherUsersInScene.Add(new NetworkedObject(keyValPair.Key, position, rotation));
                    Debug.Log(otherUsersInScene[otherUsersInScene.Count - 1]);
                } else
                {
                    throw new System.Exception("Data from server not in correct format!");
                }
            }
        }

        public List<NetworkedObject> UsersInScene()
        {
            return otherUsersInScene;
        }

    }

}