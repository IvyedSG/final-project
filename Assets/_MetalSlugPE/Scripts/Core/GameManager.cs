using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MetalSlugPE.Core
{
    public enum EstadoJuego { Jugando, Pausa, GameOver }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static event Action OnGameOver;
        public static event Action OnPausa;
        public static event Action OnReanudar;

        private EstadoJuego estadoActual = EstadoJuego.Jugando;

        public EstadoJuego EstadoActual => estadoActual;
        public bool EstaJugando => estadoActual == EstadoJuego.Jugando;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (estadoActual == EstadoJuego.Jugando) Pausar();
                else if (estadoActual == EstadoJuego.Pausa) Reanudar();
            }
        }

        public void GameOver()
        {
            if (estadoActual == EstadoJuego.GameOver) return;
            estadoActual = EstadoJuego.GameOver;
            Time.timeScale = 0f;
            OnGameOver?.Invoke();
        }

        public void Pausar()
        {
            if (estadoActual != EstadoJuego.Jugando) return;
            estadoActual = EstadoJuego.Pausa;
            Time.timeScale = 0f;
            OnPausa?.Invoke();
        }

        public void Reanudar()
        {
            if (estadoActual != EstadoJuego.Pausa) return;
            estadoActual = EstadoJuego.Jugando;
            Time.timeScale = 1f;
            OnReanudar?.Invoke();
        }

        public void ReiniciarNivel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
