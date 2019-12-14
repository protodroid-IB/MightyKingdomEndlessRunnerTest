using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "DeathZone")
        {
            player.Die();
        }
        else if (other.transform.tag == "Obstacle")
        {
            player.Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "DeathZone")
        {
            player.Die();
        }
        else if (collision.transform.tag == "Obstacle")
        {
            player.Die();
        }
    }
}
