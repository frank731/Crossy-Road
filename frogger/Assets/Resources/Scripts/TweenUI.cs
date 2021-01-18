using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenUI : MonoBehaviour
{
    public Transform uiTransform;
    public float animDuration;
    public float animDestination;
    public Ease animEase;

    private void Start()
    {
        uiTransform
            .DOMoveY(animDestination, animDuration)
            .SetEase(animEase)
            .SetLoops(-1, LoopType.Yoyo);

    }
}
