using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ObjectTimeOut : MonoBehaviour
{
    public float shrinkTime = 0.05f;
    public Rigidbody rb;
    void Start()
    {
        if(this.GetComponent<Rigidbody>() == null)
        {
            this.AddComponent<Rigidbody>();
        }
        rb = this.GetComponent<Rigidbody>();

        StartCoroutine(destroyRB());

        if (this.transform.localScale.magnitude < 0.25f)
        {
            StartCoroutine(shrinkDamage());
        }
    }

    IEnumerator shrinkDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(shrinkTime);
            this.transform.localScale *= 0.999f;
        }
    }

    IEnumerator destroyRB()
    {
        bool moving = true;
        while (moving)
        {
            yield return new WaitForSeconds(5f);
            if (this.GetComponent<Rigidbody>() != null && rb.velocity.magnitude <= 0.05f)
            {
                //Destroy(rb); //remove rigid body after set time
                if (this.transform.localScale.magnitude >= 0.25f)
                {
                    Destroy(this);
                }
                moving = false;
            }
            
        }
    }

    public void FixedUpdate()
    {
        
        if (this.transform.localScale.magnitude < 0.01f)
        {
            Destroy(this.gameObject);
        }
    }

}
