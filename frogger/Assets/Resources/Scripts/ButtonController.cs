using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject settingsScreen;
    public GameObject playerSelectScreen;
    public GameObject titleScreen;
    public List<GameObject> disableUI = new List<GameObject>();

    public void CloseGame()
    {
        Application.Quit();
    }

    public void BackToHome()
    {
        Time.timeScale = 1;
        GameManager.Instance.transitionController.FadeToWhite(1f);
        StartCoroutine(BackToHomeWait(1f));
    }

    public void OpenSettings()
    {
        settingsScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenPlayerSelect(GameObject currentScreen = null)
    {
        playerSelectScreen.SetActive(true);
        if(currentScreen != null)
        {
            currentScreen.SetActive(false);
        }
        foreach(GameObject screen in disableUI)
        {
            screen.SetActive(false);
        }
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Physics.IgnoreLayerCollision(7, 9);
        }
        
    }

    public void ClosePlayerSelect() {
        playerSelectScreen.SetActive(false);
        if(titleScreen != null)
        {
            titleScreen.SetActive(true);
        }
        foreach (GameObject screen in disableUI)
        {
            screen.SetActive(true);
        }
        Physics.IgnoreLayerCollision(7, 9, false);
    }

    private IEnumerator BackToHomeWait(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(0);
    }
}
