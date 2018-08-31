using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using UnityEngine;

using EliCDavis.Prosign.Subscription;

namespace EliCDavis.Prosign
{
    public class Server : IDisposable
    {
        NetworkStream connection;

        SubscriptionManager subscriptionManager;

        public Server(string ip, int port)
        {
            subscriptionManager = new SubscriptionManager();
            connection = new TcpClient(ip, port).GetStream();
            BeginListening();
        }

        public void CreateRoom(string name, Action<string> onRoomCreate)
        {
            subscriptionManager.SubscribeOneShot("room", "create", SubscriberFactory.MakeSubscriber(onRoomCreate));
            MessageServer("room", "create", name);
        }

        public void JoinRoom(string id, Action onRoomJoin)
        {
            subscriptionManager.SubscribeOneShot("room", "join", SubscriberFactory.MakeSubscriber(onRoomJoin));
            MessageServer("room", "join", id);
        }

        public void GetRooms(Action<List<Room>> onGetRooms)
        {
            subscriptionManager.SubscribeOneShot("room", "list", SubscriberFactory.MakeSubscriber(delegate(string message)
            {
                var rooms = new List<Room>();
                var roomStrings = message.Split(';');
                foreach(var rS in roomStrings)
                {
                    if (!rS.Equals(""))
                    {
                        var roomContents = rS.Split(':');
                        rooms.Add(new Room(roomContents[1], roomContents[0]));
                    }
                }
                onGetRooms(rooms);
            }));
            MessageServer("room", "list");
        }

        public void UpdateRoom(byte[] msg)
        {
            MessageServer("room", "update", msg);
        }

        private void MessageServer(string service, string method)
        {
            MessageServer(service, method, new byte[0]);
        }

        private void MessageServer(string service, string method, string message)
        {
            MessageServer(service, method, Encoding.UTF8.GetBytes(message));
        }

        private void MessageServer(string service, string method, byte[] message)
        {

            var msg = new List<byte>
            {
                Convert.ToByte(message.Length)
            };
            msg.AddRange(Encoding.UTF8.GetBytes(string.Format("{0}.{1}:", service, method)));
            msg.AddRange(message);

            connection.BeginWrite(msg.ToArray(), 0, msg.Count, delegate (IAsyncResult ar)
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

        public void SubscribeToRoomUpdates(Action<byte[]> callback)
        {
            subscriptionManager.Subscribe("room", "update", SubscriberFactory.MakeSubscriber(callback));
        }

        /// <summary>
        /// Continually listen for messages from the server
        /// </summary>
        private void BeginListening()
        {
            byte[] buffer = new byte[1024];
            connection.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
            {
                NetworkStream client = (NetworkStream)ar.AsyncState;
                int bytesRead = client.EndRead(ar);
                BeginListening();
                if (bytesRead > 0)
                {
                    ParseAndHandleIncomingMessage(buffer, bytesRead);
                } else
                {
                    Debug.Log("Error reading");
                }
            }, connection);
        }

        private void ParseAndHandleIncomingMessage(byte[] rawMessage, int bufferSize)
        {
            // System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length)
            byte status = rawMessage[0];
            uint bodyLength = Convert.ToUInt32(rawMessage[1]);
            int headerLength = bufferSize - (int)bodyLength - 3;
            string header = Encoding.UTF8.GetString(rawMessage, 2, headerLength);

            byte[] body = new byte[(int)bodyLength];
            Array.Copy(rawMessage, headerLength + 3, body, 0, (int)bodyLength);

            if (header.IndexOf(".") == -1)
            {
                throw new Exception("Invalid message from server. Contact server admin.");
            }

            var headerContents = header.Split('.'); 

            if(status.Equals(Convert.ToByte(0)))
            {
                subscriptionManager.DistributeMessage(headerContents[0], headerContents[1], body);
            }
            else if (status.Equals(Convert.ToByte(0)))
            {
                Debug.LogErrorFormat("Error from server. {0}:{1}", header, Encoding.UTF8.GetString(body));
            }
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            connection.Close();
        }

    }

}