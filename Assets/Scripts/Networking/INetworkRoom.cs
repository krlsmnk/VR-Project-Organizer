using System;
using System.Collections.Generic;

namespace CAVS.ProjectOrganizer.Netowrking
{
    /// <summary>
    /// Abstract any Firebase implementation away from the existing code.
    /// Minimize damage done from having to switch away from Firebase
    /// if we ever have too.
    /// </summary>
    public interface INetworkRoom
    {

        void SubscribeToUpdates(Action<Dictionary<string, object>> subscriber);


        void Update(NetworkUpdate update);


        /// <summary>
        /// Calling this cleans up data on the server and let's others know
        /// that you have officially left the room.
        /// </summary>
        void CloseRoom();

    }
}
