using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WrangledTimer))]
public class Pog : AnimalBase
{
    public override float GetEscapeVelocity()
    {
        return 10;
    }
}
