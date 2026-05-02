using UnityEngine;

namespace MetalSlugPE.World
{
    public class CamaraControlador : MonoBehaviour
    {
        [SerializeField] private Transform objetivo;
        [SerializeField] private float suavizado = 5f;
        [SerializeField] private bool usarLimites;
        [SerializeField] private Vector2 limiteMin;
        [SerializeField] private Vector2 limiteMax;

        private Vector3 velocidadActual;

        private void LateUpdate()
        {
            if (objetivo == null) return;

            float x = objetivo.position.x;
            float y = objetivo.position.y;

            if (usarLimites)
            {
                x = Mathf.Clamp(x, limiteMin.x, limiteMax.x);
                y = Mathf.Clamp(y, limiteMin.y, limiteMax.y);
            }

            Vector3 destino = new Vector3(x, y, transform.position.z);
            transform.position = Vector3.SmoothDamp(
                transform.position, destino, ref velocidadActual, 1f / suavizado);
        }
    }
}
