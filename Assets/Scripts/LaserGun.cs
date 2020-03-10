using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserGun : MonoBehaviour
{
    [SerializeField]
    private GameObject laserBullet;

    [SerializeField]
    private Transform bulletSpawn;

    public UnityEvent onShoot;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacle obstacle = collision.GetComponentInParent<Obstacle>();

        if(obstacle)
        {
            Debug.Log("Obstacle In Range!");
            LaserBullet bullet = Instantiate(laserBullet, bulletSpawn.position, Quaternion.identity).GetComponent<LaserBullet>();
            bullet.Target = obstacle;
            onShoot?.Invoke();
        }
    }

    private void OnDisable()
    {
        if(bulletSpawn)
        {
            if(bulletSpawn.childCount > 0)
            {
                for(int i=0; i < bulletSpawn.childCount; i++)
                {
                    Destroy(bulletSpawn.GetChild(i).gameObject);
                }
            }
        }
    }
}
