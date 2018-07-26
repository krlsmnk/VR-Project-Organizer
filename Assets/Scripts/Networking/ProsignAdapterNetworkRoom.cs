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

        public ProsignAdapterNetworkRoom(string server, int port)
            : base(server, port)
        { }

        public void CloseRoom()
        {
            Close();
        }

        public void SubscribeToUpdates(Action<Dictionary<string, object>> subscriber)
        {
            SubscribeToRoomUpdates(delegate(byte[] response)
            {

                bool positionUpdate = BitConverter.ToBoolean(response, 0);

                if (positionUpdate)
                {
                    if (response.Length >= 24)
                    {
                        var pos = new Dictionary<string, object>() {
                            { "x", BitConverter.ToSingle(response, 1) },
                            { "y", BitConverter.ToSingle(response, 5) },
                            { "z", BitConverter.ToSingle(response, 9) }
                        };

                        var rot = new Dictionary<string, object>() {
                            { "x", BitConverter.ToSingle(response, 13) },
                            { "y", BitConverter.ToSingle(response, 17) },
                            { "z", BitConverter.ToSingle(response, 21) }
                        };

                        subscriber(new Dictionary<string, object>() {
                            { "player", new Dictionary<string, object>() {{ "position", pos }, { "rotation", rot } } }
                        });
                    }
                    else
                    {
                        throw new Exception(string.Format("Bad Update: {0} bytes", response.Length));
                    }
                }
                else
                {
                    int carIndex = BitConverter.ToInt32(response, 1);
                    subscriber(new Dictionary<string, object>() { 
                        {"carUpdate", carIndex}
                    });
                }


            });
        }

        public void Update(NetworkUpdate update)
        {

            var vals = update.GetValues();
            var buffer = new List<byte>();

            if (vals.ContainsKey("selectedCar"))
            {
                buffer.AddRange(BitConverter.GetBytes(false));
                buffer.AddRange(BitConverter.GetBytes((int)vals["selectedCar"]));

            }
            else
            {
                var positionVals = (Dictionary<string, object>)vals["position"];
                var rotationVals = (Dictionary<string, object>)vals["rotation"];

                buffer.AddRange(BitConverter.GetBytes(true));

                buffer.AddRange(BitConverter.GetBytes((float)positionVals["x"]));
                buffer.AddRange(BitConverter.GetBytes((float)positionVals["y"]));
                buffer.AddRange(BitConverter.GetBytes((float)positionVals["z"]));
                buffer.AddRange(BitConverter.GetBytes((float)rotationVals["x"]));
                buffer.AddRange(BitConverter.GetBytes((float)rotationVals["y"]));
                buffer.AddRange(BitConverter.GetBytes((float)rotationVals["z"]));
            }



            UpdateRoom(buffer.ToArray());
        }


    }

}