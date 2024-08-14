using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ServerObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnParent;

    private readonly Dictionary<string, GameObject> _serverObjects = new Dictionary<string, GameObject>();
    private string _localPlayerId;

    private void Start()
    {
        RegisterHandlers();
    }

    private void RegisterHandlers()
    {
        WebSocketEventDispatcher.Instance.RegisterHandler("register", RegisterLocalPlayer);
        WebSocketEventDispatcher.Instance.RegisterHandler("spawn", SpawnPlayer);
        WebSocketEventDispatcher.Instance.RegisterHandler("disconnect", DisconnectPlayer);
    }

    private void RegisterLocalPlayer(JObject jsonData)
    {
        _localPlayerId = jsonData["id"].ToString();
        Debug.Log("Registered local player with ID: " + _localPlayerId);
    }

    private void SpawnPlayer(JObject jsonData)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var playerInfo = jsonData["player"]?.ToObject<PlayerInfo>();

            if (playerInfo != null)
            {
                GameObject player = Instantiate(_playerPrefab, _spawnParent);
                player.transform.position = playerInfo.Position;

                _serverObjects.Add(playerInfo.Id, player);
            }
        });
    }

    private void DisconnectPlayer(JObject jsonData)
    {
        string playerId = jsonData["id"].ToString();
        
        if (_serverObjects.ContainsKey(playerId))
        {
            Destroy(_serverObjects[playerId]);
            _serverObjects.Remove(playerId);
            Debug.Log("Player with ID " + playerId + " disconnected and removed.");
        }
    }
}
