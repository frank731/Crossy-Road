using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TimeMan : MonoBehaviour
{
    public TimerController timerController;
    public TimerController cooldownTimerController;
    public float slowTime;
    public AudioMixer audioMixer;

    private bool onCooldown = false;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.playTimeMan = true;
        timerController.startTime = slowTime;
        timerController.timerFinished.AddListener(SlowOver);
        cooldownTimerController.timerFinished.AddListener(CooldownOver);
        gameManager.transform.GetComponent<ButtonController>().disableUI.Add(timerController.transform.parent.gameObject);
    }

    private void SlowOver()
    {
        cooldownTimerController.StartTimer();
        foreach (LinearMovementObject l in gameManager.carsLMO)
        {
            if (l.slowed)
            {
                l.speed *= 2;
                l.slowed = false;
            }
        }
        foreach (SpawningRow s in gameManager.roadsSpawn)
        {
            if (s.slowed)
            {
                s.objectSpeed *= 2;
            }
            s.slowed = false;
        }
        audioMixer.SetFloat("Pitch", 1f);
    }

    private void CooldownOver()
    {
        onCooldown = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && !onCooldown)
        {
            onCooldown = true;
            foreach(LinearMovementObject l in gameManager.carsLMO)
            {
                if (!l.slowed)
                {
                    l.speed /= 2;
                    l.slowed = true;
                }
            }
            foreach(SpawningRow s in gameManager.roadsSpawn)
            {
                if (!s.slowed)
                {
                    s.objectSpeed /= 2;
                    s.slowed = true;
                }
                
            }
            timerController.StartTimer();
            audioMixer.SetFloat("Pitch", 0.5f);
        }
    }
}
