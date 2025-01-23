using UnityEngine;
using Fusion;

public enum AnimationType 
{
    Idle = 0,
    Run = 1,
    Died = 2,
}

public abstract class BaseAnimController : NetworkBehaviour
{ 
    protected abstract void SyncAnimation();

    public abstract void SetAnimation(AnimationType type);
}
