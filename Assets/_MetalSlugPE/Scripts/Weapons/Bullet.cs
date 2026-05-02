using UnityEngine;
using MetalSlugPE.Core;

namespace MetalSlugPE.Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float velocidad = 20f;
        [SerializeField] private float tiempoVida = 2f;
        [SerializeField] private int danio = 1;

        private GameObject disparador;

        public float Velocidad => velocidad;

        public void Inicializar(GameObject nuevoDisparador)
        {
            disparador = nuevoDisparador;
        }

        private void Start()
        {
            Destroy(gameObject, tiempoVida);
        }

        private void OnTriggerEnter2D(Collider2D impacto)
        {
            if (impacto == null) return;
            if (disparador != null &&
                (impacto.gameObject == disparador || impacto.transform.IsChildOf(disparador.transform))) return;

            IDamageable objetivo = impacto.GetComponent<IDamageable>()
                                ?? impacto.GetComponentInParent<IDamageable>();
            objetivo?.RecibirDanio(danio);

            Destroy(gameObject);
        }
    }
}
