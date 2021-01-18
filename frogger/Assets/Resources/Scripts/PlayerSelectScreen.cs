using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.SaveGameFree;
using UnityEngine.SceneManagement;

public class PlayerSelectScreen : MonoBehaviour
{
    public int playerID;

    public void PlayGame()
    {
        SaveGame.Save("Player Choice", playerID);
        SceneManager.LoadScene(1);
    }

}
