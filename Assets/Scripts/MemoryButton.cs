using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MemoryButton : MonoBehaviour
{
    public enum GameMode { Hat, Book};
    public GameMode gameMode;
    public AudioClip clipCorrect;
    public AudioClip clipWrong;
    public float volume = 1.0f;
    public XRSocketInteractor[] sockets;
    public GameObject[] prefabs;
    public int score = 0;

    private AudioSource source;
    private string[] prefabAnswer;
    private string[] prefabUser;
    private bool set = true;
    private int stage = 0;

    void Start()
    {
        prefabAnswer = new string[sockets.Length];
        prefabUser = new string[sockets.Length];
        source = GetComponent<AudioSource>();
    }

    public void onButtonPress()
    {
        switch(stage)
        {
            case 0:
                SpawnObjects();
                stage = 1;
                break;

            case 1:
                set = true;
                GetObjects();
                DestroyObjects();
                stage = 2;
                break;

            case 2:
                set = false;
                GetObjects();
                CheckObjects();
                DestroyObjects();
                SpawnObjects();
                stage = 1;
                break;
        }
    }

    private void SpawnObjects()
    {
        switch(gameMode)
        {
            case GameMode.Hat:
                bool[] used = new bool[sockets.Length];
                int index = Random.Range(0, sockets.Length);

                for (int i = 0; i < score + 1 && i < sockets.Length; i++)
                {
                    while (used[index])
                        index = Random.Range(0, sockets.Length);

                    used[index] = true;
                    Instantiate(prefabs[Random.Range(0, prefabs.Length)], sockets[index].transform.position, sockets[index].transform.rotation);
                }
                break;

            case GameMode.Book:
                for (int i = 0; i < score + 1 && i < sockets.Length; i ++)
                    Instantiate(prefabs[Random.Range(0, prefabs.Length)], sockets[i].transform.position, sockets[i].transform.rotation);
                break;
        }
    }

    private void GetObjects()
    {
        int i = 0;
        string socketObject;

        foreach (XRSocketInteractor socket in sockets)
        {
            try
            {
                socketObject = socket.GetOldestInteractableSelected().transform.name;
            }
            catch
            {
                socketObject = "0";
            }
            if (set)
                prefabAnswer[i] = socketObject;

            else
                prefabUser[i] = socketObject;
            i++;
        }
    }

    private void CheckObjects()
    {
        int i = 0;
        foreach (string prefab in prefabUser)
        {
            if (!prefab.Contains(prefabAnswer[i]))
            {
                source.PlayOneShot(clipWrong, volume);
                return;
            }
            i++;
        }
        source.PlayOneShot(clipCorrect, volume);
        score++;
    }

    private void DestroyObjects()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            try
            {
                Destroy(socket.GetOldestInteractableSelected().transform.gameObject);
            }
            catch {}
        }
    }
}
