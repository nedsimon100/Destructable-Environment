using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructiveObject : MonoBehaviour
{
    public GameObject cubeMesh;

    public Rigidbody rb;
    private float Non0Speed = 1;
    public float blastRadiusMult = 0.1f;

    public LayerMask Destructable;

    public int pieceCount = 8;

    private float brokenScale;

    private float destructionRadius;

    public float destructionForce=6f;


    public float minDestructSpeed = 5f;
    private void Start()
    {
        if (this.GetComponent<Rigidbody>() == null)
        {
            this.AddComponent<Rigidbody>();
        }
        rb = GetComponent<Rigidbody>();
        //  brokenScale = cubeMesh.transform.localScale.x;
        Destructable = LayerMask.GetMask("Destructable");
    }

    private void Update()
    {
        if(rb.velocity.magnitude > 0 )
        {
            Non0Speed = rb.velocity.magnitude;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (Non0Speed > minDestructSpeed) 
        {
            destructionRadius = (this.transform.localScale.x / 2) + (Non0Speed * blastRadiusMult);
            
            Non0Speed = 0f;

            destroyRange();
        }
        
    }


    private void destroyRange()
    {
 
        brokenScale = destructionRadius / pieceCount;//scale of broken pieces changes depending on scale of destruction


        Collider[] objectsToDestroy = Physics.OverlapSphere(this.transform.position, destructionRadius, Destructable);

        foreach (Collider Obj in objectsToDestroy)
        {
            if (Obj.GetComponent<Rigidbody>() != null && Obj.AddComponent<ObjectTimeOut>() != null)
            {
                // doesnt try to break or blow away cubes that have just been broken

            }
            else if (Obj.gameObject.transform.localScale.x < brokenScale*2f) // if cube is less that the size of 8 broken cubes dont break and just adds rigidbody
            {
                if (Obj.GetComponent<Rigidbody>() == null)
                {
                    Obj.AddComponent<Rigidbody>();
                    Obj.AddComponent<ObjectTimeOut>();

                }
                Obj.gameObject.GetComponent<Rigidbody>().AddForce(((Obj.transform.position - this.transform.position).normalized * destructionForce) / (Obj.transform.position - this.transform.position).magnitude, ForceMode.Impulse);
            }
            else
            {
                switch (Obj.gameObject.tag)
                {
                    case "Cubic":
                        CubicDestruction(sliceShape(Obj.gameObject));
                        break;
                }
            }

        }
    }

    public void RandColour(GameObject obj) // for debugging to show different shapes and how object was destroyed
    {
        
        obj.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public GameObject sliceShape(GameObject ObjToSlice)
    {
        //slice x scale of shape to fit damage radius
        if (ObjToSlice.transform.position.x + (ObjToSlice.transform.localScale.x / 2) > (this.transform.position.x + destructionRadius)+ brokenScale)
        {
            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3((ObjToSlice.transform.position.x + (ObjToSlice.transform.localScale.x / 2))- (this.transform.position.x + destructionRadius), ObjToSlice.transform.localScale.y, ObjToSlice.transform.localScale.z);
            GameObject cubeoutRange = Instantiate(cubeMesh, ObjToSlice.transform.position +new Vector3((ObjToSlice.transform.localScale.x / 2)-(cubeMesh.gameObject.GetComponent<Transform>().localScale.x / 2), 0,0), Quaternion.identity);
            cubeoutRange.layer = ObjToSlice.layer;
            cubeoutRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeoutRange.AddComponent<ObjectTimeOut>();
            }

            RandColour(cubeoutRange);

            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(ObjToSlice.transform.localScale.x- cubeoutRange.transform.localScale.x, ObjToSlice.transform.localScale.y, ObjToSlice.transform.localScale.z);
            GameObject cubeinRange = Instantiate(cubeMesh, new Vector3(((ObjToSlice.transform.position.x - (ObjToSlice.transform.localScale.x / 2)) + cubeMesh.gameObject.GetComponent<Transform>().localScale.x/2), ObjToSlice.transform.position.y, ObjToSlice.transform.position.z), Quaternion.identity);
            cubeinRange.layer = ObjToSlice.layer;
            cubeinRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeinRange.AddComponent<ObjectTimeOut>();
            }

            Destroy(ObjToSlice);
            return sliceShape(cubeinRange);
        }
        else if (ObjToSlice.transform.position.x - (ObjToSlice.transform.localScale.x / 2) < (this.transform.position.x - destructionRadius)- brokenScale)
        {
            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3((this.transform.position.x - destructionRadius) - (ObjToSlice.transform.position.x - (ObjToSlice.transform.localScale.x / 2)) , ObjToSlice.transform.localScale.y, ObjToSlice.transform.localScale.z);
            GameObject cubeoutRange = Instantiate(cubeMesh, ObjToSlice.transform.position + new Vector3((-ObjToSlice.transform.localScale.x / 2) + (cubeMesh.gameObject.GetComponent<Transform>().localScale.x / 2), 0, 0), Quaternion.identity);
            cubeoutRange.layer = ObjToSlice.layer;
            cubeoutRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeoutRange.AddComponent<ObjectTimeOut>();
            }

            RandColour(cubeoutRange);

            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(ObjToSlice.transform.localScale.x - cubeoutRange.transform.localScale.x, ObjToSlice.transform.localScale.y, ObjToSlice.transform.localScale.z);
            GameObject cubeinRange = Instantiate(cubeMesh, new Vector3(((ObjToSlice.transform.position.x + (ObjToSlice.transform.localScale.x / 2)) - cubeMesh.gameObject.GetComponent<Transform>().localScale.x / 2), ObjToSlice.transform.position.y, ObjToSlice.transform.position.z), Quaternion.identity);
            cubeinRange.layer = ObjToSlice.layer;
            cubeinRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeinRange.AddComponent<ObjectTimeOut>();
            }

            Destroy(ObjToSlice);
            return sliceShape(cubeinRange);
        }
        // slice shapes local y scale to fit damage radius
        else if (ObjToSlice.transform.position.y + (ObjToSlice.transform.localScale.y / 2) > (this.transform.position.y + destructionRadius)+brokenScale)
        {
            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3( ObjToSlice.transform.localScale.x, (ObjToSlice.transform.position.y + (ObjToSlice.transform.localScale.y / 2)) - (this.transform.position.y + destructionRadius), ObjToSlice.transform.localScale.z);
            GameObject cubeoutRange = Instantiate(cubeMesh, ObjToSlice.transform.position + new Vector3( 0, (ObjToSlice.transform.localScale.y / 2) - (cubeMesh.gameObject.GetComponent<Transform>().localScale.y / 2), 0), Quaternion.identity);
            cubeoutRange.layer = ObjToSlice.layer;
            cubeoutRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeoutRange.AddComponent<ObjectTimeOut>();
            }

            RandColour(cubeoutRange);

            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3( ObjToSlice.transform.localScale.x, ObjToSlice.transform.localScale.y - cubeoutRange.transform.localScale.y, ObjToSlice.transform.localScale.z);
            GameObject cubeinRange = Instantiate(cubeMesh, new Vector3( ObjToSlice.transform.position.x, ((ObjToSlice.transform.position.y - (ObjToSlice.transform.localScale.y / 2)) + cubeMesh.gameObject.GetComponent<Transform>().localScale.y / 2), ObjToSlice.transform.position.z), Quaternion.identity);
            cubeinRange.layer = ObjToSlice.layer;
            cubeinRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeinRange.AddComponent<ObjectTimeOut>();
            }

            Destroy(ObjToSlice);
            return sliceShape(cubeinRange);
        }
        else if (ObjToSlice.transform.position.y - (ObjToSlice.transform.localScale.y / 2) < (this.transform.position.y - destructionRadius)- brokenScale)
        {
            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3( ObjToSlice.transform.localScale.x, (this.transform.position.y - destructionRadius) - (ObjToSlice.transform.position.y - (ObjToSlice.transform.localScale.y / 2)), ObjToSlice.transform.localScale.z);
            GameObject cubeoutRange = Instantiate(cubeMesh, ObjToSlice.transform.position + new Vector3( 0, (-ObjToSlice.transform.localScale.y / 2) + (cubeMesh.gameObject.GetComponent<Transform>().localScale.y / 2), 0), Quaternion.identity);
            cubeoutRange.layer = ObjToSlice.layer;
            cubeoutRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeoutRange.AddComponent<ObjectTimeOut>();
            }

            RandColour(cubeoutRange);

            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3( ObjToSlice.transform.localScale.x, ObjToSlice.transform.localScale.y - cubeoutRange.transform.localScale.y, ObjToSlice.transform.localScale.z);
            GameObject cubeinRange = Instantiate(cubeMesh, new Vector3( ObjToSlice.transform.position.x, ((ObjToSlice.transform.position.y + (ObjToSlice.transform.localScale.y / 2)) - cubeMesh.gameObject.GetComponent<Transform>().localScale.y / 2), ObjToSlice.transform.position.z), Quaternion.identity);
            cubeinRange.layer = ObjToSlice.layer;
            cubeinRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeinRange.AddComponent<ObjectTimeOut>();
            }

            Destroy(ObjToSlice);
            return sliceShape(cubeinRange);
        }
        // slice shapes local z scale to fit damage radius
        else if (ObjToSlice.transform.position.z + (ObjToSlice.transform.localScale.z / 2) > (this.transform.position.z + destructionRadius)+ brokenScale)
        {
            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(ObjToSlice.transform.localScale.x,  ObjToSlice.transform.localScale.y, (ObjToSlice.transform.position.z + (ObjToSlice.transform.localScale.z / 2)) - (this.transform.position.z + destructionRadius));
            GameObject cubeoutRange = Instantiate(cubeMesh, ObjToSlice.transform.position + new Vector3(0, 0, (ObjToSlice.transform.localScale.z / 2) - (cubeMesh.gameObject.GetComponent<Transform>().localScale.z / 2)), Quaternion.identity);
            cubeoutRange.layer = ObjToSlice.layer;
            cubeoutRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeoutRange.AddComponent<ObjectTimeOut>();
            }

            RandColour(cubeoutRange);

            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(ObjToSlice.transform.localScale.x, ObjToSlice.transform.localScale.y, ObjToSlice.transform.localScale.z - cubeoutRange.transform.localScale.z);
            GameObject cubeinRange = Instantiate(cubeMesh, new Vector3(ObjToSlice.transform.position.x, ObjToSlice.transform.position.y, ((ObjToSlice.transform.position.z - (ObjToSlice.transform.localScale.z / 2)) + cubeMesh.gameObject.GetComponent<Transform>().localScale.z / 2)), Quaternion.identity);
            cubeinRange.layer = ObjToSlice.layer;
            cubeinRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeinRange.AddComponent<ObjectTimeOut>();
            }

            Destroy(ObjToSlice);
            return sliceShape(cubeinRange);
        }
        else if (ObjToSlice.transform.position.z - (ObjToSlice.transform.localScale.z / 2) < (this.transform.position.z - destructionRadius)- brokenScale)
        {
            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(ObjToSlice.transform.localScale.x, ObjToSlice.transform.localScale.y, (this.transform.position.z - destructionRadius) - (ObjToSlice.transform.position.z - (ObjToSlice.transform.localScale.z / 2)));
            GameObject cubeoutRange = Instantiate(cubeMesh, ObjToSlice.transform.position + new Vector3( 0, 0, (-ObjToSlice.transform.localScale.z / 2) + (cubeMesh.gameObject.GetComponent<Transform>().localScale.z / 2)), Quaternion.identity);
            cubeoutRange.layer = ObjToSlice.layer;
            cubeoutRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeoutRange.AddComponent<ObjectTimeOut>();
            }

            RandColour(cubeoutRange);

            cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(ObjToSlice.transform.localScale.x, ObjToSlice.transform.localScale.y, ObjToSlice.transform.localScale.z - cubeoutRange.transform.localScale.z);
            GameObject cubeinRange = Instantiate(cubeMesh, new Vector3(ObjToSlice.transform.position.x, ObjToSlice.transform.position.y, ((ObjToSlice.transform.position.z + (ObjToSlice.transform.localScale.z / 2)) - cubeMesh.gameObject.GetComponent<Transform>().localScale.z / 2)), Quaternion.identity);
            cubeinRange.layer = ObjToSlice.layer;
            cubeinRange.tag = ObjToSlice.tag;

            if (ObjToSlice.GetComponent<Rigidbody>() != null)
            {
                cubeinRange.AddComponent<ObjectTimeOut>();
            }

            Destroy(ObjToSlice);

            return sliceShape(cubeinRange);
        }
        return ObjToSlice;
    }

    public void CubicDestruction(GameObject ObjToDest)
    {
        float cubeWidth = ObjToDest.transform.localScale.x;
        float cubeHeight = ObjToDest.transform.localScale.y;
        float cubeDepth = ObjToDest.transform.localScale.z;

        float brokenScaleX = cubeWidth / Mathf.CeilToInt(cubeWidth / brokenScale);
        float brokenScaleY = cubeHeight / Mathf.CeilToInt(cubeHeight / brokenScale);
        float brokenScaleZ = cubeDepth / Mathf.CeilToInt(cubeDepth / brokenScale);


        cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(brokenScaleX, brokenScaleY, brokenScaleZ);
        
        //since the transform position is in the centre of the object, the loop has to go to the negative half of the width, height, and depth of the object to re-generate it without shifting it about.
        for(float x = (-cubeWidth/2)+(brokenScaleX/2); x < cubeWidth/2; x += brokenScaleX)
        {
            for (float y = -cubeHeight/2 + (brokenScaleY / 2); y < cubeHeight/2; y += brokenScaleY)
            {
                for (float z = -cubeDepth/2 + (brokenScaleZ / 2); z < cubeDepth / 2; z += brokenScaleZ)
                {
                    GameObject cube = Instantiate(cubeMesh, ObjToDest.transform.position + new Vector3(x, y, z), Quaternion.identity);

                    cube.layer = ObjToDest.layer;
                    cube.tag = ObjToDest.tag;
                    RandColour(cube);
                    if ((cube.transform.position-this.transform.position).magnitude < destructionRadius || ObjToDest.GetComponent<Rigidbody>() != null)
                    {
                        
                        cube.AddComponent<Rigidbody>();
                        cube.AddComponent<ObjectTimeOut>();
                        cube.GetComponent<Rigidbody>().AddForce(((cube.transform.position-this.transform.position).normalized*destructionForce)/ (cube.transform.position - this.transform.position).magnitude, ForceMode.Impulse);
                    }
                }
            }
        }
        Destroy(ObjToDest);
    }

    public void addRBandTimeOut(GameObject obj)
    {

    }
}
