using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class DestructiveObject : MonoBehaviour
{
    public GameObject cubeMesh;

    public Rigidbody rb;
    public float Non0Speed;
    public float blastRadiusMult = 1f;

    // Particle implementation
    public ParticleSystem particles;

    public LayerMask Destructable;

    public int pieceCount = 100;

    private float brokenScale = 0.5f;

    public float destructionRadius;

    public float destructionForce=5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
      //  brokenScale = cubeMesh.transform.localScale.x;
    }

    private void Update()
    {
        if(rb.velocity.magnitude > 0f)
        {
            Non0Speed = rb.velocity.magnitude;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        destructionRadius = this.transform.localScale.x + Non0Speed * blastRadiusMult;
        Collider[] objectsToDestroy = Physics.OverlapSphere(this.transform.position, destructionRadius, Destructable);

        //brokenScale = ((2*destructionRadius)^3)/

         var emitParams = new ParticleSystem.EmitParams();
         emitParams.position = this.transform.position;
         emitParams.applyShapeToPosition = true;
         emitParams.velocity = this.GetComponent<Rigidbody>().velocity;
        particles.Emit(emitParams, 10);

        foreach (Collider Obj in objectsToDestroy)
        {
            switch(Obj.gameObject.tag)
            {
                case "Cubic":
                    CubicDestruction(Obj.gameObject);
                    break;
            }
        }
    }

    public void CubicDestruction(GameObject ObjToDest)
    {
        float cubeWidth = ObjToDest.transform.localScale.x;
        float cubeHeight = ObjToDest.transform.localScale.y;
        float cubeDepth = ObjToDest.transform.localScale.z;
        cubeMesh.gameObject.GetComponent<Transform>().localScale = new Vector3(brokenScale, brokenScale, brokenScale);
        
        //since the transform position is in the centre of the object, the loop has to go to the negative half of the width, height, and depth of the object to re-generate it without shifting it about.
        for(float x = -cubeWidth/2; x < cubeWidth/2; x += brokenScale)
        {
            for (float y = -cubeHeight/2; y < cubeHeight/2; y += brokenScale)
            {
                for (float z = -cubeDepth/2; z < cubeDepth / 2; z += brokenScale)
                {
                    GameObject cube = Instantiate(cubeMesh,ObjToDest.transform.position+new Vector3(x,y,z),Quaternion.identity);
                    if((cube.transform.position-this.transform.position).magnitude< destructionRadius)
                    {
                        cube.AddComponent<Rigidbody>();
                        cube.GetComponent<Rigidbody>().AddForce(((cube.transform.position-this.transform.position).normalized*destructionForce)/ (cube.transform.position - this.transform.position).magnitude, ForceMode.Impulse);
                        ParticleSystem cubeParticles = cube.GetComponent<ParticleSystem>();

                        // Particle test
                        //particles.Stop();
                        // print(this.transform.position.y);

                        //var emitParams = new ParticleSystem.EmitParams();
                        //emitParams.applyShapeToPosition = true;
                        //emitParams.velocity = cube.GetComponent<Rigidbody>().velocity;
                        //cubeParticles.Emit(emitParams, 4);
                        cubeParticles.Play();
                    }
                }
            }
        }
        Destroy(ObjToDest);
    }
}
