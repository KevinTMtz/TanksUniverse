using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayTest()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetGeneralInfoNumPlayers(int num)
    {
        GeneralInfo.numOfPlayers = num;
    }

    public void SetGeneralInfoNumIAs(int num)
    {
        GeneralInfo.numOfIAs = num;
    }
}
