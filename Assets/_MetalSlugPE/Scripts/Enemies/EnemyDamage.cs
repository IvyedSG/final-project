using UnityEngine;
using MetalSlugPE.Player;

namespace MetalSlugPE.Enemies
{
    public class EnemyDamage : MonoBehaviour
    {
        [SerializeField] private int danio = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            PlayerHealth saludJugador = collision.gameObject.GetComponent<PlayerHealth>();
            if (saludJugador == null)
            {
                saludJugador = collision.gameObject.GetComponentInParent<PlayerHealth>();
            }

            if (saludJugador != null)
            {
                saludJugador.RecibirDanio(danio);
            }
        }
    }
}
