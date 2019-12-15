using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Magnet : PickUp
{
    protected override void OnPickUp()
    {
        base.OnPickUp();

        FindObjectOfType<PlayerAbility>().SetPickUp(this);
    }

}
