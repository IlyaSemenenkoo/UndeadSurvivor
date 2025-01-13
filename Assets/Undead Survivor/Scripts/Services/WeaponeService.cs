using Fusion;
using UnityEngine;

public class WeaponeService : NetworkBehaviour
{
    [SerializeField] private AmmoHandler _ammoHandler;
    [SerializeField] private GameObject[] _weaponsPrefab;
    [SerializeField] private WeaponeSettings[] _weaponeSettings;
    [SerializeField] private GameObject _spawnPoint;
    
    private WeaponeSettings _settings;
    private WeaponeBaseType _weaponeType;
    private float _currentCulldown = 0;

    public void SettingsSetup(int NumberOfWeapon)
    {
        Debug.Log("SettingsSetup" + NumberOfWeapon);
        Runner.Spawn(_weaponsPrefab[NumberOfWeapon], _spawnPoint.transform.position, Quaternion.identity, Object.InputAuthority,
            (runner, spawnedObject) =>
            {
                _weaponeType = spawnedObject.GetComponent<WeaponeBaseType>();
                _weaponeType.transform.SetParent(_spawnPoint.transform);
                _weaponeType.transform.localPosition = new Vector3(-0.19f, -0.23f, 0);
                _weaponeType.transform.localRotation = Quaternion.identity;
                _settings = _weaponeSettings[NumberOfWeapon];
                _weaponeType.Initialize(_settings.ProjectilePrefab, _settings.LifeTime, _settings.Speed, _settings.Damage);
            });
        _ammoHandler.Initialize(_settings.MaxAmmoInMagazine, _settings.MagazineAmount);
    }
    public override void FixedUpdateNetwork()
    {
        if (_weaponeType != null)
        {
            if (_currentCulldown >= _settings.AttackCulldown)
            {
                if (GetInput(out NetworkInputData data))
                {
                    if (data.BulletDirection != Vector2.zero)
                    {
                        if (!_ammoHandler.MagazineIsEmpty())
                        {
                            _currentCulldown = 0;
                            _weaponeType.Shoot(data.BulletDirection);
                            _ammoHandler.Shoot();
                        }
                        else
                        {
                            _ammoHandler.ChangeMagazine();
                        }
                    }
                }
            }
            else
            {
                _currentCulldown += Time.fixedDeltaTime;
            }
        }
    }
}
