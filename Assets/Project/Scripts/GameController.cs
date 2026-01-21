using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // ============================================
    // RIFERIMENTI UI E PREFAB
    // ============================================
    public GameObject ballPrefab;
    public Text score1Text;
    public Text score2Text;
    public Text pauseText;
    public Text winnerText;
    // NUOVO: Riferimento all'immagine di pausa (o pannello)
    public GameObject pauseImage; 

    // ============================================
    // CONFIGURAZIONE DI GIOCO
    // ============================================
    public float scoreCoordinates = 2.75f;
    public int maxScore = 5;

    // ============================================
    // DATI PRIVATI
    // ============================================
    private Ball _currentBall;
    private int _score1 = 0;
    private int _score2 = 0;
    private bool _isPaused = false;
    private bool _gameOver = false;

    // ============================================
    // LIFECYCLE
    // ============================================
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        InitializeBall();
        UpdateScoreDisplay();
        
        // Inizializzazione stati UI
        if(pauseText != null) pauseText.gameObject.SetActive(false);
        if(winnerText != null) winnerText.gameObject.SetActive(false);
        // NUOVO: Nasconde l'immagine all'avvio
        if(pauseImage != null) pauseImage.SetActive(false); 
        
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_gameOver)
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        if (!_gameOver)
        {
            CheckBallPosition();
        }
    }

    // ============================================
    // LOGICA DI GIOCO
    // ============================================

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        
        // Gestione visibilità testo
        if(pauseText != null) pauseText.gameObject.SetActive(_isPaused);
        
        // NUOVO: Gestione visibilità immagine
        if(pauseImage != null) pauseImage.SetActive(_isPaused);
        
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void InitializeBall()
    {
        if (ballPrefab == null) return;

        GameObject ballInstance = Instantiate(ballPrefab, transform);
        _currentBall = ballInstance.GetComponent<Ball>();

        if (_currentBall != null)
        {
            _currentBall.transform.position = Vector3.zero;
            SetBallReferenceToPaddles();
        }
    }

    private void UpdateScoreDisplay()
    {
        if (score1Text != null) score1Text.text = _score1.ToString();
        if (score2Text != null) score2Text.text = _score2.ToString();
    }

    private void CheckBallPosition()
    {
        if (_currentBall == null) return;

        float ballX = _currentBall.transform.position.x;

        if (ballX > scoreCoordinates)
        {
            _score1++;
            ProcessGoal();
        }
        else if (ballX < -scoreCoordinates)
        {
            _score2++;
            ProcessGoal();
        }
    }

    private void ProcessGoal()
    {
        UpdateScoreDisplay();
        DestroyBall();
        
        if (!CheckWinner())
        {
            StartCoroutine(SpawnNewBallAfterDelay(1f));
        }
    }

    private bool CheckWinner()
    {
        if (_score1 >= maxScore)
        {
            DisplayWinner("PLAYER 1");
            return true;
        }
        if (_score2 >= maxScore)
        {
            DisplayWinner("PLAYER 2");
            return true;
        }
        return false;
    }

    private void DisplayWinner(string playerName)
    {
        _gameOver = true;
        Time.timeScale = 0f; 
        
        if(winnerText != null)
        {
            winnerText.gameObject.SetActive(true);
            winnerText.text = playerName + " WINS!";
        }
        
        StartCoroutine(RestartGameAfterDelay(3f));
    }

    private void DestroyBall()
    {
        if (_currentBall != null)
        {
            Destroy(_currentBall.gameObject);
            _currentBall = null;
        }
    }

    private IEnumerator SpawnNewBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!_gameOver) InitializeBall();
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetBallReferenceToPaddles()
    {
        Paddle[] paddles = FindObjectsOfType<Paddle>();
        foreach (Paddle paddle in paddles)
        {
            paddle.SetBallReference(_currentBall.transform, _currentBall.GetComponent<Rigidbody2D>());
        }
    }
}