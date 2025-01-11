using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelChallenge;
    public GameObject panelTutorial;
    public GameObject panelLeaderboard;

    void Start()
    {
        ShowMain();
    }

    public void ShowMain()
    {
        panelMain.SetActive(true);
        panelChallenge.SetActive(false);
        panelTutorial.SetActive(false);
        panelLeaderboard.SetActive(false);
    }

    public void ShowChallenge()
    {
        panelMain.SetActive(false);
        panelChallenge.SetActive(true);
        panelTutorial.SetActive(false);
        panelLeaderboard.SetActive(false);
    }

    public void ShowTutorial()
    {
        panelMain.SetActive(false);
        panelChallenge.SetActive(false);
        panelTutorial.SetActive(true);
        panelLeaderboard.SetActive(false);
    }

    public void ShowLeaderboard()
    {
        panelMain.SetActive(false);
        panelChallenge.SetActive(false);
        panelTutorial.SetActive(false);
        panelLeaderboard.SetActive(true);
    }
}
