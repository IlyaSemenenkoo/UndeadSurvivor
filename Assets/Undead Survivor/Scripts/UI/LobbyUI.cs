using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject _lobbyName;
    [SerializeField] private InputField _lobbyNameInputField;
    [SerializeField] private Text _lobbyNameWarmingText;

    public void StartGame()
    {
        _lobbyNameWarmingText.text = "";
        if (_lobbyNameInputField.text != "")
        {
            _lobbyName.SetActive(false);
        }
        else
        {
            _lobbyNameWarmingText.text = "Wrong Name!";
        }
    }
}
