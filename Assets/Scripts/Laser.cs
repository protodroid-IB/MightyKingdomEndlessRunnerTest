using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : PickUp
{
    protected override void OnPickUp()
    {
        base.OnPickUp();

        FindObjectOfType<PlayerAbility>().SetPickUp(this);
    }
}
