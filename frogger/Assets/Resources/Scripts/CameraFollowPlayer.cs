using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public PlayerController playerController;
    public Vector3 offset = new Vector3(0, 29, -30);
    public float smoothSpeed = 10f;
    
    //private Coroutine moveCoroutine;
    private Vector3 desiredPosition;

    private void Start()
    {
        FollowPlayer(0.00001f);
        //playerController = player.GetComponent<PlayerController>();
        //player = player.GetChild(0);
    }

    public void FollowPlayer(float speed)
    {
        if (!playerController.onLog)
        {
            StartCoroutine(GlobalFunctions.MoveObject(transform, transform.position, player.position + offset, speed));
        }
    }

    public void LateUpdate()
    {
        
        if (playerController.onLog)
        {
            //StopAllCoroutines();
            /*
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }*/
            desiredPosition = player.position + offset;
            desiredPosition = new Vector3(desiredPosition.x, offset.y, desiredPosition.z);
            StartCoroutine(GlobalFunctions.MoveObject(transform, transform.position, desiredPosition, 0.3f));
            /*
            desiredPosition = player.position + offset;
            desiredPosition = new Vector3(desiredPosition.x, offset.y, desiredPosition.z);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition,  smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
            */
            //transform.position = new Vector3(player.position.x + offset.x, offset.y, player.position.z + offset.z);
            /*
            if(moveCoroutine != null)
            {
                
            }
            */

            //Debug.Log(transform.position);
        }
        }
}
