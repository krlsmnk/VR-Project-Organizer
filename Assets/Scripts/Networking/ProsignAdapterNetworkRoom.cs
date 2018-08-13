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

        private Dictionary<string, object> TransformFromBinary(byte[] response, int start)
        {
            return new Dictionary<string, object>() {
                {
                    "position",
                    new Dictionary<string, object>() {
                        { "x", BitConverter.ToSingle(response, start) },
                        { "y", BitConverter.ToSingle(response, start + 4) },
                        { "z", BitConverter.ToSingle(response, start + 8) }
                    }
                },
                {
                    "rotation",
                    new Dictionary<string, object>() {
                        { "x", BitConverter.ToSingle(response, start + 12) },
                        { "y", BitConverter.ToSingle(response, start + 16) },
                        { "z", BitConverter.ToSingle(response, start + 20) }
                    }
                }
            };
        }

        public void SubscribeToUpdates(Action<Dictionary<string, object>> subscriber)
        {
            SubscribeToRoomUpdates(delegate (byte[] response)
            {

                bool positionUpdate = BitConverter.ToBoolean(response, 0);

                if (positionUpdate)
                {
                    var returnMessage = new Dictionary<string, object>();
                    if (response.Length >= 24)
                    {
                        returnMessage.Add("player-head", TransformFromBinary(response, 1));

                        if (response.Length >= 48)
                        {
                            returnMessage.Add("player-left", TransformFromBinary(response, 25));
                            if (response.Length >= 72)
                            {
                                returnMessage.Add("player-right", TransformFromBinary(response, 49));
                            }
                        }
                        subscriber(returnMessage);
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

        private void WriteVector(List<byte> buffer, Dictionary<string, object> vals)
        {
            buffer.AddRange(BitConverter.GetBytes((float)vals["x"]));
            buffer.AddRange(BitConverter.GetBytes((float)vals["y"]));
            buffer.AddRange(BitConverter.GetBytes((float)vals["z"]));
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
                buffer.AddRange(BitConverter.GetBytes(true));

                if (vals.ContainsKey("head-position")) WriteVector(buffer, (Dictionary<string, object>)vals["head-position"]);
                if (vals.ContainsKey("head-rotation")) WriteVector(buffer, (Dictionary<string, object>)vals["head-rotation"]);
                if (vals.ContainsKey("left-position")) WriteVector(buffer, (Dictionary<string, object>)vals["left-position"]);
                if (vals.ContainsKey("left-rotation")) WriteVector(buffer, (Dictionary<string, object>)vals["left-rotation"]);
                if (vals.ContainsKey("right-position")) WriteVector(buffer, (Dictionary<string, object>)vals["right-position"]);
                if (vals.ContainsKey("right-rotation")) WriteVector(buffer, (Dictionary<string, object>)vals["right-rotation"]);

            }



            UpdateRoom(buffer.ToArray());
        }


    }

}