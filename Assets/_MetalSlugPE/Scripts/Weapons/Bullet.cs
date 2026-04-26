using UnityEngine;
using MetalSlugPE.Enemies;
using MetalSlugPE.Player;

namespace MetalSlugPE.Weapons
{
    public class Bullet : MonoBehaviour
    {
        private const string ETIQUETA_JUGADOR = "Player";

        public float velocidad = 20f;
        public float tiempoVida = 2f;
        public GameObject disparador;

        private void Start()
        {
            Destroy(gameObject, tiempoVida);
        }

        private void OnTriggerEnter2D(Collider2D impacto)
        {
            if (impacto == null) return;

            if (disparador != null)
            {
                if (impacto.gameObject == disparador) return;
                if (impacto.transform.IsChildOf(disparador.transform)) return;
            }

            if (impacto.CompareTag(ETIQUETA_JUGADOR))
            {
                PlayerHealth saludJugador = impacto.GetComponent<PlayerHealth>();
                if (saludJugador == null)
                {
                    saludJugador = impacto.GetComponentInParent<PlayerHealth>();
                }

                if (saludJugador != null)
                {
                    saludJugador.RecibirDanio(1);
                }

                Destroy(gameObject);
                return;
            }

            EnemyHealth saludEnemigo = impacto.GetComponent<EnemyHealth>();
            if (saludEnemigo == null)
            {
                saludEnemigo = impacto.GetComponentInParent<EnemyHealth>();
            }

            if (saludEnemigo != null)
            {
                saludEnemigo.RecibirDanio(1);
            }

            Destroy(gameObject);
        }
    }
}
