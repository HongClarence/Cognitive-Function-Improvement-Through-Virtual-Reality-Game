using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void FreePlay()
    {
        SceneManager.LoadScene(0);
    }

    public void ChallengeHat()
    {
        SceneManager.LoadScene(1);
    }

    public void ChallengeBook()
    {
        SceneManager.LoadScene(2);
    }

    public void ChallengeWord()
    {
        SceneManager.LoadScene(3);
    }

    public void ChallengeFruit()
    {
        SceneManager.LoadScene(4);
    }

    public void TutorialHat()
    {
        SceneManager.LoadScene(5);
    }

    public void TutorialBook()
    {
        SceneManager.LoadScene(6);
    }

    public void TutorialWord()
    {
        SceneManager.LoadScene(7);
    }

    public void TutorialFruit()
    {
        SceneManager.LoadScene(8);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
