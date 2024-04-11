using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public BoxCollider SpawnRegion;
    public bool CullAndSpawnNew = true;//will remove the oldest gameobject instance when the queue fills up every spawn delay tick
    public float SpawnDelay = 3.0f;
    public int MaxSpawnCount = 1;

    private Queue<GameObject> spawnQueue = new Queue<GameObject>();
    private float timerCurrent = 0.0f;
    private Vector3[] SpawnBounds = new Vector3[1];

    private GameObject temp;


    // Start is called before the first frame update
    void Start()
    {
        if (MaxSpawnCount < 0)
        {
            MaxSpawnCount = 0;
        }
        if (SpawnDelay <= 0.0f)
        {
            SpawnDelay = 0.1f;
        }
        //SpawnBounds[0] = new Vector3
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnQueue.Count >= MaxSpawnCount)//removes first instance spawned by spawner
        {
            temp = spawnQueue.Dequeue();
            Destroy(temp);
            temp = null;
        }

        if (spawnQueue.Count > MaxSpawnCount)
        {
            

            //temp = Instantiate(ObjectToSpawn,)
        }
    }
}
