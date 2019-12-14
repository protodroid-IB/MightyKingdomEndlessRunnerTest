using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    private Collider2D lifeSpanCollider;

    [SerializeField]
    private List<GameObject> spawnPrefabs;
    private List<ObjectScroller> spawned = new List<ObjectScroller>();
    private ObjectScroller lastSpawned;

    private IDisposable spawnSub;

    private float spawnXPosition;
    private float lastYSpawnPosition;

    [SerializeField]
    [Range(0.1f, 100f)]
    private float minDistanceApart = 2f, maxDistanceApart = 5f;

    [SerializeField]
    private bool considerHeightApart = false;

    [SerializeField] [Range(0.1f, 15f)]
    private float maxHeightApart = 1f;

    [SerializeField]
    private bool randomiseSpeed = false;

    [SerializeField]
    [Range(0.1f, 100f)]
    private float minSpeed = 0.5f, maxSpeed = 0.6f;




    private void Start()
    {
        lifeSpanCollider = GetComponent<Collider2D>();
        spawnXPosition = lifeSpanCollider.bounds.max.x;

        Initialise();
    }







    public void Initialise()
    {
        spawned = transform.GetComponentsInChildren<ObjectScroller>().ToList();
        lastYSpawnPosition = Random.Range(lifeSpanCollider.bounds.min.y, lifeSpanCollider.bounds.max.y);;

        foreach (ObjectScroller scroller in spawned)
        {
            float speed = randomiseSpeed ? Random.Range(minSpeed, maxSpeed) : (maxSpeed + minSpeed) * 0.5f;

            scroller.SetScrollingObject(speed, lifeSpanCollider);
            lastSpawned = scroller;
        }

        float nextSpawnDistance = Random.Range(minDistanceApart, maxDistanceApart);
        spawnSub = this.UpdateAsObservable()
            .Where(_ => GameManager.instance.Difficulty != GameDifficulty.Stop)
            .Subscribe(_ => CheckCanSpawn(nextSpawnDistance));
    }

    private void CheckCanSpawn(float spawnDistance)
    {
        float distance = Mathf.Abs(lastSpawned.transform.position.x - spawnXPosition);

        if (distance < spawnDistance)
            return;

        SpawnObject();

        spawnSub.Dispose();
        float nextSpawnDistance = Random.Range(minDistanceApart, maxDistanceApart);
        spawnSub = this.UpdateAsObservable()
            .Subscribe(_ => CheckCanSpawn(nextSpawnDistance));
    }

    private void SpawnObject()
    {
        float minY = lifeSpanCollider.bounds.min.y;
        float maxY = lifeSpanCollider.bounds.max.y;

        if (considerHeightApart)
        {
            minY = Mathf.Clamp(lastYSpawnPosition - maxHeightApart, lifeSpanCollider.bounds.min.y, lifeSpanCollider.bounds.max.y);
            maxY = Mathf.Clamp(lastYSpawnPosition + maxHeightApart, lifeSpanCollider.bounds.min.y, lifeSpanCollider.bounds.max.y);
        }

        lastYSpawnPosition = Random.Range(minY, maxY);
        ObjectScroller newObject = Instantiate(spawnPrefabs[Random.Range(0, spawnPrefabs.Count)], new Vector3(spawnXPosition, lastYSpawnPosition, 0), Quaternion.identity, transform).GetComponent<ObjectScroller>();

        float speed = randomiseSpeed ? Random.Range(minSpeed, maxSpeed) : (maxSpeed + minSpeed) * 0.5f;
        newObject.SetScrollingObject(speed, lifeSpanCollider);
        newObject.StartScroll();

        spawned.Add(newObject);
        lastSpawned = newObject;

    }

    private void OnValidate()
    {
        foreach (ObjectScroller scroller in spawned)
        {
            float speed = randomiseSpeed ? Random.Range(minSpeed, maxSpeed) : (maxSpeed + minSpeed) * 0.5f;

            scroller.MaxSpeed = speed;
        }
    }

    
}
