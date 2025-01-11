using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttentionButton : MonoBehaviour
{
    public enum GameMode { Words, Fruits };
    public GameMode gameMode;
    public AudioClip clipCorrect;
    public AudioClip clipWrong;
    public float volume = 1.0f;
    public XRSocketInteractor[] sockets;
    public GameObject[] prefabs;
    public int score = 0;

    private AudioSource source;
    private int stage = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void onButtonPress()
    {
        switch (stage)
        {
            case 0:
                SpawnObjects();
                stage = 1;
                break;

            case 1:
                DestroyObjects();
                SpawnObjects();
                break;
        }
    }

    private void SpawnObjects()
    {
        bool[] used = new bool[sockets.Length];
        int index = Random.Range(0, sockets.Length);

        for (int i = 0; i < 9; i++)
        {
            while (used[index])
                index = Random.Range(0, sockets.Length);

            used[index] = true;
            Instantiate(prefabs[i], sockets[index].transform.position, sockets[index].transform.rotation);
        }
    }

    private void DestroyObjects()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            try
            {
                Destroy(socket.GetOldestInteractableSelected().transform.gameObject);
            }
            catch { }
        }
    }
}
