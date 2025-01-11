using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class GameButton : MonoBehaviour
{
    public enum GameMode { Hat, Book, Word, Fruit };
    public GameMode gameMode;
    public enum Mode { Freeplay, Challenge };
    public Mode mode;

    public int score = 0;
    [SerializeField] TextMeshProUGUI wordTextBox;

    [Header("Audio")]
    public AudioClip clipCorrect;
    public AudioClip clipWrong;
    public float volume = 1.0f;

    [Header("Interactables")]
    public XRSocketInteractor[] sockets;
    public GameObject[] prefabs;
    public GameObject mouthWord;
    public GameObject mouthFruit;

    private XRSocketInteractor mouth;
    private string[] prefabAnswer;
    private string[] prefabUser;
    private AudioSource source;
    private bool set = true;
    private int stage = 0;
    private string prefKey;

    void Start()
    {
        prefabAnswer = new string[sockets.Length];
        prefabUser = new string[sockets.Length];
        source = GetComponent<AudioSource>();
        if(mode == Mode.Freeplay)
        {
            switch (gameMode)
            {
                case GameMode.Hat:
                    prefKey = "FreeplayHat";
                    break;

                case GameMode.Book:
                    prefKey = "FreeplayBook";
                    break;

                case GameMode.Word:
                    prefKey = "FreeplayWord";
                    break;

                case GameMode.Fruit:
                    prefKey = "FreeplayFruit";
                    break;
            }

            if (PlayerPrefs.HasKey(prefKey))
                score = PlayerPrefs.GetInt(prefKey);
        }
    }

    void Update()
    {
        CheckMouth();
    }

    public void OnButtonPress()
    {
        switch (gameMode)
        {
            case GameMode.Hat:
            case GameMode.Book:
                switch (stage)
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
                break;

            case GameMode.Word:
            case GameMode.Fruit:
                if(score < 4)
                    prefabAnswer[0] = prefabs[Random.Range(0, prefabs.Length - 6)].transform.name;
                else if(score < 7)
                    prefabAnswer[0] = prefabs[Random.Range(0, prefabs.Length - 3)].transform.name;
                else
                    prefabAnswer[0] = prefabs[Random.Range(0, prefabs.Length)].transform.name;

                SetText();
                DestroyObjects();
                SpawnObjects();
                break;
        }
    }

    private void SpawnObjects()
    {
        bool[] used = new bool[sockets.Length];
        int index = Random.Range(0, sockets.Length);

        switch (gameMode)
        {
            case GameMode.Hat:
                for (int i = 0; i < score + 1 && i < sockets.Length; i++)
                {
                    while (used[index])
                        index = Random.Range(0, sockets.Length);

                    used[index] = true;
                    Instantiate(prefabs[Random.Range(0, prefabs.Length)], sockets[index].transform.position, sockets[index].transform.rotation);
                }
                break;


            case GameMode.Book:
                for (int i = 0; i < score + 1 && i < sockets.Length; i++)
                    Instantiate(prefabs[Random.Range(0, prefabs.Length)], sockets[i].transform.position, sockets[i].transform.rotation);
                break;


            case GameMode.Word:
            case GameMode.Fruit:
                for (int i = 0; i < 9; i++)
                {
                    while (used[index])
                        index = Random.Range(0, sockets.Length);

                    used[index] = true;
                    Instantiate(prefabs[i], sockets[index].transform.position, sockets[index].transform.rotation);
                }
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
        PlayerPrefs.SetInt(prefKey, score);
        PlayerPrefs.Save();
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

    private void CheckMouth()
    {
        try
        {
            prefabUser[0] = mouth.GetOldestInteractableSelected().transform.name;
        }
        catch
        {
            prefabUser[0] = "0";
        }
        
        if(!prefabUser[0].Equals("0"))
        {
            if(prefabUser[0].Contains(prefabAnswer[0]))
            {
                source.PlayOneShot(clipCorrect, volume);
                score++;
                PlayerPrefs.SetInt(prefKey, score);
                PlayerPrefs.Save();
            }
            else
                source.PlayOneShot(clipWrong, volume);

            Destroy(mouth.GetOldestInteractableSelected().transform.gameObject);
            OnButtonPress();
        }
    }

    private void SetText()
    {
        switch (gameMode)
        {
            case GameMode.Word:
                mouthWord.SetActive(true);
                if(mouthFruit != null)
                    mouthFruit.SetActive(false);
                mouth = mouthWord.GetComponent<XRSocketInteractor>();
                if (prefabAnswer[0].Contains("Yellow"))
                    wordTextBox.text = "Yellow";
                else if (prefabAnswer[0].Contains("Green"))
                    wordTextBox.text = "Green";
                else if (prefabAnswer[0].Contains("Red"))
                    wordTextBox.text = "Red";
                break;

            case GameMode.Fruit:
                mouthFruit.SetActive(true);
                if (mouthWord != null)
                    mouthWord.SetActive(false);
                mouth = mouthFruit.GetComponent<XRSocketInteractor>();
                if (prefabAnswer[0].Contains("Yellow"))
                    wordTextBox.text = "Lemon";
                else if (prefabAnswer[0].Contains("Green"))
                    wordTextBox.text = "Pear";
                else if (prefabAnswer[0].Contains("Red"))
                    wordTextBox.text = "Tomato";
                break;
        }

        if (prefabAnswer[0].Contains("Lemon"))
            wordTextBox.color = Color.yellow;
        else if (prefabAnswer[0].Contains("Pear"))
            wordTextBox.color = Color.green;
        else if (prefabAnswer[0].Contains("Tomato"))
            wordTextBox.color = Color.red;
    }
}
