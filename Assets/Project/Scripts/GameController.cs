using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necessario per ricaricare la scena

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
        // Blocca il cursore all'interno della finestra di gioco
        Cursor.lockState = CursorLockMode.Locked;

        InitializeBall();
        UpdateScoreDisplay();
        
        // Assicuriamoci che i testi siano spenti all'avvio
        if(pauseText != null) pauseText.gameObject.SetActive(false);
        if(winnerText != null) winnerText.gameObject.SetActive(false);
        
        // Assicuriamoci che il tempo scorra normalmente
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Gestione Pausa
        if (Input.GetKeyDown(KeyCode.Tab) && !_gameOver)
        {
            TogglePause();
        }

        // Gestione Uscita
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        // Se il gioco non è finito, controlla la posizione della palla
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
        pauseText.gameObject.SetActive(_isPaused);
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
        Time.timeScale = 0f; // Ferma il gioco
        
        winnerText.gameObject.SetActive(true);
        winnerText.text = playerName + " WINS!";
        
        // Avvia il riavvio ignorando il Time.timeScale a 0
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

    // ============================================
    // COROUTINES (GESTIONE TEMPO)
    // ============================================

    private IEnumerator SpawnNewBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!_gameOver) InitializeBall();
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        // Utilizziamo Realtime perché il Time.timeScale è a 0
        yield return new WaitForSecondsRealtime(delay);
        
        // Ripristiniamo il tempo prima di caricare
        Time.timeScale = 1f;
        
        // Ricarica la scena corrente
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