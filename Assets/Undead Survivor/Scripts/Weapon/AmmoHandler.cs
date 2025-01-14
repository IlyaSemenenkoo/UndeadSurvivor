using System;
using Fusion;
using UnityEngine;
using System.Collections;

public class AmmoHandler : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(SyncAmmo))] private int AmmoAmount {get; set;}
    [Networked] private int MagazineAmount { get; set; }
    private int _maxAmmoInMagazine; 
    
    public event Action<int, int> OnAmmoChangedEvent;

    private void SyncAmmo()
    {
        if (HasInputAuthority)
        {
            OnAmmoChangedEvent?.Invoke(AmmoAmount, MagazineAmount * _maxAmmoInMagazine);
        }
    }
    
    public void Initialize(int MaxAmmoInMagazine, int MagazineAmount)
    {
        _maxAmmoInMagazine = MaxAmmoInMagazine;
        this.MagazineAmount = MagazineAmount;
        AmmoAmount = MaxAmmoInMagazine;
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

    public void StartSync()
    {
        SyncAmmo();
    }
}
