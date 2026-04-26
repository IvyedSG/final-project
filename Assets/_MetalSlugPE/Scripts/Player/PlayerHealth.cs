using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MetalSlugPE.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Vidas")]
        [SerializeField] private int vidasMaximas = 3;
        [SerializeField] private float duracionInvulnerabilidad = 2f;

        private int vidasActuales;
        private bool esInvulnerable;
        private bool estaReapareciendo;

        public int VidasActuales => vidasActuales;
        public bool EsInvulnerable => esInvulnerable;

        private Rigidbody2D cuerpo;

        private void Awake()
        {
            cuerpo = GetComponent<Rigidbody2D>();
            vidasActuales = vidasMaximas;
            Debug.Log("Vidas iniciales: " + vidasActuales);
        }

        public void RecibirDanio(int danio)
        {
            if (danio <= 0) return;
            if (esInvulnerable || estaReapareciendo) return;

            vidasActuales -= 1;
            Debug.Log("Impacto recibido. Vidas restantes: " + vidasActuales);

            if (vidasActuales <= 0)
            {
                Morir();
                return;
            }

            Vector3 posicionMuerte = transform.position;
            StartCoroutine(ReaparecerEnPosicionMuerte(posicionMuerte));
        }

        private IEnumerator ReaparecerEnPosicionMuerte(Vector3 posicionMuerte)
        {
            estaReapareciendo = true;

            if (cuerpo != null)
            {
                cuerpo.linearVelocity = Vector2.zero;
                cuerpo.angularVelocity = 0f;
            }

            transform.position = posicionMuerte;
            Debug.Log("Respawn en posicion de muerte: " + posicionMuerte);

            yield return null;
            estaReapareciendo = false;

            yield return StartCoroutine(VentanaInvulnerabilidad());
        }

        private IEnumerator VentanaInvulnerabilidad()
        {
            esInvulnerable = true;
            Debug.Log("Inmunidad activada por " + duracionInvulnerabilidad + " segundos");

            yield return new WaitForSeconds(duracionInvulnerabilidad);

            esInvulnerable = false;
            Debug.Log("Inmunidad finalizada");
        }

        private void Morir()
        {
            Debug.Log("Sin vidas. Reiniciando escena...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}