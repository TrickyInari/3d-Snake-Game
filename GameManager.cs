using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // For TextMeshPro support

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startScreen;           // Assign start screen panel here
    public GameObject gameplayUI;            // Assign gameplay UI panel here (score, etc)
    public GameObject gameOverScreen;        // Assign game over screen panel here

    [Header("Game Objects")]
    public GameObject snake;                 // Assign snake GameObject here

    [Header("Game Over UI")]
    public TMP_Text gameOverScoreText;      // Text to display final score on game over screen

    private bool gameStarted = false;
    private bool isGameOver = false;
    private SnakeController snakeController;

    void Start()
    {
        // Pause game at start
        Time.timeScale = 0f;

        // Show start screen, hide other UI
        SetActiveUI(start: true, gameplay: false, gameOver: false);

        // Hide snake at start
        if (snake != null)
            snake.SetActive(false);

        // Cache snake controller for score access
        if (snake != null)
            snakeController = snake.GetComponent<SnakeController>();
    }

    void Update()
    {
        if (!gameStarted)
        {
            // Wait for any key to start the game
            if (Input.anyKeyDown)
            {
                StartGame();
            }
        }
        else if (isGameOver)
        {
            // Wait for any key to restart the game after game over
            if (Input.anyKeyDown)
            {
                RestartGame();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        isGameOver = false;

        // Show gameplay UI, hide start/game over screens
        SetActiveUI(start: false, gameplay: true, gameOver: false);

        if (snake != null)
            snake.SetActive(true);

        // Unpause the game
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        isGameOver = true;

        // Pause the game
        Time.timeScale = 0f;

        // Show game over screen, hide gameplay and start screens
        SetActiveUI(start: false, gameplay: false, gameOver: true);

        // Display final score if snakeController exists
        if (gameOverScoreText != null && snakeController != null)
        {
            gameOverScoreText.text = $"Game Over!\nFinal Score: {snakeController.GetScore()}\nPress Any Key to Restart";
        }
    }

    void RestartGame()
    {
        // Reload current scene to restart game
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Utility method to manage UI visibility consistently
    void SetActiveUI(bool start, bool gameplay, bool gameOver)
    {
        if (startScreen != null)
            startScreen.SetActive(start);

        if (gameplayUI != null)
            gameplayUI.SetActive(gameplay);

        if (gameOverScreen != null)
            gameOverScreen.SetActive(gameOver);
    }
}