using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialButton : MonoBehaviour
{
    public enum GameMode { Hat, Book, Word, Fruit };
    public GameMode gameMode;
    [SerializeField] TextMeshProUGUI instruction;
    public GameObject[] pyramid;

    private int stage = 0;

    void Start()
    {
        instruction.text = "Press the button to start";
        pyramid[0].SetActive(true);
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
                        pyramid[0].SetActive(false);
                        pyramid[1].SetActive(true);
                        instruction.text = "Memorize the items\nPress the button again";
                        stage++;
                        break;

                    case 1:
                        pyramid[1].SetActive(false);
                        pyramid[2].SetActive(true);
                        instruction.text = "Place back the items based on memory\nPress the button again";
                        stage++;
                        break;

                    case 2:
                        pyramid[2].SetActive(false);
                        pyramid[1].SetActive(true);
                        instruction.text = "Memorize the items again\nDifficulty increases gradually";
                        stage++;
                        break;

                    case 3:
                        pyramid[1].SetActive(false);
                        instruction.text = "Tutorial completed!\nPress quit to leave anytime or stay as long as you like!";
                        break;
                }
                break;


            case GameMode.Word:
            case GameMode.Fruit:
                switch (stage)
                {
                    case 0:
                        pyramid[0].SetActive(false);
                        pyramid[1].SetActive(true);
                        instruction.text = "Eat(bring it near your mouth) based on given prompt\nPress button to change question";
                        stage++;
                        break;

                    case 1:
                        pyramid[0].SetActive(true);
                        pyramid[1].SetActive(false);
                        instruction.text = "Difficulty increases gradually";
                        stage++;
                        break;

                    case 2:
                        pyramid[0].SetActive(false);
                        instruction.text = "Tutorial completed!\nPress quit to leave anytime";
                        break;
                }
                break;
        }
    }
}
