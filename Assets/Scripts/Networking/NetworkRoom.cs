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
        enum ConnectionStatus {
            Connected,
            Ended
        }

        private ConnectionStatus connectionStatus;

        private string playerId;

        private DatabaseReference playerData;

        private DatabaseReference sceneData;

        private DatabaseReference scenePlayers;

        private List<Action<List<object>>> dataSubscribers;


        public NetworkRoom(string playerId, DatabaseReference sceneData, DatabaseReference scenePlayers)
        {
            this.playerId = playerId;
            this.sceneData = sceneData;
            this.sceneData.ValueChanged += RoomValueChanged;
            this.scenePlayers = scenePlayers;
            playerData = sceneData.Child(string.Format("players/{0}/", playerId));
            dataSubscribers = new List<Action<List<object>>>();
            connectionStatus = ConnectionStatus.Connected;
        }

        public bool Connected()
        {
            return connectionStatus == ConnectionStatus.Connected;
        }

        public void SubscribeToNewData(Action<List<object>> subscriber)
        {
            if (subscriber == null)
            {
                throw new Exception("Subscriber can't be null!");
            } else if (connectionStatus == ConnectionStatus.Ended)
            {
                throw new Exception("The connection has ended");
            }
            dataSubscribers.Add(subscriber);
        }

        public void SetValue(string key, string value)
        {
            if (connectionStatus == ConnectionStatus.Ended)
            {
                throw new Exception("The connection has ended");
            }
            playerData.Child(key).SetValueAsync(value);
            playerData.Child("timestamp").SetValueAsync(ServerValue.Timestamp);
        }

        public void SetObjectValue(string key, string value)
        {
            if (connectionStatus == ConnectionStatus.Ended)
            {
                throw new Exception("The connection has ended");
            }
            playerData.Child(key).SetRawJsonValueAsync(value);
            playerData.Child("timestamp").SetValueAsync(ServerValue.Timestamp);
        }

        public void Update(NetworkUpdate update)
        {
            if (connectionStatus == ConnectionStatus.Ended)
            {
                throw new Exception("The connection has ended");
            }
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
            List<object> filteredData = new List<object>();

            foreach(var child in args.Snapshot.Child("players").Children)
            {
                if(child.Key != playerId)
                {
                    filteredData.Add(child.Value);
                }
            }

            foreach (var sub in dataSubscribers)
            {
                sub(filteredData);
            }
        }

        /// <summary>
        /// Calling this cleans up data on the server and let's others know
        /// that you have officially left the room.
        /// </summary>
        public void CloseRoom()
        {
            playerData.RemoveValueAsync();
            scenePlayers.Child(playerId).RemoveValueAsync();
            connectionStatus = ConnectionStatus.Ended;
        }

    }
}
