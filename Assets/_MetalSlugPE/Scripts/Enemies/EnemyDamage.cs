using UnityEngine;
using MetalSlugPE.Player;

namespace MetalSlugPE.Enemies
{
    public class EnemyDamage : MonoBehaviour
    {
        public int damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }
}
