using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public Rigidbody2D RB { get => rb; set => rb = value; }

    public Platform Platform { get; set; }

    public Vector2 Velocity { get; set; } = Vector2.zero;


    [HideInInspector]
    public UnityEvent addVelocity;

    [HideInInspector]
    public Vector2 toAddVelocity = Vector2.zero;

    public bool overrideVelocity = false;




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
        if (Platform && !overrideVelocity)
        {
            Velocity = new Vector2(Platform.RB.velocity.x, RB.velocity.y);
        }
    }


    private void FixedUpdate()
    {
        rb.velocity = Velocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "ObstacleDeathZone")
        {
            Destroy(gameObject);
        }
    }
}
