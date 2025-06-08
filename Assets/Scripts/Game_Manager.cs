using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    continues,
    paused,
    ended,
}
public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public GameState state = GameState.continues;

    [Header("Game Score")]
    public int score = 1;
    public int currentStreak = 0;
    public int StreakBonusMultiplier = 2;
    public int maxStreakBonus = 10;
    public Transform PauseWindow;
    public Transform EndGameWindow;
    public CameraControl cameraScript;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currentStreakText;

    public TextMeshProUGUI AfterGameScoreText;
    public TextMeshProUGUI AfterGameBestScoreText;

    private int bestScore;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayerData savedData = SaveSystem.LoadPlayer();
        if (savedData == null) return;

        bestScore = savedData.record;
        AfterGameBestScoreText.text = "Best Score : " + bestScore.ToString();
    }

    void Update()
    {
        if (state == GameState.ended) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchGameState();
        }
    }

    public void EndGame()
    {
        state = GameState.ended;
        cameraScript.focusOnTower = true;
        EndGameWindow.gameObject.SetActive(true);
        if (score > bestScore)
        {
            AfterGameBestScoreText.text = "Best Score : " + score.ToString();
            SaveSystem.SavePlayer(score);
        }
        AfterGameScoreText.text = "Score : " + score.ToString();

        scoreText.gameObject.SetActive(false);
        currentStreakText.gameObject.SetActive(false);
    }

    public void CalculateScores(bool isPerfectlyPlaced)
    {
        if (isPerfectlyPlaced)
        {
            currentStreak += 1;
            if (currentStreak >= maxStreakBonus) currentStreak = maxStreakBonus;
            score += currentStreak * StreakBonusMultiplier;
        }
        else
        {
            currentStreak = 0;
            score += 1;
        }

        scoreText.text = "Score : " + score.ToString();
        currentStreakText.text = "Current Streak : " + currentStreak.ToString();
    }

    public void SwitchGameState()
    {
        bool _isPaused = state == GameState.paused;
        if (_isPaused) state = GameState.continues;
        else state = GameState.paused;
        _isPaused = !_isPaused;
        PauseWindow.gameObject.SetActive(_isPaused);
        cameraScript.focusOnTower = _isPaused;
    }

    public void PauseGame()
    {
        state = GameState.paused;
        PauseWindow.gameObject.SetActive(true);
        cameraScript.focusOnTower = true;
    }

    public void ResumeGame()
    {
        state = GameState.continues;
        PauseWindow.gameObject.SetActive(false);
        cameraScript.focusOnTower = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
