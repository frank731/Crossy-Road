using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollsionCheck : MonoBehaviour
{
    public PlayerController playerController;
    public Collider playerCollider;
    private Collider lastLogCollider;

    private void OnCollisionEnter(Collision collision)
    {
        Collider c = collision.collider;

        if (c.CompareTag("Car"))
        {
            playerController.rb.constraints = RigidbodyConstraints.None;
            playerController.rb.AddExplosionForce(10000, collision.transform.position, 100);
            playerController.Die();
            playerController.onLog = false;
        }
  
        else if (c.CompareTag("Kill Zone"))
        {
            playerController.Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            if (lastLogCollider == null || other != lastLogCollider)
            {
                playerController.isJumping = false;
                StartCoroutine(DR(playerCollider));
                transform.position = other.transform.parent.GetChild(0).position;
                playerController.rb.velocity = new Vector3(0, 0, 0);
                
                playerController.onLog = true;
                playerController.currentLog = other.transform.parent.parent.GetComponent<Rigidbody>();
                lastLogCollider = other;
                playerController.goingToLog = true;
                playerController.animator.SetTrigger("Hit Tree"); //cancels jump animation
                GameManager.Instance.CreateRowFront();
            }
        }
    }
    private IEnumerator DR(Collider c) //stops player from getting stuck on log as it will register another collision with the log without this
    {
        c.enabled = false;
        yield return new WaitForSeconds(0.2f);
        c.enabled = true;
    }

}
