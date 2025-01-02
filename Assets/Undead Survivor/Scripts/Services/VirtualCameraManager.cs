using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    
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
                Debug.LogError($"There should only ever be one instance of {nameof(VirtualCameraManager)}!");
            }
        }
    }
    public static VirtualCameraManager _singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void FollowThis(Transform target)
    {
        Debug.Log("FollowThis");
        _virtualCamera.Follow = target;
        _virtualCamera.LookAt = target;
    }
}
