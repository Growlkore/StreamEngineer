﻿using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SocketIOClient;

namespace GoryMoon.StreamEngineer.Data
{
    public class TwitchExtensionData : IDisposable
    {
        private readonly BaseDataHandler _baseDataHandler;

        private bool _running;
        private SocketIO _socketIo;

        public TwitchExtensionData(BaseDataHandler baseDataHandler)
        {
            _baseDataHandler = baseDataHandler;
        }

        public void Dispose()
        {
            if (_running)
            {
                _socketIo?.CloseAsync();
                _running = false;
            }
        }

        public void Init(string token)
        {
            _socketIo = new SocketIO("ws://localhost:3000/")
            {
                Parameters = new Dictionary<string, string>
                {
                    {"token", token}
                }
            };
            _socketIo.OnConnected += () => _baseDataHandler.Logger.WriteLine("Connected to Twitch Extension");
            _socketIo.OnClosed += OnSocketClosed;
            _socketIo.On("action", args =>
            {
                try
                {
                    _baseDataHandler.Logger.WriteLine("Message: " + args.Text);
                    OnSocketMessage(args.Text);
                }
                catch (Exception e)
                {
                    _baseDataHandler.Logger.WriteLine(e);
                }
            });
            _socketIo.ConnectAsync();
            _running = true;
        }

        private async void OnSocketClosed(ServerCloseReason reason)
        {
            _baseDataHandler.Logger.WriteLine($"Closed Twitch Extension: {reason}");
            if (reason != ServerCloseReason.ClosedByClient)
            {
                _baseDataHandler.Logger.WriteLine("Reconnecting Twitch Extension");
                for (var i = 0; i < 3; i++)
                    try
                    {
                        await _socketIo.ConnectAsync();
                        return;
                    }
                    catch (WebSocketException ex)
                    {
                        _baseDataHandler.Logger.WriteLine(ex.Message);
                        await Task.Delay(2000);
                    }
                    catch (AggregateException ex)
                    {
                        _baseDataHandler.Logger.WriteLine(ex.Message);
                        await Task.Delay(2000);
                    }

                _baseDataHandler.Logger.WriteLine("Tried to reconnect 3 times, unable to connect to the server");
            }

            _running = false;
        }

        private void OnSocketMessage(string args)
        {
            var data = JObject.Parse(args);
            _baseDataHandler.OnTwitchExtension((string) data["user"], (int) data["bits"], (string) data["action"], data["settings"]);
        }
    }
}