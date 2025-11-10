using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    void Start()
    {
        int enemies = GameOverData.enemiesSlain;
        int seconds = Mathf.FloorToInt(GameOverData.timeSurvived);

        statsText.text = $"Enemies Slain: {enemies}\nTime Survived: {seconds} seconds";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGameScene"); // Replace with your main scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
    