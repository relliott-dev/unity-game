using UnityEngine;
using TMPro;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System;
using Crosstales.BWF.Model.Enum;
using Crosstales.BWF;

namespace RDE
{
    /// <summary>
    /// 
    /// The class is responsible for managing in-game chat functionality
    /// It allows players to send and receive chat messages in different channels with support for private messages
    /// This class handles UI elements, message formatting, and communication with a server for message storage
    /// 
    /// @TODO:
    /// - Return to fix and implement the chat system
    /// - Add functionality for private messages/system messages
    /// 
    /// </summary>
    public class PlayerChatManager : MonoBehaviour
    {
        #region Variables

        public struct Message : IBroadcast
        {
            public int id;
            public string channel;
            public string timestamp;
            public string sender;
            public string destination;
            public string message;
        }

        [Serializable]
        public struct ChatChannel
        {
            public string channelName;
            public string channelShortcut;
            public Color32 textColor;
            public bool privateMessage;
        }

        [SerializeField] private List<ChatChannel> chatChannels = new List<ChatChannel>();

        [Header("UI Objects")]
        [SerializeField] private GameObject messagePrefab;
        [HideInInspector] public TMP_InputField playerMessage;
        private GameObject messageContent;
        private ScrollRect scrollRect;
        private TMP_Dropdown channelDropdown;

        [Header("Settings")]
        [SerializeField] private int messageHistoryLength = 50;
        [SerializeField] private float miniChatTimer = 5f;
        [SerializeField] private float messageSpamWindow = 2f;
        [SerializeField] private int messageLimit = 5;
        [SerializeField] private float spamCooldown = 5f;

        [Header("Helper Variables")]
        [SerializeField] private string secureKey;
        private List<Message> messageHistory = new List<Message>();
        private string messagesURL;
        private string currentChannel;
        private int messageCount = 0;
        private float lastMessageTime = 0f;
        private Coroutine cooldownCoroutine;

        #endregion

        #region Base Functions

        private void Start()
        {
            messagesURL = "https://nerdbroski.com/gameplay/messages.php";

            scrollRect = PlayerUIManager.instance.chatUI.GetComponentInChildren<ScrollRect>();
            channelDropdown = PlayerUIManager.instance.chatUI.GetComponentInChildren<TMP_Dropdown>();
            playerMessage = PlayerUIManager.instance.chatUI.GetComponentInChildren<TMP_InputField>();
            messageContent = scrollRect.content.gameObject;

            scrollRect.verticalNormalizedPosition = 0f;
            playerMessage.text = "";

            PopulateDropdownOptions();
            channelDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

            //InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageRecieved);
            //InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnClientMessageRecieved);

            PlayerUIManager.instance.chatUI.SetActive(false);
        }

        #endregion

        #region Manage Messages

        //Called when the channel dropdown value changes
        private void OnDropdownValueChanged(int value)
        {
            Message message = new Message()
            {
                channel = "System",
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //sender = AccountManager.instance.GetUsername(),
                destination = "",
                message = "You have left channel: " + currentChannel
            };

            messageHistory.Add(message);

            currentChannel = channelDropdown.options[value].text;

            foreach (Transform child in messageContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Message message2 in messageHistory)
            {
                if (message2.channel == currentChannel || currentChannel == "All")
                {
                    CreateMessage(message);
                }
            }
        }

        //Populate dropdown options based on defined chat channels
        private void PopulateDropdownOptions()
        {
            List<string> channelOptions = new List<string>();
            foreach (ChatChannel channel in chatChannels)
            {
                channelOptions.Add(channel.channelName);
            }
            channelDropdown.AddOptions(channelOptions);
        }

        //Create and display a new chat message
        private void CreateMessage(Message message)
        {
            GameObject finalMessage = Instantiate(messagePrefab, messageContent.transform);
            ChatChannel channelInfo = chatChannels.Find(channel => channel.channelName == message.channel);

            string formattedMessage = FormatMessage(message, channelInfo);
            finalMessage.GetComponent<TextMeshProUGUI>().text = formattedMessage;

            scrollRect.verticalNormalizedPosition = 0f;
            playerMessage.text = "";
        }

        //Format a chat message for display
        private string FormatMessage(Message message, ChatChannel channelInfo)
        {
            string formattedMessage = string.Empty;

            if (channelInfo.channelName == "System")
            {

            }
            else
            {
                if (currentChannel == "All")
                {
                    formattedMessage = $"[{message.channel}] ";
                }

                formattedMessage += $"<color=#{ColorUtility.ToHtmlStringRGB(channelInfo.textColor)}>{message.sender}:</color> {message.message}";
            }

            return formattedMessage;
        }

        #endregion

        #region Update Messages

        //Send a chat message
        public void SendMessage()
        {
            if (cooldownCoroutine != null)
            {
                Debug.LogWarning("Please wait before sending another message");
                return;
            }

            bool hasBadWords = BWFManager.Instance.Contains(playerMessage.text, ManagerMask.BadWord);

            if (hasBadWords)
            {
                //WarningManager.instance.ReportUser(AccountManager.instance.GetUserID(), AccountManager.instance.GetUserID(), playerMessage.text);
                playerMessage.text = BWFManager.Instance.ReplaceAll(playerMessage.text, ManagerMask.BadWord);
            }

            bool isSpam = SpamCheck();

            if (!isSpam)
            {
                Message message = new Message()
                {
                    channel = currentChannel,
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //sender = AccountManager.instance.GetUsername(),
                    destination = "",
                    message = playerMessage.text
                };

                StartCoroutine(SaveMessage(message));
                playerMessage.text = "";

                /*if (InstanceFinder.IsServer)
                    InstanceFinder.ServerManager.Broadcast(message);
                else if (InstanceFinder.IsClient)
                    InstanceFinder.ClientManager.Broadcast(message);*/
            }
        }

        //Checks if spamming
        private bool SpamCheck()
        {
            float currentTime = Time.time;

            if (currentTime - lastMessageTime <= messageSpamWindow)
            {
                messageCount++;

                if (messageCount >= messageLimit)
                {
                    cooldownCoroutine = StartCoroutine(StartCooldown());
                    return true;
                }
            }
            else
            {
                messageCount = 1;
            }

            lastMessageTime = currentTime;
            return false;
        }

        //Starts Cooldown for sending messages
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(spamCooldown);

            messageCount = 0;
        }

        //Coroutine to save a chat message to the server
        private IEnumerator SaveMessage(Message message)
        {
            WWWForm form = new WWWForm();
            form.AddField("channel", message.channel);
            form.AddField("sender", message.sender);
            form.AddField("destination", message.destination);
            form.AddField("message", message.message);
            form.AddField("secureid", secureKey);

            UnityWebRequest request = UnityWebRequest.Post(messagesURL, form);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Message saved: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Message save failed: " + request.error);
            }
        }

        //Handle a chat message received from the server or another client
        private void OnMessageRecieved(Message message)
        {
            messageHistory.Add(message);
            CreateMessage(message);

            while (messageHistory.Count > messageHistoryLength)
            {
                messageHistory.RemoveAt(0);
                Destroy(messageContent.transform.GetChild(0).gameObject);
            }
        }

        //Handle a chat message received from a client
        private void OnClientMessageRecieved(NetworkConnection networkConnection, Message message)
        {
            InstanceFinder.ServerManager.Broadcast(message);
        }

        #endregion
    }
}