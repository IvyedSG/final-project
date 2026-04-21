using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f; // Segundos antes de destruirse sola

    void Start()
    {
        // Le damos velocidad hacia adelante (derecha) nada más nacer
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;

        // Se destruye sola para no llenar la memoria de balas infinitas
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Por ahora, que se destruya al tocar cualquier cosa
        if (!hitInfo.CompareTag("Player")) // Que no se choque con el jugador
        {
            Debug.Log("Impacto en: " + hitInfo.name);
            Destroy(gameObject);
        }
    }
}