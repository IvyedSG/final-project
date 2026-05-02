using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using MetalSlugPE.Core;

namespace MetalSlugPE.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        public static event Action<int, int> OnVidasCambiadas;

        [Header("Vidas")]
        [SerializeField] private int vidasMaximas = 3;
        [SerializeField] private float duracionInvulnerabilidad = 2f;

        private int vidasActuales;
        private bool esInvulnerable;
        private bool estaReapareciendo;
        private Rigidbody2D cuerpo;

        public int VidasActuales => vidasActuales;
        public bool EsInvulnerable => esInvulnerable;

        private void Awake()
        {
            cuerpo = GetComponent<Rigidbody2D>();
            vidasActuales = vidasMaximas;
        }

        private void Start()
        {
            OnVidasCambiadas?.Invoke(vidasActuales, vidasMaximas);
        }

        public void RecibirDanio(int danio)
        {
            if (danio <= 0) return;
            if (esInvulnerable || estaReapareciendo) return;

            vidasActuales -= danio;
            OnVidasCambiadas?.Invoke(vidasActuales, vidasMaximas);

            if (vidasActuales <= 0)
            {
                Morir();
                return;
            }

            StartCoroutine(ReaparecerEnPosicionMuerte(transform.position));
        }

        private IEnumerator ReaparecerEnPosicionMuerte(Vector3 posicionMuerte)
        {
            estaReapareciendo = true;
            cuerpo.linearVelocity = Vector2.zero;
            cuerpo.angularVelocity = 0f;
            transform.position = posicionMuerte;

            yield return null;
            estaReapareciendo = false;

            yield return StartCoroutine(VentanaInvulnerabilidad());
        }

        private IEnumerator VentanaInvulnerabilidad()
        {
            esInvulnerable = true;
            yield return new WaitForSeconds(duracionInvulnerabilidad);
            esInvulnerable = false;
        }

        private void Morir()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
