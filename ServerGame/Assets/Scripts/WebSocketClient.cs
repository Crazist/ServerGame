using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using WebSocketSharp;

public class WebSocketEventDispatcher : MonoBehaviour
{
    private Dictionary<string, Action<object>> _eventHandlers = new Dictionary<string, Action<object>>();
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

    public void RegisterHandler(string eventType, Action<object> handler)
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

    public void UnregisterHandler(string eventType, Action<object> handler)
    {
        if (_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] -= handler;
        }
    }

    public void SendMessageToServer(string eventType, object data)
    {
        var message = new ClientMessage { Type = eventType, Data = data };
        _ws.Send(JsonUtility.ToJson(message));
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        var message = JsonUtility.FromJson<ServerMessage>(e.Data);
        if (_eventHandlers.ContainsKey(message.Type))
        {
            _eventHandlers[message.Type].Invoke(message.Data);
        }
    }
}