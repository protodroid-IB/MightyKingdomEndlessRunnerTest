using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DestroyWhenOutBounds : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "ObstacleDeathZone")
            Destroy(gameObject);
    }
}
