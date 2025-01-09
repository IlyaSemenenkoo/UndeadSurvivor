using System;
using Fusion;
using UnityEngine;
using System.Collections;

public class AmmoHandler : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(SyncAmmo))] private int AmmoAmount {get; set;}
    [Networked] private int MagazineAmount { get; set; }
    private int _maxAmmoInMagazine;
    [SerializeField] private WeaponeSettings _settings;
    
    public event Action<int, int> OnAmmoChangedEvent;

    private void SyncAmmo()
    {
        if (HasInputAuthority)
        {
            OnAmmoChangedEvent?.Invoke(AmmoAmount, MagazineAmount * _maxAmmoInMagazine);
        }
    }
    
    public override void Spawned()
    {
        _maxAmmoInMagazine = _settings.MaxAmmoInMagazine;
        MagazineAmount = _settings.MagazineAmount;
        AmmoAmount = _settings.MaxAmmoInMagazine;
    }

    public bool MagazineIsEmpty()
    {
        return AmmoAmount == 0;
    }

    public void Shoot()
    {
        AmmoAmount -= 1;
    }

    public void ChangeMagazine()
    {
        if (MagazineAmount > 0)
        {
            StartCoroutine(Reload());
        }
    }
    
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);
        AmmoAmount = _maxAmmoInMagazine;
        MagazineAmount -= 1;
    }

    public void AddAmmo(int ammo)
    {
        MagazineAmount += ammo;
    }
}
