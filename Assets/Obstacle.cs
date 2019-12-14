using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public Rigidbody2D RB { get; set; }

    public Platform Platform { get; set; }

    private void Start()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        GameManager.instance.onResetGame.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        if (Platform)
        {
            rb.velocity = new Vector2(Platform.RB.velocity.x, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "ObstacleDeathZone")
        {
            Destroy(gameObject);
        }
    }
}
