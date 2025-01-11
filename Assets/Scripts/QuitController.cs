using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitController : MonoBehaviour
{
    public GameObject panelTimer;
    public GameObject panelConfirm;

    void Start()
    {
        ShowTimer();
    }

    public void ShowConfirm()
    {
        panelTimer.SetActive(false);
        panelConfirm.SetActive(true);
    }

    public void ShowTimer()
    {
        panelTimer.SetActive(true);
        panelConfirm.SetActive(false);
    }
}
