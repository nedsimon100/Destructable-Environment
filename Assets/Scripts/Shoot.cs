using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shoot : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 aim;

    public GameObject cubeMesh;

    public float ShootForce = 5f;

    public Vector3 spawnOffset;

    private GameObject objToFire;
    public List<GameObject> possibleProjectiles = new List<GameObject>();

    public bool debugging=false;
    void Start()
    {
        objToFire = Instantiate(possibleProjectiles[Random.Range(0, possibleProjectiles.Count)], this.transform.position + spawnOffset, Quaternion.identity);
        objToFire.GetComponent<Collider>().enabled = false;
        objToFire.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        objToFire.transform.localScale *= Random.Range(0.4f, 2.2f);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos += Camera.main.transform.forward * 20f; // Make sure to add some "depth" to the screen point 
        aim = Camera.main.ScreenToWorldPoint(mousePos);
        if (Input.GetMouseButtonDown(0))
        {
            fire();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugging = !debugging;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void fire()
    {
        objToFire.AddComponent<Rigidbody>();
        objToFire.AddComponent<DestructiveObject>();
        objToFire.GetComponent<DestructiveObject>().cubeMesh = this.cubeMesh;
        objToFire.GetComponent<DestructiveObject>().debugging = this.debugging;
        objToFire.GetComponent<Rigidbody>().AddForce(aim * ShootForce);
        objToFire.GetComponent<Collider>().enabled = true;

        objToFire = Instantiate(possibleProjectiles[Random.Range(0, possibleProjectiles.Count)], this.transform.position + spawnOffset, Quaternion.identity);
        objToFire.GetComponent<Collider>().enabled = false;
        objToFire.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        objToFire.transform.localScale *= Random.Range(0.4f, 2.2f);
    }
}
