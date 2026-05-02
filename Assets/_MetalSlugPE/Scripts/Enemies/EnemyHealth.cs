using UnityEngine;
using MetalSlugPE.Core;

namespace MetalSlugPE.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private int saludMaxima = 1;

        private int saludActual;

        private void Start()
        {
            saludActual = saludMaxima;
        }

        public void RecibirDanio(int danio)
        {
            saludActual -= danio;
            if (saludActual <= 0)
                Destroy(gameObject);
        }
    }
}
