using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    //used to disable ui screens that get opened later so components load properly
    void Start()
    {
        gameObject.SetActive(false);
    }

}
