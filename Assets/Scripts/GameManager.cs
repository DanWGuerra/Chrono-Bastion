using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gameplay")]
    public float timeRemaining = 60f;
    public float timePerKill = 1f;
    public int points = 0;

    private float TimetoPowerUp;

    private int enemiesSlain;

    [Header("UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI pointsText;

    private float elapsedTime = 0f;
    private bool isGameOver = false;

    public EnemySpawner enemySpawner;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isGameOver) return;


        TimetoPowerUp += Time.deltaTime;

        if (TimetoPowerUp >= 25)
        {
            enemySpawner.IncreaseDifficulty();
        }
        elapsedTime += Time.deltaTime;
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver();
        }

        UpdateUI();
    }

    public void AddPoints()
    {
        points++;
        enemiesSlain++;
        timeRemaining += timePerKill;
        UpdateUI();
    }

    public bool SpendPoints(int cost)
    {
        if (points >= cost)
        {
            points -= cost;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void LoseTime(float amount)
    {
        timeRemaining -= amount;
        if (timeRemaining <= 0 && !isGameOver)
            GameOver();
    }

    void UpdateUI()
    {
        if (timeText != null)
            timeText.text = $"Time: {timeRemaining:F1}s";
        if (pointsText != null)
            pointsText.text = $"Points: {points}";
    }

    void GameOver()
    {
        isGameOver = true;

        // Save stats for next scene
        GameOverData.enemiesSlain = enemiesSlain;
        GameOverData.timeSurvived = elapsedTime;

        // Load the Game Over scene
        SceneManager.LoadScene("GameOver");
    }
}
