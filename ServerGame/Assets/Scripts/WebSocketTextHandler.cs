using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WebSocketTextHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _receivedText;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _sendButton;

    private void Start()
    {
        RegisterJson();
        
        _sendButton.onClick.AddListener(() =>
        {
            SendMessageToServer();
        });
    }
    private void RegisterJson() => 
        WebSocketEventDispatcher.Instance.RegisterHandler("register", ShowText);

    private void ShowText(JObject jsonData)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var playerInfo = jsonData.ToObject<PlayerInfo>();
            
            if (playerInfo != null)
            {
                _receivedText.text = playerInfo.Id;
            }
        });
    }

    private void SendMessageToServer()
    {
        MessageData messageToSend = new MessageData
        {
            Input = _inputField.text,
            Rofl = 11
        };

        WebSocketEventDispatcher.Instance.SendMessageToServer("register", messageToSend);
        Debug.Log("Sent message: " + JsonConvert.SerializeObject(messageToSend));
    }

}