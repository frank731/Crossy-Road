using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectScreenController : MonoBehaviour
{
    public Transform screenHolder;
    public GameObject[] playerSelectScreens;
    public GameObject[] buttons;
    public float transitionSpeed;
    public int index = 0;
    private List<GameObject> notDisabledButtons = new List<GameObject>();
    private int playerSelectScreensLength;

    private void Start()
    {
        playerSelectScreensLength = playerSelectScreens.Length - 1;
    }

    public void LeftCharacter()
    {
        index--;
        buttons[1].SetActive(true);
        if(index == 0)
        {
            buttons[0].SetActive(false);
        }
        Move(true);
    }

    public void RightCharacter()
    {
        index++;
        buttons[0].SetActive(true);
        if (index == playerSelectScreensLength)
        {
            buttons[1].SetActive(false);
        }
        Move(false);
    }

    private void Move(bool isLeft)
    {
        //GameObject nextScreen = playerSelectScreens[index];
        //Vector3 diff = screenHolder.localPosition - nextScreen.transform.localPosition;
        Vector3 diff = new Vector3(-1300, 0, 0);
        if (isLeft)
        {
            diff *= -1;
        }
        //Debug.Log(screenHolder.localosition);
        //Debug.Log(nextScreen.transform.localPosition);
        //Debug.Log(diff);
        StartCoroutine(GlobalFunctions.MoveObject(screenHolder, screenHolder.localPosition, screenHolder.localPosition + diff, transitionSpeed));
        StartCoroutine(DRButtons());
    }

    private IEnumerator DRButtons()
    {
        notDisabledButtons.Clear();
        foreach (GameObject g in buttons)
        {
            if (g.activeSelf)
            {
                notDisabledButtons.Add(g);
                g.SetActive(false);
            }
        }
        yield return new WaitForSeconds(transitionSpeed);
        foreach (GameObject g in notDisabledButtons)
        {
            g.SetActive(true);
        }
    }
}
