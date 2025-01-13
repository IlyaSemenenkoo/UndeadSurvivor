using UnityEngine;
using TMPro;


public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TextMeshProUGUI _nameWarmingText;
    [SerializeField] private GameObject _changeSkinCanvas;
    [SerializeField] private GameObject _lobbyCanvas;
    
    private void Awake()
    {
        _nameWarmingText.text = "";
        _lobbyCanvas.SetActive(false);
        _changeSkinCanvas.SetActive(false);
        if (PlayerPrefs.HasKey("Name"))
        {
            _nameInputField.text = PlayerPrefs.GetString("Name");
        }
    }

    private bool CheckNameInputField()
    {
        if (_nameInputField.text == "")
        {
            _nameWarmingText.text = "Wrong Name!";
            return false;
        }
        else
        {
            PlayerPrefs.SetString("Name", _nameInputField.text);
            _nameInputField.text = "";
            return true;
        }
    }

    public void StartGame(string gameMode)
    {
        if (CheckNameInputField())
        {
            PlayerPrefs.SetString("GameMode", gameMode);
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
