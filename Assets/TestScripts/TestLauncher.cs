using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace net.EthanTFH.BTSGameJam {
    public class TestLauncher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [field:SerializeField, Tooltip("The maximum number of players per room."), Header("Room Settings")]
            private byte maxPlayersPerRoom = 4;
        [field: SerializeField, Tooltip("The control panel for the lanucher."), Header("Menu Settings")]
            private GameObject controlPanel;
        [field: SerializeField, Tooltip("The progress label.")]
            private GameObject progressLabel;
        [field: Tooltip("The Game Version. (IMPORTANT)")]
            string gameVersion = "1.5";

        #endregion

        #region Private Fields

        private bool isConnecting = false;        

        #endregion

        #region Private Constants
        const string playerNamePrefKey = "PlayerName";
        #endregion

        #region MonoBehaviour CallBacks
        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();
            if(_inputField != null)
            {
                if(PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods
        
        public void CloseGame()
        {
            PhotonNetwork.Disconnect();

            Application.Quit();
        }
        
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
                PhotonNetwork.JoinRandomRoom();
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length > 13)
            {
                Debug.LogError("Player Name is Empty or over max length.");
                GameObject.Find("PlayButton").GetComponent<Button>().interactable = false;
                return;
            }

            GameObject.Find("PlayButton").GetComponent<Button>().interactable = true;
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Launcher: OnConnectedToMaster() was called.");
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.Log("PUN Launcher: OnDisconnected() called with reason " + cause.ToString());
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Launcher: OnJoinRandomFailed() called with return code " + returnCode + " and message: " + message);
            Debug.Log("PUN Launcher: Attempting to create a new room.");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Launcher: Joined a new room for 1 player(s).");

            PhotonNetwork.LoadLevel("Test_Room for 1");
        }
        #endregion
    }
}