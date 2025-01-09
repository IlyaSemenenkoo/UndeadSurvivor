using TMPro;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private TextMeshPro _ammoText;
    
    private AmmoHandler _ammoHandler;

    private void OnEnable()
    {
        _ammoHandler.OnAmmoChangedEvent += SetAmmo;
    }

    private void SetAmmo(int ammo, int magazineAmmo)
    {
        _ammoText.text = $"{ammo} / {magazineAmmo}";
    }

    private void OnDisable()
    {
        _ammoHandler.OnAmmoChangedEvent -= SetAmmo;
    }
}

