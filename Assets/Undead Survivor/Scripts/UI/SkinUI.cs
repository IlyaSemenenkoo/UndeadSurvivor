using UnityEngine;
using UnityEngine.UI;

public class SkinUI : MonoBehaviour
{
    [SerializeField] private Image[] Skins;
    
    private Vector3 _defaultScale = new Vector3(3f, 3f, 3f);
    private Vector3 _selectedScale = new Vector3(4f, 4f, 4f);
    private string _skin = "Skin";
    private void Awake()
    {
        if (PlayerPrefs.HasKey(_skin))
        {
            MakeVisible(PlayerPrefs.GetInt(_skin));
        }
        else
        {
            MakeVisible(0);
            PlayerPrefs.SetInt(_skin, 0);
        }
    }

    public void ChangeSkin(int skin)
    {
        MakeVisible(skin);
        PlayerPrefs.SetInt(_skin, skin);
    }

    private void MakeVisible(int skin)
    {
        for (int i = 0; i < Skins.Length; i++)
        {
            Skins[i].transform.localScale = _defaultScale;
            if (i == skin)
            {
                Skins[i].transform.localScale = _selectedScale;
            }
        }
    }
}
