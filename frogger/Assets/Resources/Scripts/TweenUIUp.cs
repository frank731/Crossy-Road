using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TweenUIUp : MonoBehaviour
{
    public Transform uiTransform;
    public float animDuration;
    public float animDestination;
    public Ease animEase;
    public TweenUI tweenUI;
    public PlayerSelectScreen playerSelectScreen;
    public TransitionController transitionController;

    public void GoUp()
    {
        transitionController = GameObject.FindGameObjectWithTag("Transition").transform.GetChild(0).GetComponent<TransitionController>();
        transitionController.FadeToWhite(animDuration);
        tweenUI.enabled = false;
        uiTransform
            .DOMoveY(animDestination, animDuration)
            .SetEase(animEase)
            .OnComplete(playerSelectScreen.PlayGame);
    }

}
