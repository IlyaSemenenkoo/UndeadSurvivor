using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject _lobbyName;
    [SerializeField] private InputField _lobbyNameInputField;
    [SerializeField] private Text _lobbyNameWarmingText;
    
    public event Action<GameMode, string> OnGameStarteEvent;

    public void StartGame()
    {
        _lobbyNameWarmingText.text = "";
        if (_lobbyNameInputField.text != "")
        {
            _lobbyName.SetActive(false);
            GameMode mode = Enum.Parse<GameMode>(PlayerPrefs.GetString("GameMode"));
            OnGameStarteEvent?.Invoke(mode, _lobbyNameInputField.text);
        }
        else
        {
            _lobbyNameWarmingText.text = "Wrong Name!";
        }
    }
}
