using UnityEngine;
using Fusion;

public enum AnimationType 
{
    idle = 0,
    run = 1,
    died = 2,
}

public abstract class BaseAnimController : NetworkBehaviour
{ 
    protected abstract void SyncAnimation();

    public abstract void SetAnimation(AnimationType type);
}
