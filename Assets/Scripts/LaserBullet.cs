using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private float lifetime = 8f;

    private Obstacle target = null;
    public Obstacle Target { get => target; set => target = value; }

    private Collider2D collider;
    private SpriteRenderer sr;

    private float deathTime = 2f;

    public UnityEvent onCollide;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        Invoke("Kill", lifetime);
    }

    private void Update()
    {
        if(Target)
        {
            Vector3 difference = Target.transform.position - transform.position;
            Vector3 dir = difference.normalized;

            transform.position += dir * speed * Time.deltaTime;

            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform == Target.transform)
        {
            Target.Kill();
            Kill();

            onCollide?.Invoke();
        }
    }

    private void Kill()
    {
        if(collider)
            collider.enabled = false;

        if (sr)
            sr.enabled = false;

        Destroy(gameObject, deathTime);
    }
}
