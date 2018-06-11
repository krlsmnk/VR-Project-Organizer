using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Firebase;
using Firebase.Database;

namespace CAVS.ProjectOrganizer.Netowrking
{

    public class NetworkingManager
    {
        
        private static NetworkingManager instance;

        public static NetworkingManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new NetworkingManager();
                }
                return instance;
            }
        }

        private DatabaseReference databaseReference;

        private NetworkingManager()
        {
            FirebaseApp.Create();
            FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://organizer-vr.firebaseio.com");

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // Set a flag here indiciating that Firebase is ready to use by your
                    // application.
                }
                else
                {
                    Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });

            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            // databaseReference.Child("item").SetRawJsonValueAsync("{ \"test\" : 1 }");
        }

        public DatabaseReference CreateSceneEntry(string sceneName)
        {
            string key = databaseReference.Child("scenes").Push().Key;
            databaseReference.Child("/scenes/" + key).SetValueAsync(sceneName);
            return databaseReference.Child("/sceneData/" + key + "/");
        }

    }

}