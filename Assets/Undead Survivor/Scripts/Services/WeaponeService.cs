using Fusion;
using UnityEngine;

public class WeaponeService : NetworkBehaviour, IPlayerJoined
{
    [SerializeField] private AmmoHandler _ammoHandler;
    [SerializeField] private GameObject[] _weaponsPrefab;
    [SerializeField] private WeaponeSettings[] _weaponeSettings;
    [SerializeField] private GameObject _spawnPoint;
    
    private WeaponeSettings _settings;
    private NetworkObject _networkObject;
    private WeaponeBaseType _weaponeType;
    private float _currentCulldown;
    private Vector3 _spawnPosition = new(-0.19f, -0.23f, 0);

    public void SettingsSetup(int NumberOfWeapon)
    {
        Runner.Spawn(_weaponsPrefab[NumberOfWeapon], _spawnPoint.transform.position, Quaternion.identity, Object.InputAuthority,
            (runner, spawnedObject) =>
            {
                _networkObject = spawnedObject;
                _weaponeType = _networkObject.GetComponent<WeaponeBaseType>();
                _settings = _weaponeSettings[NumberOfWeapon];
                _weaponeType.Initialize(_settings.ProjectilePrefab, _settings.LifeTime, _settings.Speed, _settings.Damage);
                
            });
        
        _ammoHandler.Initialize(_settings.MaxAmmoInMagazine, _settings.MagazineAmount);
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcSetParent(NetworkObject networkObject)
    {
        var weaponTransform = networkObject.transform;
        weaponTransform.SetParent(_spawnPoint.transform);
        weaponTransform.localPosition = _spawnPosition;
        weaponTransform.localRotation = Quaternion.identity;
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

    public void PlayerJoined(PlayerRef player)
    {
        RpcSetParent(_networkObject);
    }

    public void PlayerDead()
    {
        Destroy(_networkObject);
    }
}
