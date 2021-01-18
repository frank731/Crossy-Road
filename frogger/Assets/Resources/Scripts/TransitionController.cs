using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TransitionController : Singleton<TransitionController>
{
    public Image whiteScreen;
    public bool startFade = false;
    private void Start()
    {
        SceneManager.sceneLoaded += ChangedActiveScene;
        DontDestroyOnLoad(transform.parent.gameObject);
    }
    public void FadeToWhite(float duration)
    {
        whiteScreen.DOFade(1, duration);
    }
    public void FadeFromWhite(float duration)
    {
        whiteScreen.DOFade(0, duration);
    }

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            FadeFromWhite(3);
        }
    }

}
