using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // [SerializeField] permette di modificare variabili private dall'Inspector di Unity.
    // Mantiene l'incapsulamento (private) ma offre flessibilità di design.
    [SerializeField] private float speed = 3f;
    
    // Riferimento al componente fisico di QUESTO paddle.
    private Rigidbody2D rb;

    // --- SEZIONE AI (INTELLIGENZA ARTIFICIALE) ---
    // Se true, il paddle si muove da solo. Se false, risponde ai tasti.
    [SerializeField] private bool isAI = false;
    
    // Quanto tempo aspetta l'AI prima di ricalcolare la mossa. 
    // Serve a renderla imperfetta/umana (0.1s = molto reattiva).
    [SerializeField] private float aiReactionTime = 0.1f;
    
    // Quanto guarda "nel futuro" l'AI basandosi sulla velocità della palla.
    [SerializeField] private float aiPredictionOffset = 0.5f;
    
    // Riferimenti alla palla (necessari all'AI per "vederla").
    private Transform _ballTransform;
    private Rigidbody2D _ballRb;
    
    // Timer interno per gestire il ritardo di reazione.
    private float _aiTimer = 0f;

    void Start()
    {
        // Recuperiamo il Rigidbody2D attaccato allo stesso GameObject di questo script.
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // BRANCHING: Qui decidiamo chi controlla il paddle.
        if (isAI)
        {
            MoveAI(); // Logica computer
        }
        else
        {
            // --- LOGICA GIOCATORE UMANO ---
            // Input.GetAxis restituisce un valore fluido tra -1 (Giù/S) e 1 (Su/W).
            // 0 significa nessun tasto premuto.
            float verticalMovement = Input.GetAxis("Vertical");
            
            // Impostiamo direttamente la velocità. 
            // X è 0 (il paddle non si muove orizzontalmente).
            // Y è input * velocità.
            rb.velocity = new Vector2(0, verticalMovement * speed);
        }
    }

    private void MoveAI()
    {
        // GUARDIA: Se non sappiamo dov'è la palla, non facciamo nulla per evitare errori (NullReferenceException).
        if (_ballTransform == null || _ballRb == null)
            return;

        // Incrementiamo il timer con il tempo passato dall'ultimo frame.
        _aiTimer += Time.deltaTime;

        // È passato abbastanza tempo per "pensare"?
        if (_aiTimer >= aiReactionTime)
        {
            _aiTimer = 0f; // Resetta il timer
            CalculateAIMovement(); // Esegui la decisione
        }
    }

    private void CalculateAIMovement()
    {
        // 1. Dove si trova la palla ORA.
        float ballY = _ballTransform.position.y;
        
        // 2. Dove sta andando la palla (velocità verticale).
        float ballVelocityY = _ballRb.velocity.y;
        
        // 3. PREDIZIONE: Dove sarà la palla tra un po'? 
        // Se la palla sale veloce, miriamo più in alto della sua posizione attuale.
        float predictedBallY = ballY + (ballVelocityY * aiPredictionOffset);
        
        // 4. Dove sono IO (il paddle).
        float paddleY = transform.position.y;
        
        // 5. Zona morta: se la palla è vicina al centro del paddle (entro 0.3 unità), non muoverti.
        // Questo evita che l'AI "tremi" avanti e indietro cercando la perfezione millimetrica.
        float tolerance = 0.3f;

        float verticalMovement = 0f;

        // Se la palla predetta è più in alto del paddle + tolleranza... sali (1).
        if (predictedBallY > paddleY + tolerance)
            verticalMovement = 1f;
        // Se è più in basso... scendi (-1).
        else if (predictedBallY < paddleY - tolerance)
            verticalMovement = -1f;

        // Applica il movimento calcolato.
        rb.velocity = new Vector2(0, verticalMovement * speed);
    }

    // Metodo pubblico chiamato dall'esterno (es. dal GameManger) per dire all'AI qual è la palla.
    public void SetBallReference(Transform ballTransform, Rigidbody2D ballRb)
    {
        _ballTransform = ballTransform;
        _ballRb = ballRb;
    }
}