using UnityEngine;
using MetalSlugPE.Core;

namespace MetalSlugPE.Enemies
{
    public class EnemyDamage : MonoBehaviour
    {
        [SerializeField] private int danio = 1;
        [SerializeField] private float cooldownContacto = 1f;

        private float tiempoUltimoGolpe = -Mathf.Infinity;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(Etiquetas.Jugador)) return;
            if (Time.time < tiempoUltimoGolpe + cooldownContacto) return;

            IDamageable objetivo = collision.gameObject.GetComponent<IDamageable>()
                                ?? collision.gameObject.GetComponentInParent<IDamageable>();
            if (objetivo != null)
            {
                objetivo.RecibirDanio(danio);
                tiempoUltimoGolpe = Time.time;
            }
        }
    }
}
