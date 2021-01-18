using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovementObject : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public bool canMove = true;
    public bool slowed = false;

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector3(speed, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Object Destroy"))
        {
            gameObject.SetActive(false);
        }
    }
}
