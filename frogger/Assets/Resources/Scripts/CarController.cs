using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public AudioClip driveBySlow;
    public AudioClip driveByMedium;
    public AudioClip driveByFast;
    public AudioClip[] honkSFX;

    private AudioSource audioSource;
    private LinearMovementObject linearMovement;
    private float speed;

    private void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();
        linearMovement = transform.parent.GetComponent<LinearMovementObject>();
        speed = linearMovement.speed;

        GameManager.Instance.carsLMO.Add(linearMovement);

        if (speed > 40)
        {
            audioSource.PlayOneShot(driveByFast);
        }
        else if (speed > 30)
        {
            audioSource.PlayOneShot(driveByMedium);
        }
        else
        {
            audioSource.PlayOneShot(driveBySlow);
        }
        
    }

    private void OnBecameVisible()
    {
        if(speed > 40)
        {
            StartCoroutine(PlayHonk(Random.Range(0.1f, 0.5f)));
        }
        else if(speed > 30)
        {
            StartCoroutine(PlayHonk(Random.Range(0.3f, 0.6f)));
        }
        else
        {
            StartCoroutine(PlayHonk(Random.Range(0.6f, 1f)));
        }
    }

    private IEnumerator PlayHonk(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource.PlayOneShot(honkSFX[Random.Range(0, honkSFX.Length)], 0.5f);
    }
}
