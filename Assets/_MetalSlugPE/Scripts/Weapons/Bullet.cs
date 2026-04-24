using UnityEngine;
using MetalSlugPE.Enemies;
using MetalSlugPE.Player;

namespace MetalSlugPE.Weapons
{
    public class Bullet : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";

        public float speed = 20f;
        public float lifeTime = 2f;

        public GameObject shooter;

        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D hitInfo)
        {
            if (hitInfo.CompareTag(PLAYER_TAG))
            {
                // Daño al jugador
                PlayerController player = hitInfo.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(1);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Impacto en: " + hitInfo.name);
                // Solo dañar a enemigos (no al disparador)
                EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
                if (enemy != null && hitInfo.gameObject != shooter)
                {
                    enemy.TakeDamage(1);
                }
                Destroy(gameObject);
            }
        }
    }
}
