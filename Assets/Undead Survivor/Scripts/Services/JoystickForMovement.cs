using UnityEngine;

public class JoystickForMovement : JoystickHandler
{
    public Vector2 ReturnVectorDirection()
    {
        if(InputVector.x != 0 || InputVector.y != 0) return new Vector2(InputVector.x, InputVector.y);
        else return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
