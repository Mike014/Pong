using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // ============================================
    // RIFERIMENTI UI E PREFAB
    // ============================================
    public GameObject ballPrefab;
    public Text score1Text;
    public Text score2Text;

    // ============================================
    // CONFIGURAZIONE DI GIOCO
    // ============================================
    public float scoreCoordinates = 2.75f;

    // ============================================
    // DATI PRIVATI
    // ============================================
    private Ball _currentBall;
    private int _score1 = 0;
    private int _score2 = 0;

    // ============================================
    // LIFECYCLE
    // ============================================
    void Start()
    {
        InitializeBall();
        UpdateScoreDisplay();
    }

    void Update()
    {
        CheckBallPosition();
    }

    // ============================================
    // METODI PRIVATI
    // ============================================

    /// <summary>
    /// Inizializza la palla all'inizio della partita.
    /// </summary>
    private void InitializeBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("❌ Ball Prefab non assegnato nell'Inspector!");
            return;
        }

        GameObject ballInstance = Instantiate(ballPrefab, transform);
        _currentBall = ballInstance.GetComponent<Ball>();

        if (_currentBall == null)
        {
            Debug.LogError("❌ Ball Prefab non ha il componente Ball script!");
            return;
        }

        _currentBall.transform.position = Vector3.zero;
        Debug.Log("✅ Palla inizializzata a posizione (0, 0, 0)");

        SetBallReferenceToPaddles();
    }

    /// <summary>
    /// Aggiorna la visualizzazione dei punteggi nella UI.
    /// </summary>
    private void UpdateScoreDisplay()
    {
        if (score1Text != null)
            score1Text.text = _score1.ToString();

        if (score2Text != null)
            score2Text.text = _score2.ToString();
    }

    /// <summary>
    /// Controlla la posizione della palla e aggiorna i punteggi.
    /// </summary>
    private void CheckBallPosition()
    {
        if (_currentBall == null)
            return;

        float ballX = _currentBall.transform.position.x;

        // Giocatore 1 segna (palla a destra)
        if (ballX > scoreCoordinates)
        {
            _score1++;
            UpdateScoreDisplay();
            DestroyBall();
            Debug.Log($"🎉 Player 1 segna! Score: {_score1} - {_score2}");
        }

        // Giocatore 2 segna (palla a sinistra)
        if (ballX < -scoreCoordinates)
        {
            _score2++;
            UpdateScoreDisplay();
            DestroyBall();
            Debug.Log($"🎉 Player 2 segna! Score: {_score1} - {_score2}");
        }
    }

    /// <summary>
    /// Distrugge la palla attuale dopo un goal.
    /// </summary>
    private void DestroyBall()
    {
        if (_currentBall != null)
        {
            Debug.Log("🗑️ Palla distrutta");
            Destroy(_currentBall.gameObject);
            _currentBall = null;
            
            // ✅ Crea una nuova palla dopo una piccola pausa
            StartCoroutine(SpawnNewBallAfterDelay(1f));
        }
    }

    /// <summary>
    /// Crea una nuova palla dopo un delay (coroutine).
    /// </summary>
    /// <param name="delay">Tempo di attesa in secondi prima di creare la nuova palla</param>
    private IEnumerator SpawnNewBallAfterDelay(float delay)
    {
        Debug.Log($"⏳ Attendendo {delay} secondi prima di creare una nuova palla...");
        yield return new WaitForSeconds(delay);
        InitializeBall();
        Debug.Log("✅ Nuova palla creata!");
    }

    /// <summary>
    /// Resetta la posizione della palla (usato solo se riutilizzi la stessa palla).
    /// </summary>
    private void ResetBall()
    {
        if (_currentBall != null)
        {
            _currentBall.transform.position = Vector3.zero;
            // Opzionale: resetta anche la velocità
            Rigidbody2D rb = _currentBall.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(-0.5f, 1f);
            }
        }
    }

    private void SetBallReferenceToPaddles()
    {
        Paddle[] paddles = FindObjectsOfType<Paddle>();
        
        foreach (Paddle paddle in paddles)
        {
            paddle.SetBallReference(_currentBall.transform, _currentBall.GetComponent<Rigidbody2D>());
        }
        
        Debug.Log("✅ Riferimento palla assegnato ai paddle");
    }
}