using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Firebase;
using Firebase.Database;

namespace CAVS.ProjectOrganizer.Netowrking
{

    /// <summary>
    /// Abstract any Firebase implementation away from the existing code.
    /// Minimize damage done from having to switch away from Firebase
    /// if we ever have too.
    /// </summary>
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

        private string displayName;

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

            displayName = PlayerPrefs.GetString("displayName", "anonymous");
            // databaseReference.Child("item").SetRawJsonValueAsync("{ \"test\" : 1 }");
        }

        public NetworkRoom CreateSceneEntry(string sceneName)
        {
            string sceneKey = databaseReference.Child("scenes").Push().Key;
            databaseReference.Child("/scenes/" + sceneKey).SetValueAsync(sceneName);

            string playerKeyPath = string.Format("/scenePlayers/{0}/", sceneKey);
            string playerKey = databaseReference.Child(playerKeyPath).Push().Key;
            databaseReference.Child(playerKeyPath + playerKey).SetValueAsync(displayName);

            return new NetworkRoom(
                playerKey, 
                databaseReference.Child(string.Format("/sceneData/{0}/", sceneKey)), 
                databaseReference.Child(string.Format("/scenePlayers/{0}/", sceneKey))
            );
        }

    }

}