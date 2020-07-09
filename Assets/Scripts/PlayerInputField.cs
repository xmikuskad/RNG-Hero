using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(InputField))]
public class PlayerInputField : MonoBehaviour
{
    #region Private Constants

    const string PLAYER_NAME_PREF_KEY = "PlayerName";

    #endregion

    #region Monobehaviour CallBacks

    private void Start()
    {
        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();
        if(inputField != null)
        {
            if (PlayerPrefs.HasKey(PLAYER_NAME_PREF_KEY)) ;
            {
                defaultName = PlayerPrefs.GetString(PLAYER_NAME_PREF_KEY);
                inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    #endregion

    #region Public Methods

    public void SetPlayerName(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(PLAYER_NAME_PREF_KEY, value);
    }

    #endregion
}
