using UnityEngine;
using TMPro;


public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TextMeshProUGUI _nameWarmingText;
    [SerializeField] private GameObject _changeSkinCanvas;
    [SerializeField] private GameObject _lobbyCanvas;
    
    private string _name = "Name";
    private string _wrongName = "Wrong Name!";
    private string _gameMode = "GameMode";
    
    private void Awake()
    {
        _nameWarmingText.text = "";
        _lobbyCanvas.SetActive(false);
        _changeSkinCanvas.SetActive(false);
        if (PlayerPrefs.HasKey(_name))
        {
            _nameInputField.text = PlayerPrefs.GetString(_name);
        }
    }

    private bool CheckNameInputField()
    {
        if (_nameInputField.text == "")
        {
            _nameWarmingText.text = _wrongName;
            return false;
        }
        else
        {
            PlayerPrefs.SetString(_name, _nameInputField.text);
            _nameInputField.text = "";
            return true;
        }
    }

    public void StartGame(string gameMode)
    {
        if (CheckNameInputField())
        {
            PlayerPrefs.SetString(_gameMode, gameMode);
            _lobbyCanvas.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void ChangeSkin()
    {
        _changeSkinCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
