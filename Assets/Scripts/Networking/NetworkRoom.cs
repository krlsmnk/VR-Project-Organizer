using System;
using System.Collections.Generic;

using Firebase.Database;

namespace CAVS.ProjectOrganizer.Netowrking
{
    /// <summary>
    /// Abstract any Firebase implementation away from the existing code.
    /// Minimize damage done from having to switch away from Firebase
    /// if we ever have too.
    /// </summary>
    public class NetworkRoom
    {

        private string playerId;

        private DatabaseReference playerData;

        private DatabaseReference sceneData;

        private List<Action<DataSnapshot>> dataSubscribers; 

        public NetworkRoom(string playerId, DatabaseReference sceneData)
        {
            this.playerId = playerId;
            this.sceneData = sceneData;
            this.sceneData.ValueChanged += RoomValueChanged;
            playerData = sceneData.Child(string.Format("players/{0}/", playerId));
            dataSubscribers = new List<Action<DataSnapshot>>();
        }

        public void SubscribeToNewData(Action<DataSnapshot> subscriber)
        {
            if (subscriber == null)
            {
                throw new Exception("Subscriber can't be null!");
            }
            dataSubscribers.Add(subscriber);
        }

        public void SetValue(string key, string value)
        {
            playerData.Child(key).SetValueAsync(value);
            playerData.Child("timestamp").SetValueAsync(ServerValue.Timestamp);
        }

        public void SetObjectValue(string key, string value)
        {
            playerData.Child(key).SetRawJsonValueAsync(value);
            playerData.Child("timestamp").SetValueAsync(ServerValue.Timestamp);
        }

        public void Update(NetworkUpdate update)
        {
            var vals = update.GetValues();
            vals.Add("timestamp", ServerValue.Timestamp);
            playerData.UpdateChildrenAsync(vals);
        }

        private void RoomValueChanged(object sender, ValueChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                UnityEngine.Debug.LogError(args.DatabaseError.Message);
                return;
            }
            UnityEngine.Debug.Log(args.Snapshot.ToString());
        }

        public void CloseRoom()
        {
            playerData.RemoveValueAsync();
        }

    }
}
