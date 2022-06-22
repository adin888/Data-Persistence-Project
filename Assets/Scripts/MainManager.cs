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

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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
        LoadNameAndScore();
        if (UIManager.Instance != null)
            bestScoreText.text = $"Best Score : {UIManager.Instance.playerName} : {UIManager.Instance.bestScore}";
        else
            bestScoreText.text = $"Best Score : Name : 0";
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
                if (UIManager.Instance.bestScoreNow > UIManager.Instance.bestScore)
                    UIManager.Instance.bestScore = UIManager.Instance.bestScoreNow;
                UIManager.Instance.bestScoreNow = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            if (UIManager.Instance.bestScoreNow > UIManager.Instance.bestScore)
                UIManager.Instance.bestScore = UIManager.Instance.bestScoreNow;
            UIManager.Instance.bestScoreNow = 0;
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        BestScoreCalculation(m_Points);
    }
    void BestScoreCalculation(int Point)
    {
        if (Point > UIManager.Instance.bestScore)
        {
            UIManager.Instance.bestScore = Point;
            UIManager.Instance.playerName = UIManager.Instance.playerNameNow;
            BestScoreDisplay(UIManager.Instance.bestScore);
            SaveNameAndScore();
        }
    }
    void BestScoreDisplay(int bestPoint)
    {
        if(UIManager.Instance!=null)
            bestScoreText.text = $"Best Score : {UIManager.Instance.playerNameNow} : {bestPoint}";
        else
            bestScoreText.text = $"Best Score : Name : {bestPoint}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int bestScore;
    }
    public void SaveNameAndScore()
    {
        SaveData data = new SaveData();
        data.playerName = UIManager.Instance.playerName;
        data.bestScore = UIManager.Instance.bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadNameAndScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            UIManager.Instance.playerName = data.playerName;
            UIManager.Instance.bestScore = data.bestScore;
        }
    }
}
