using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.FalconSocka.RNGHero
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Seriazable Fields

        [SerializeField] byte maxPlayersPerRoom = 2;

        #endregion

        #region Private Fields

        const string gameVersion = "0.1";

        #endregion

        #region Public Fields

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if(progressLabel!=null)
                progressLabel.SetActive(false);
            if(controlPanel!=null)
                controlPanel.SetActive(true);
        }
        #endregion

        #region Public Methods

        public void Connect()
        {
            if(progressLabel!=null)
            progressLabel.SetActive(true);
            if(controlPanel!=null)
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster was called");
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            if (progressLabel != null)
                progressLabel.SetActive(false);
            if (controlPanel != null)
                controlPanel.SetActive(true);
            Debug.LogWarningFormat("OnDisconnected was called - reason {0}", cause);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom });
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("PlayerEnteredRoom - changing lvl");
            if(PhotonNetwork.IsMasterClient)
{
                PhotonNetwork.LoadLevel("Game Screen");
            }
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            PhotonNetwork.LoadLevel("Waiting Screen");
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom() was called. Now this client is a room.");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Waiting Screen");
            }           
        }

        #endregion
    }
}
