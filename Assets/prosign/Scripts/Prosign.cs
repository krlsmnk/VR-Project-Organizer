using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using CAVS.ProjectOrganizer.Netowrking;

using UnityEngine;

namespace EliCDavis.Prosign
{
    public class Prosign: IDisposable, INetworkRoom
    {
        NetworkStream connection;

        Dictionary<string, Dictionary<string, List<Action<Dictionary<string, object>>>>> subscribers;
        Dictionary<string, Dictionary<string, List<Action<string>>>> oneTimeSubscribers;

        public Prosign(string ip, int port)
        {
            subscribers = new Dictionary<string, Dictionary<string, List<Action<Dictionary<string, object>>>>>();
            oneTimeSubscribers = new Dictionary<string, Dictionary<string, List<Action<string>>>>();
            connection = new TcpClient(ip, port).GetStream();
            connection.ReadTimeout = -1;
            connection.WriteTimeout = -1;
            BeginListening();
        }

        public void CreateRoom(string name, Action<string> onRoomCreate)
        {
            MessageServer("room", "create", name, onRoomCreate);
        }

        public void JoinRoom(string id, Action<string> onRoomJoin)
        {
            MessageServer("room", "join", id, onRoomJoin);
        }

        public void Update(string msg)
        {
            MessageServer("room", "update", msg.Trim(), null);
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

            var returnMessage = Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Count);

            MessageServer("room", "update", returnMessage, null);
        }

        private void MessageServer(string service, string method, string message, Action<string> serverResponseCallback){
            var msg = Encoding.ASCII.GetBytes(string.Format("{0}.{1}\n{2}\n", service, method, message));

            if ( serverResponseCallback != null ) {
                SubscribeOneShot(service, method, serverResponseCallback);
            }
            connection.BeginWrite(msg, 0, msg.Length, delegate (IAsyncResult ar)
            {
                try
                {
                    NetworkStream client = (NetworkStream)ar.AsyncState;
                    client.EndWrite(ar);
                }
                catch (Exception e)
                {
                    Debug.Log(string.Format("Error Writing: {0}", e.ToString()));
                }
            }, connection);
        }


        public void CreatePrivateRoom(Action<string> onRoomCreate)
        {
            CreateRoom("", onRoomCreate);
        }

        public void SubscribeToUpdates(Action<Dictionary<string, object>> callback) {
            Subscribe("room", "update", callback);
        }

        private void SubscribeOneShot(string service, string method, Action<string> cb)
        {
            if (oneTimeSubscribers.ContainsKey(service) == false)
            {
                oneTimeSubscribers.Add(service, new Dictionary<string, List<Action<string>>>());
            }

            if (oneTimeSubscribers[service].ContainsKey(method) == false)
            {
                oneTimeSubscribers[service].Add(method, new List<Action<string>>());
            }

            oneTimeSubscribers[service][method].Add(cb);
        }

        private void Subscribe(string service, string method, Action<Dictionary<string, object>> cb)
        {
            
            if (subscribers.ContainsKey(service) == false)
            {
                subscribers.Add(service, new Dictionary<string, List<Action<Dictionary<string, object>>>>());
            }

            if (subscribers[service].ContainsKey(method) == false)
            {
                subscribers[service].Add(method, new List<Action<Dictionary<string, object>>>());
            }

            subscribers[service][method].Add(cb);
        }

        /// <summary>
        /// Continually listen for messages from the server
        /// </summary>
        private void BeginListening()
        {
            byte[] buffer = new byte[1024];
            Debug.Log("Begining Read");

            connection.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
            {
                Debug.Log("read: " + Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                NetworkStream client = (NetworkStream)ar.AsyncState;
                int bytesRead = client.EndRead(ar);
                if (bytesRead > 0)
                {
                    ParseAndHandleIncomingMessage(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                } else
                {
                    Debug.Log("No bytes read");
                }
                BeginListening();
            }, connection);
        }

        private void ParseAndHandleIncomingMessage(string rawMessage)
        {
            var split = rawMessage.IndexOf(":");
            if (split == -1)
            {
                throw new Exception("Invalid message from server. Contact server admin.");
            }

            var header = rawMessage.Substring(0, split).Trim();
            var body = rawMessage.Substring(split + 1).Trim();

            if (header.IndexOf(".") == -1)
            {
                throw new Exception("Invalid message from server. Contact server admin.");
            }

            var headerContents = header.Split('.');

            DistributeMessage(headerContents[0], headerContents[1], body);
        }

        private void DistributeMessage(string service, string method, string body)
        {
            Debug.Log(string.Format("{0}.{1}: {2}", service,method, body));
            if(oneTimeSubscribers.ContainsKey(service) && oneTimeSubscribers[service].ContainsKey(method))
            {
                var callbacks = oneTimeSubscribers[service][method];
                foreach (var sub in callbacks)
                {
                    sub(body);
                }
                oneTimeSubscribers[service][method].Clear();
            }

            if(subscribers.ContainsKey(service) && subscribers[service].ContainsKey(method))
            {
                var callbacks = subscribers[service][method];
                foreach (var sub in callbacks)
                {
                    sub(new Dictionary<string, object>() { { "something", body } });
                }
            }
        }

        public void Dispose() {
            CloseRoom();
        }

        public void CloseRoom()
        {
            connection.Close();
        }

    }

}
