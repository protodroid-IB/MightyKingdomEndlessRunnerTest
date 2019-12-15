using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagnetAttractor : MonoBehaviour
{
    [SerializeField]
    private float attractSpeed = 20f;

    private List<Coin> coinsToAttract = new List<Coin>();

    [SerializeField]
    private AudioSource source;
    

    private void Start()
    {
        GameManager.instance.onStopGame.AddListener(() =>
        {
            coinsToAttract.Clear();
        });
    }

    private void OnEnable()
    {
        source.Play();
    }

    private void OnDisable()
    {
        source.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Coin coin = collision.GetComponentInParent<Coin>();

        if(coin)
        {
            coinsToAttract.Add(coin);
            coin.overrideVelocity = true;
            coin.onDestroy.AddListener(() =>
            {
                coinsToAttract.Remove(coin);
            });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Coin coin = collision.GetComponentInParent<Coin>();

        if (coin)
        {
            coinsToAttract.Remove(coin);
            coin.overrideVelocity = false;
        }
    }

    private void Update()
    {
        if (coinsToAttract.Count <= 0)
            return;

        foreach(Coin coin in coinsToAttract)
        {
            Vector3 direction = (transform.position - coin.transform.position).normalized;

            coin.transform.position += direction * attractSpeed * Time.deltaTime;
        }

        

        

        //coin.addVelocity.AddListener(() =>
        //{
        //    //Debug.Log(coin.gameObject.name + "\tVelocity Before: " + coin.Velocity);

            

        //    //Debug.Log(coin.gameObject.name + "\tVelocity After: " + coin.Velocity);
        //});
        
    }
}
