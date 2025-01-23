using Cinemachine;
using Fusion;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameLogic _gameLogic;
    private GameObject _target = null;
    
    private void CreateInstance()
    {
        if (_singleton == null)
        {
            _singleton = this;
        }
    }
    public static VirtualCameraManager _singleton;

    private void Awake()
    {
        CreateInstance();
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
