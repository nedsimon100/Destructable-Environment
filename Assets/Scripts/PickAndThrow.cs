using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndThrow : MonoBehaviour
{
    private GameObject heldObject;
    public float radius = 2f;
    public float distance = 1f;
    public float height = 1f;
    public float throwForce = 50.0f;

    private void Update()
    {
        var t = transform;
        var pressed = Input.GetKeyDown(KeyCode.E);

        if (heldObject)
        {
            if (pressed)
            {
                // Throw the held object
                ThrowObject();
            }
        }
        else
        {
            if (pressed)
            {
                var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
                var hitIndex = Array.FindIndex(hits, hit => hit.transform.CompareTag("Cubic")); // Use CompareTag for performance

                if (hitIndex != -1)
                {
                    var hitObject = hits[hitIndex].transform.gameObject;
                    heldObject = hitObject;
                    var rigidbody = heldObject.GetComponent<Rigidbody>();
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    rigidbody.drag = 25f;
                    rigidbody.useGravity = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (heldObject != null)
        {
            var t = transform;
            var rigidbody = heldObject.GetComponent<Rigidbody>();
            var moveTo = t.position + distance * t.forward + height * t.up;
            var difference = moveTo - heldObject.transform.position;
            rigidbody.AddForce(difference * 500);

            heldObject.transform.rotation = t.rotation;
        }
    }

    private void ThrowObject()
    {
        if (heldObject != null)
        {
            var t = transform;
            var rigidbody = heldObject.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.None; // Allow rotation
            rigidbody.useGravity = true; // Enable gravity for realistic fall
            rigidbody.drag = 1.0f; // Adjust drag to simulate air resistance
            rigidbody.mass = 5.0f;//Adjust the mass to simulate gravity effectivness
            rigidbody.angularDrag = 1.0f; // Adjust angular drag for rotational resistance
            rigidbody.AddForce(t.forward * throwForce, ForceMode.Impulse); // Apply throwing force
            heldObject = null; // Reset held object
        }
    }

}
