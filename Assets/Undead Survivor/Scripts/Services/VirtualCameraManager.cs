using Cinemachine;
using Fusion;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameLogic _gameLogic;
    private GameObject _target = null;
    
    public static VirtualCameraManager Singleton
    {
        get => _singleton;
        set
        {
            if (value == null)
                _singleton = null;
            else if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Destroy(value);
            }
        }
    }
    public static VirtualCameraManager _singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void FollowThis(GameObject target)
    {
        if (_target != target)
        {
            _target = target;
            _virtualCamera.Follow = target.transform;
            _virtualCamera.LookAt = target.transform;
        }
    }

    public void PlayerDead(PlayerRef playerRef)
    {
        Debug.Log(playerRef + " " + _gameLogic.GetSecondPlayer(playerRef).GetComponent<NetworkObject>().InputAuthority);
        FollowThis(_gameLogic.GetSecondPlayer(playerRef));
    }
}
