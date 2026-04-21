using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Buscamos el componente PlayerController del objeto con el que chocamos
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}