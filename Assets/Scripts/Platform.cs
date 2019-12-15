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
    public class ItemType
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
    private List<ItemType> itemTypes;

    public Rigidbody2D RB { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (!canSpawnObject)
            return;

        RB = GetComponent<Rigidbody2D>();

        int i = 0;
        int rnd = Random.Range(0, 101);

        while (rnd > 0 && i < itemTypes.Count)
        {
            rnd -= itemTypes[i].chanceToSpawn;

            if (rnd <= 0)
            {
                SpawnItem(i);
            }

            i++;
        }
    }

    private void SpawnItem(int index)
    {
        float xPos = Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x);
        float yPos = spawnZone.bounds.center.y;

        Transform parent = GameObject.FindWithTag("Items").transform;

        if (!parent)
            parent = spawnZone.transform;

        GameObject spawned = Instantiate(itemTypes[index].prefab, new Vector3(xPos, yPos, 0f), Quaternion.identity, parent);

        InteractableObject item = spawned.GetComponent<InteractableObject>();

        if (item)
        {
            item.Platform = this;
        }
        else
        {
            for (int i = 0; i < spawned.transform.childCount; i++)
            {
                item = spawned.transform.GetChild(i).GetComponent<InteractableObject>();

                if (item)
                {
                    item.Platform = this;
                }
            }
        }
    }

    private void OnValidate()
    {

        if (itemTypes.Count <= 0 || !canSpawnObject)
            return;

        // this ensures that the chance values don't exceed a total of 100 for the item prefabs that can possibly spawn on this platform
        int newTotal = 0;

        foreach (ItemType item in itemTypes)
        {
            newTotal += item.chanceToSpawn;
        }

        int i = 0;

        while (newTotal > 100)
        {
            if (itemTypes[i].chanceToSpawn > 0)
            {
                itemTypes[i].chanceToSpawn--;
                newTotal--;
            }

            i++;

            if (i >= itemTypes.Count)
                i = 0;
        }

        totalChance = newTotal;
    }

}
