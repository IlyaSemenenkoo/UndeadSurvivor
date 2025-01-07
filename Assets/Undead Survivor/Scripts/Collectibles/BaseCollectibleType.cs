using System;
using Fusion;
using UnityEngine;

public abstract class BaseCollectibleType : NetworkBehaviour
{
    public abstract void OnCollisionEnter(Collision other);
}
