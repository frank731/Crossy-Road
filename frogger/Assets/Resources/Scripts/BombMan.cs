using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMan : MonoBehaviour
{
    public TimerController timerController;
    public PlayerController playerController;
    public GameObject explosion;
    public float explosionRadius;
    public float explosionForce;

    private void Start()
    {
        timerController.StartTimer();
        timerController.timerFinished.AddListener(Explode);
        playerController.moveForward.AddListener(ResetTimer);
        playerController.logReanableTime = new WaitForSeconds(0.075f);
        GameManager.Instance.transform.GetComponent<ButtonController>().disableUI.Add(timerController.transform.parent.gameObject);
    }

    private void ResetTimer()
    {
        timerController.StartTimer();
    }

    private void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Collider[] objects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider h in objects)
        {
            if(h.CompareTag("Car") || h.CompareTag("Log"))
            {
                Rigidbody r = h.GetComponentInParent<Rigidbody>();
                if (r != null)
                {
                    r.constraints = RigidbodyConstraints.None;
                    r.GetComponent<LinearMovementObject>().canMove = false;
                    r.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }
        playerController.Explode();
    }
    
}
