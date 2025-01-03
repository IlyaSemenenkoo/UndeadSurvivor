using System.Runtime.InteropServices.ComTypes;
using Fusion;
using UnityEngine;
using System.Collections;

public class AmmoHandler : MonoBehaviour
{
    [Networked] public int AmmoAmount {get; private set;}
    [Networked] public int MagazineAmount {get; private set;}
    private int _maxAmmoInMagazine;

    public void Initialize(int MaxAmmoInMagazine, int magazineAmount)
    {
        _maxAmmoInMagazine = MaxAmmoInMagazine;
        MagazineAmount = magazineAmount;
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
}
