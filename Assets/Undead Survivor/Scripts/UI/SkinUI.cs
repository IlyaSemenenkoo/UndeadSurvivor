using UnityEngine;
using UnityEngine.UI;

public class SkinUI : MonoBehaviour
{

    [SerializeField] private Image[] Skins;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("Skin"))
        {
            MakeVisible(PlayerPrefs.GetInt("Skin"));
        }
        else
        {
            MakeVisible(0);
        }
    }

    public void ChangeSkin(int skin)
    {
        MakeVisible(skin);
        PlayerPrefs.SetInt("Skin", skin);
    }

    private void MakeVisible(int skin)
    {
        for (int i = 0; i < Skins.Length; i++)
        {
            Skins[i].transform.localScale = new Vector3(3, 3, 3);
            if (i == skin)
            {
                Skins[i].transform.localScale = new Vector3(4, 4, 4);
            }
        }
    }
}
