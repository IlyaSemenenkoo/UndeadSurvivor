using System;
using Fusion;
using UnityEngine;

public abstract class BaseCollectibleType : NetworkBehaviour
{
    public abstract void OnCollisionEnter2D(Collision2D other);
}
