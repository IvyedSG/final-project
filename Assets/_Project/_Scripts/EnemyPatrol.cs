using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRange = 5f; // Distancia para verte
    public Transform groundCheck;
    public float rayDistance = 1f;
    
    private bool movingRight = true;
    private Rigidbody2D rb;
    private Transform player; // Referencia al jugador

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        // Buscamos al objeto con el Tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calculamos la distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            // MODO PERSECUCIÓN
            Perseguir();
        }
        else
        {
            // MODO PATRULLA
            Patrullar();
        }
    }

    void Patrullar()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, rayDistance);
        if (groundInfo.collider == false) Flip();
    }

    void Perseguir()
    {
        // Mirar hacia donde está el jugador
        if (player.position.x > transform.position.x && !movingRight) Flip();
        else if (player.position.x < transform.position.x && movingRight) Flip();

        // Moverse hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), chaseSpeed * Time.deltaTime);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}