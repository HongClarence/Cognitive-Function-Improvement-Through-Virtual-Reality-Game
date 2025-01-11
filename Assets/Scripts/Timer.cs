using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;

public class Timer : MonoBehaviour
{    
    public enum GameMode { Hat, Book, Word, Fruit };
    public GameMode gameMode;
    public enum Mode { Challenge, Tutorial };
    public Mode mode;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timeTextBox;
    [SerializeField] TextMeshProUGUI scoreCurrent;
    public float timeLeft = 60.0f;

    [Header("Summary")]
    public GameObject canvasSummary;
    [SerializeField] TextMeshProUGUI textRank;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] TextMeshProUGUI textUserRank;
    [SerializeField] TextMeshProUGUI textUserName;
    [SerializeField] TextMeshProUGUI textUserScore;
    [SerializeField] TextMeshProUGUI textUserPercent;


    [Header("Audio")]
    public AudioClip clipFinish;
    public float volume = 1.0f;
    private AudioSource source;

    [Header("Button")]
    public GameObject button;
    public GameButton gameButton;

    private string prefKey;
    private bool timerOn = false;

    void Start()
    {
        source = GetComponent<AudioSource>();
        timeTextBox.text = timeLeft.ToString();
        scoreCurrent.text = gameButton.score.ToString();

        switch (gameMode)
        {
            case GameMode.Hat:
                prefKey = "ChallengeHat";
                break;

            case GameMode.Book:
                prefKey = "ChallengeBook";
                break;

            case GameMode.Word:
                prefKey = "ChallengeWord";
                break;

            case GameMode.Fruit:
                prefKey = "ChallengeFruit";
                break;
        }
    }

    void Update()
    {
        if (timerOn)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timerOn = false;
                timeLeft = 0.0f;

                if(mode == Mode.Challenge)
                {
                    source.PlayOneShot(clipFinish, volume);
                    button.SetActive(false);
                    canvasSummary.SetActive(true);
                    IDbConnection dbConnection = CreateAndOpenDatabase();

                    string username = System.Environment.UserName;
                    int highscore = 0;
                    if (PlayerPrefs.HasKey(prefKey))
                        highscore = PlayerPrefs.GetInt(prefKey);
                    if (gameButton.score > highscore)
                    {
                        PlayerPrefs.SetInt(prefKey, gameButton.score);
                        PlayerPrefs.Save();

                        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
                        dbCommandInsertValue.CommandText = "INSERT OR REPLACE INTO " + prefKey + " (id, highscore) VALUES (\"" + username + "\" , " + gameButton.score + ")";
                        dbCommandInsertValue.ExecuteNonQuery();
                        dbConnection.Close();
                    }

                    IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
                    dbCommandReadValues.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY highscore DESC) num, * FROM " + prefKey + " LIMIT 10";
                    IDataReader dataReader = dbCommandReadValues.ExecuteReader();
                    string rank = "Rank\n";
                    string name = "Name\n";
                    string score = "Score\n";
                    while (dataReader.Read())
                    {
                        rank += dataReader.GetInt32(0) + "\n";
                        name += dataReader.GetString(1) + "\n";
                        score += dataReader.GetInt32(2) + "\n";
                    }


                    dbCommandReadValues = dbConnection.CreateCommand();
                    dbCommandReadValues.CommandText = "SELECT count(*) FROM " + prefKey + " WHERE highscore >= " + highscore;
                    dataReader = dbCommandReadValues.ExecuteReader();
                    int rankHigh = 999;
                    while (dataReader.Read())
                        rankHigh = dataReader.GetInt32(0);

                    dbCommandReadValues = dbConnection.CreateCommand();
                    dbCommandReadValues.CommandText = "SELECT count(*) FROM " + prefKey + " WHERE highscore >= " + gameButton.score;
                    dataReader = dbCommandReadValues.ExecuteReader();
                    int rankCurrent = 999;
                    while (dataReader.Read())
                        rankCurrent = dataReader.GetInt32(0);


                    dbCommandReadValues = dbConnection.CreateCommand();
                    dbCommandReadValues.CommandText = "SELECT count(*) FROM " + prefKey;
                    dataReader = dbCommandReadValues.ExecuteReader();
                    int total = 999;
                    while (dataReader.Read())
                        total = dataReader.GetInt32(0);

                    dbConnection.Close();

                    textRank.text = rank;
                    textName.text = name;
                    textScore.text = score;

                    textUserRank.text = "Rank\n" + rankHigh.ToString("#00") + "\n\n\n\n" + rankCurrent.ToString("#00");
                    textUserName.text = "Name\n" + username + "\n\n\n\n" + username;
                    textUserScore.text = "Score\n" + highscore.ToString("#00") + "\n\n\n\n" + gameButton.score.ToString("#00");
                    textUserPercent.text = "Top\n" + ((rankHigh / total) * 100).ToString("#00") + "%\n\n\n\n" + ((rankCurrent / total) * 100).ToString("#00") + "%";
                }
            }
        }
        timeTextBox.text = "Time: " + timeLeft.ToString("#00.00");
        scoreCurrent.text = "Score: " + gameButton.score.ToString("#00");
    }

    public void TimerStart()
    {
        timerOn = true;
    }

    private IDbConnection CreateAndOpenDatabase()
    {
        string dbUri = "URI=file:Brainworks.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS " + prefKey + " (id TEXT PRIMARY KEY, highscore INTEGER )";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }
}
