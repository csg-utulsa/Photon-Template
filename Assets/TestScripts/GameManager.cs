using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace net.EthanTFH.BTSGameJam
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields
        [field: Tooltip("The prefab to use for representing the player.")]
        public GameObject playerPrefabBig;
        public GameObject playerPrefabSmall;
        

        #endregion

        #region Photon Callbacks

        public override void OnLeftRoom()
        {
            Debug.Log("PUN Launcher: Left the room.");
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
            
            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        #endregion

        #region Public Methods

        void Start()
        {
            if (playerPrefabBig == null)
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference on the GameManager.");
            else
            {
                Debug.LogFormat("We are Instatiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                if(PlayerController.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("Making a new LocalPlayer for {0}", SceneManagerHelper.ActiveSceneName);
                    
                    if (GameObject.Find("Floor"))
                        PhotonNetwork.Instantiate(this.playerPrefabBig.name, new Vector3(0f, GameObject.Find("Floor").transform.position.y + 5, 0f), Quaternion.identity, 0);
                    if (PhotonNetwork.CurrentRoom.PlayerCount == 2) 
                    {
                        Debug.Log("Player Count 2");
                    }
                    else
                        PhotonNetwork.Instantiate(this.playerPrefabBig.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);

                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        void LoadArena()
        {
            if(!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master client.");
            }
            Debug.LogFormat("PhotonNetwork: Loading Level: {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Test_Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion

    }
}