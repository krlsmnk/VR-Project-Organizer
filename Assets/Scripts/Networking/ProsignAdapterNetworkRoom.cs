using System;
using System.Collections.Generic;
using EliCDavis.Prosign;

namespace CAVS.ProjectOrganizer.Netowrking
{

    /// <summary>
    /// An interface to the prosign server
    /// </summary>
    public class ProsignAdapterNetworkRoom : Server, INetworkRoom
    {

        public ProsignAdapterNetworkRoom(string server, int port) : base(server, port)
        { }

        public void CloseRoom()
        {
            Close();
        }

        public void SubscribeToUpdates(Action<Dictionary<string, object>> subscriber)
        {
            SubscribeToRoomUpdates(delegate (byte[] response)
            {
                if (response.Length >= 24)
                {
                    var pos = new Dictionary<string, object>() {
                        { "x", BitConverter.ToSingle(response, 0) },
                        { "y", BitConverter.ToSingle(response, 4) },
                        { "z", BitConverter.ToSingle(response, 8) }
                    };

                    var rot = new Dictionary<string, object>() {
                        { "x", BitConverter.ToSingle(response, 12) },
                        { "y", BitConverter.ToSingle(response, 16) },
                        { "z", BitConverter.ToSingle(response, 20) }
                    };

                    subscriber(new Dictionary<string, object>() {
                        { "player", new Dictionary<string, object>() {{ "position", pos }, { "rotation", rot } } }
                    });
                }

                throw new Exception(string.Format("Bad Update: {0} bytes", response.Length));
            });
        }

        public void Update(NetworkUpdate update)
        {
            var vals = update.GetValues();
            var positionVals = (Dictionary<string, object>)vals["position"];
            var rotationVals = (Dictionary<string, object>)vals["rotation"];

            var buffer = new List<byte>();
            buffer.AddRange(BitConverter.GetBytes((float)positionVals["x"]));
            buffer.AddRange(BitConverter.GetBytes((float)positionVals["y"]));
            buffer.AddRange(BitConverter.GetBytes((float)positionVals["z"]));
            buffer.AddRange(BitConverter.GetBytes((float)rotationVals["x"]));
            buffer.AddRange(BitConverter.GetBytes((float)rotationVals["y"]));
            buffer.AddRange(BitConverter.GetBytes((float)rotationVals["z"]));

            UpdateRoom(buffer.ToArray());
        }


    }

}