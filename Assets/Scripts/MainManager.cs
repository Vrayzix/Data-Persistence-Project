using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    private int m_highestPoints;

    private bool m_GameOver = false;

    [System.Serializable]
    public class BestScore
    {
        public string bestScorePlayerText;
        public string PlayerText;
        public string bestScoreText;
        public string ScoreText;
        public int highestScore;
    }
    [System.Serializable]
    public class PlayerName
    {
        public string PlayerText;
    }


    void Start()
    {
        string path = Application.persistentDataPath + "/BestScoreData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BestScore bestScore = JsonUtility.FromJson<BestScore>(json);
            m_highestPoints = bestScore.highestScore;
            bestScoreText.text = bestScore.bestScoreText;
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        SavePlayerName();
        CompareScores();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    void CompareScores()
    {
        if (m_Points > m_highestPoints)
        {
            BestScore bestScore = new BestScore();

            bestScore.highestScore = m_Points;
            bestScore.bestScorePlayerText = MenuManager.playerName;
            bestScore.ScoreText = ScoreText.text;

            bestScore.bestScoreText = "Best Score : " + bestScore.bestScorePlayerText + " : " + bestScore.highestScore;

            bestScoreText.text = bestScore.bestScoreText;
            string json = JsonUtility.ToJson(bestScore);
            File.WriteAllText(Application.persistentDataPath + "/BestScoreData.json", json);
        }

    }

    void SavePlayerName()
    {
        PlayerName playerName = new PlayerName();
        playerName.PlayerText = MenuManager.playerName;
        string json = JsonUtility.ToJson(playerName);
        File.WriteAllText(Application.persistentDataPath + "/PlayerName.json",json);
    }

}
