using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public List<GameObject> PossibleSpawns = new List<GameObject>();

    public List<GameObject> cantSpawn;

    public FPSMovement player;

    public bool spawned = false;
    void Start()
    {
        player = FindObjectOfType<FPSMovement>();
        if (transform.position.x > 25 || transform.position.z > 25)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (spawned)
        {
            Destroy(this.gameObject);
        }
        if (player.transform.position.y < this.transform.position.y + 10)
        {
            
            spawnGameObject();
            spawned = true;
        }
        
        
    }
            
    public void spawnGameObject()
    {
        ProceduralGeneration[] allSpawners = FindObjectsOfType<ProceduralGeneration>();
        foreach (ProceduralGeneration projGen in allSpawners)
        {
            if(this.transform == projGen.transform && this != projGen)
            {
                cantSpawn.Clear();
                projGen.spawned = true;
                foreach(GameObject posSpawn in PossibleSpawns)
                {
                    if (!projGen.PossibleSpawns.Contains(posSpawn))
                    {
                        cantSpawn.Add(posSpawn);
                    }
                }
                foreach (GameObject nospawn in cantSpawn)
                {
                    PossibleSpawns.Remove(nospawn);
                }
            }

        }
        if (PossibleSpawns.Count > 0)
        {
            Instantiate(PossibleSpawns[Random.Range(0, PossibleSpawns.Count)], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No possible spawns available!");
        }
    }
        
}
