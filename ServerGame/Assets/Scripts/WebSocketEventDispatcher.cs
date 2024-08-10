using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WebSocketEventDispatcher : MonoBehaviour
{
    private Dictionary<string, Action<JObject>> _eventHandlers = new Dictionary<string, Action<JObject>>();
    private WebSocket _ws;

    private static WebSocketEventDispatcher instance;

    public static WebSocketEventDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("WebSocketEventDispatcher");
                instance = go.AddComponent<WebSocketEventDispatcher>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    public void Start()
    {
        _ws = new WebSocket("ws://localhost:52300");

        _ws.OnMessage += OnMessageReceived;
        _ws.Connect();
    }

    public void OnDestroy() => _ws?.Close();

    public void RegisterHandler(string eventType, Action<JObject> handler)
    {
        if (!_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] = handler;
        }
        else
        {
            _eventHandlers[eventType] += handler;
        }
    }

    public void UnregisterHandler(string eventType, Action<JObject> handler)
    {
        if (_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] -= handler;
        }
    }

    public void SendMessageToServer(string eventType, object data)
    {
        string jsonData = JsonConvert.SerializeObject(data);
    
        var message = new ClientMessage<object> { Type = eventType, Data = jsonData };
    
        string jsonMessage = JsonConvert.SerializeObject(message);
        _ws.Send(jsonMessage);
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        Debug.Log("Received raw data: " + e.Data);
        var message = JsonConvert.DeserializeObject<ServerMessage<JObject>>(e.Data);
        if (message != null && _eventHandlers.ContainsKey(message.Type))
        {
            _eventHandlers[message.Type]?.Invoke(message.Data);
        }
    }
}