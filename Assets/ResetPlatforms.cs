using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(ObjectSpawner))]
public class ResetPlatforms : MonoBehaviour
{

    [SerializeField]
    private List<Vector3> platformsStartingPos = new List<Vector3>();

    [SerializeField]
    private GameObject startingPrefab;

    private ObjectSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<ObjectSpawner>();

        for (int i = 0; i < transform.childCount; i++)
        {
            platformsStartingPos.Add(transform.GetChild(i).position);
        }

        GameManager.instance.onResetGame.AddListener(Reset);
    }


    [ContextMenu("Reset Platforms")]
    public void Reset()
    {
        for(int i=0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (Vector3 platformPosition in platformsStartingPos)
        {
            Instantiate(startingPrefab, platformPosition, Quaternion.identity, transform);
        }

        spawner.Initialise();
        
    }
}
