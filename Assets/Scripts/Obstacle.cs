using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Obstacle : InteractableObject
{
    private float deathTime = 2f;

    private Collider2D collider;
    private SpriteRenderer sr;

    private void Awake()
    {
        collider = GetComponentInChildren<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Kill()
    {
        if (collider)
            collider.enabled = false;

        if (sr)
            sr.enabled = false;

        Destroy(gameObject, deathTime);
    }
}
