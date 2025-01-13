using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _lobbyNameInputField;
    [SerializeField] private TextMeshProUGUI _lobbyNameWarmingText;

    public void StartGame()
    {
        if (_lobbyNameInputField.text == "")
        {
            _lobbyNameWarmingText.text = "Wrong Name!";
        }
        else
        {
            PlayerPrefs.SetString("LobbyName", _lobbyNameInputField.text);   
            _lobbyNameWarmingText.text = "";
            SceneManager.LoadScene("GameScene");
        }
    }
}
