using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ObjectTimeOut : MonoBehaviour
{
    public float shrinkTime = 0.02f;
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
            this.transform.localScale *= 0.9996f;
        }
    }

    IEnumerator destroyRB()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (this.GetComponent<Rigidbody>() != null && rb.velocity.magnitude == 0f)
            {
                Destroy(rb);
                if (this.transform.localScale.magnitude > 0.25f)
                {
                    Destroy(this);
                }
            }
            
        }
    }

    public void FixedUpdate()
    {
        
        if (this.transform.localScale.magnitude < 0.05f)
        {
            Destroy(this.gameObject);
        }
    }

}
