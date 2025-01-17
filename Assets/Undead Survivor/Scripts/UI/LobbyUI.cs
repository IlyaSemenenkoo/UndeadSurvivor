using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _lobbyNameInputField;
    [SerializeField] private TextMeshProUGUI _lobbyNameWarmingText;

    private string _lobbyName = "LobbyName";
    private string _wrongName = "Wrong Name!";
    private string _gameScene = "GameScene";
    
    public void StartGame()
    {
        if (_lobbyNameInputField.text == "")
        {
            _lobbyNameWarmingText.text = _wrongName;
        }
        else
        {
            PlayerPrefs.SetString(_lobbyName, _lobbyNameInputField.text);   
            _lobbyNameWarmingText.text = "";
            SceneManager.LoadScene(_gameScene);
        }
    }
}
