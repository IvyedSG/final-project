using UnityEngine;

namespace MetalSlugPE.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        public int saludMaxima = 1;
        private int saludActual;

        private void Start()
        {
            saludActual = saludMaxima;
        }

        public void RecibirDanio(int danio)
        {
            saludActual -= danio;
            if (saludActual <= 0)
            {
                Morir();
            }
        }

        private void Morir()
        {
            Destroy(gameObject);
        }
    }
}