using System;
using Fusion;
using UnityEngine;
using System.Collections;

public class AmmoHandler : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(SyncAmmo))] private int AmmoAmount { get; set; }
    [Networked] public int MagazineAmount { get; set; }
    [Networked] private int MaxAmmoInMagazine { get; set; }
    private bool _reload;
    
    public event Action<int, int> OnAmmoChangedEvent;

    private void SyncAmmo()
    {
        if (HasInputAuthority)
        {
            OnAmmoChangedEvent?.Invoke(AmmoAmount, MagazineAmount * MaxAmmoInMagazine);
        }
    }
    
    public void Initialize(int MaxAmmoInMagazine, int MagazineAmount)
    {
        this.MaxAmmoInMagazine = MaxAmmoInMagazine;
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
        if (MagazineAmount > 0 && !_reload)
        {
            StartCoroutine(Reload());
        }
    }
    
    private IEnumerator Reload()
    {
        _reload = true;
        yield return new WaitForSeconds(1f);
        AmmoAmount = MaxAmmoInMagazine;
        MagazineAmount -= 1;
        _reload = false;
    }

    public void AddAmmo(int ammo)
    {
        MagazineAmount += ammo;
        StartSync();
    }

    public void StartSync()
    {
        SyncAmmo();
    }
}
