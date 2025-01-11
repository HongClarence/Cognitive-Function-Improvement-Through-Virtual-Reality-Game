using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textHatRank;
    [SerializeField] TextMeshProUGUI textHatName;
    [SerializeField] TextMeshProUGUI textHatScore;
    [SerializeField] TextMeshProUGUI textBookRank;
    [SerializeField] TextMeshProUGUI textBookName;
    [SerializeField] TextMeshProUGUI textBookScore;
    [SerializeField] TextMeshProUGUI textWordRank;
    [SerializeField] TextMeshProUGUI textWordName;
    [SerializeField] TextMeshProUGUI textWordScore;
    [SerializeField] TextMeshProUGUI textFruitRank;
    [SerializeField] TextMeshProUGUI textFruitName;
    [SerializeField] TextMeshProUGUI textFruitScore;

    private string hatRank;
    private string hatName;
    private string hatScore;
    private string bookRank;
    private string bookName;
    private string bookScore;
    private string wordRank;
    private string wordName;
    private string wordScore;
    private string fruitRank;
    private string fruitName;
    private string fruitScore;

    void Start()
    {
        string username = System.Environment.UserName;
        IDbConnection dbConnection = CreateAndOpenDatabase();

        string prefKey = "ChallengeHat";
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY highscore DESC) num, * FROM " + prefKey + " LIMIT 5";
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            hatRank += dataReader.GetInt32(0) + "\n";
            hatName += dataReader.GetString(1) + "\n";
            hatScore += dataReader.GetInt32(2) + "\n";
        }

        int highscore = 0; 
        if (PlayerPrefs.HasKey(prefKey))
            highscore = PlayerPrefs.GetInt(prefKey);
        dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT count(*) FROM " + prefKey + " WHERE highscore >= " + highscore;
        dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
            hatRank += dataReader.GetInt32(0);
        hatName += username;
        hatScore += highscore;


        prefKey = "ChallengeBook";
        dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY highscore DESC) num, * FROM " + prefKey + " LIMIT 5";
        dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            bookRank += dataReader.GetInt32(0) + "\n";
            bookName += dataReader.GetString(1) + "\n";
            bookScore += dataReader.GetInt32(2) + "\n";
        }

        highscore = 0;
        if (PlayerPrefs.HasKey(prefKey))
            highscore = PlayerPrefs.GetInt(prefKey);
        dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT count(*) FROM " + prefKey + " WHERE highscore >= " + highscore;
        dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
            bookRank += dataReader.GetInt32(0);
        bookName += username;
        bookScore += highscore;


        prefKey = "ChallengeWord";
        dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY highscore DESC) num, * FROM " + prefKey + " LIMIT 5";
        dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            wordRank += dataReader.GetInt32(0) + "\n";
            wordName += dataReader.GetString(1) + "\n";
            wordScore += dataReader.GetInt32(2) + "\n";
        }

        highscore = 0;
        if (PlayerPrefs.HasKey(prefKey))
            highscore = PlayerPrefs.GetInt(prefKey);
        dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT count(*) FROM " + prefKey + " WHERE highscore >= " + highscore;
        dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
            wordRank += dataReader.GetInt32(0);
        wordName += username;
        wordScore += highscore;


        prefKey = "ChallengeFruit";
        dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY highscore DESC) num, * FROM " + prefKey + " LIMIT 5";
        dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            fruitRank += dataReader.GetInt32(0) + "\n";
            fruitName += dataReader.GetString(1) + "\n";
            fruitScore += dataReader.GetInt32(2) + "\n";
        }

        highscore = 0;
        if (PlayerPrefs.HasKey(prefKey))
            highscore = PlayerPrefs.GetInt(prefKey);
        dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT count(*) FROM " + prefKey + " WHERE highscore >= " + highscore;
        dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
            fruitRank += dataReader.GetInt32(0);
        fruitName += username;
        fruitScore += highscore;
        dbConnection.Close();


        textHatRank.text = hatRank;
        textHatName.text = hatName;
        textHatScore.text = hatScore;
        textBookRank.text = bookRank;
        textBookName.text = bookName;
        textBookScore.text = bookScore;
        textWordRank.text = wordRank;
        textWordName.text = wordName;
        textWordScore.text = wordScore;
        textFruitRank.text = fruitRank;
        textFruitName.text = fruitName;
        textFruitScore.text = fruitScore;
    }


    private IDbConnection CreateAndOpenDatabase()
    {
        string dbUri = "URI=file:Brainworks.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        return dbConnection;
    }
}
