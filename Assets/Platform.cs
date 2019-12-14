using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleType
    {
        public GameObject prefab;

        [Range(0, 100)]
        public int chanceToSpawn = 0;
    }

    [SerializeField]
    private bool canSpawnObject = true; 

    [SerializeField]
    private Collider2D spawnZone;

    private int totalChance = 0;

    [SerializeField]
    private List<ObstacleType> obstacleTypes;

    private GameObject spawned;
    private Obstacle obstacle;

    private IDisposable moveObstacleSub;

    public Rigidbody2D RB { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (!canSpawnObject)
            return;

        RB = GetComponent<Rigidbody2D>();

        int i = 0;
        int rnd = Random.Range(0, 101);

        while (rnd > 0 && i < obstacleTypes.Count)
        {
            rnd -= obstacleTypes[i].chanceToSpawn;

            if (rnd <= 0)
            {
                SpawnObstacle(i);
            }

            i++;
        }
    }

    private void SpawnObstacle(int index)
    {
        float xPos = Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x);
        float yPos = spawnZone.bounds.center.y;

        Transform parent = GameObject.FindWithTag("Obstacles").transform;

        if (!parent)
            parent = spawnZone.transform;

        spawned = Instantiate(obstacleTypes[index].prefab, new Vector3(xPos, yPos, 0f), Quaternion.identity, parent);
        obstacle = spawned.GetComponent<Obstacle>();
        obstacle.Platform = this;
    }

    private void OnValidate()
    {

        if (obstacleTypes.Count <= 0 || !canSpawnObject)
            return;

        // this ensures that the chance values don't exceed a total of 100 for the obstacle prefabs that can possibly spawn on this platform
        int newTotal = 0;

        foreach (ObstacleType obs in obstacleTypes)
        {
            newTotal += obs.chanceToSpawn;
        }

        int i = 0;

        while (newTotal > 100)
        {
            if (obstacleTypes[i].chanceToSpawn > 0)
            {
                obstacleTypes[i].chanceToSpawn--;
                newTotal--;
            }

            i++;

            if (i >= obstacleTypes.Count)
                i = 0;
        }

        totalChance = newTotal;
    }

    private void OnDestroy()
    {
        moveObstacleSub?.Dispose();
    }

}
