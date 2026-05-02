using UnityEngine;
using MetalSlugPE.Core;

namespace MetalSlugPE.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject panelGameOver;
        [SerializeField] private GameObject panelPausa;

        private void OnEnable()
        {
            GameManager.OnGameOver += MostrarGameOver;
            GameManager.OnPausa    += MostrarPausa;
            GameManager.OnReanudar += OcultarPausa;
        }

        private void OnDisable()
        {
            GameManager.OnGameOver -= MostrarGameOver;
            GameManager.OnPausa    -= MostrarPausa;
            GameManager.OnReanudar -= OcultarPausa;
        }

        private void Start()
        {
            if (panelGameOver != null) panelGameOver.SetActive(false);
            if (panelPausa != null) panelPausa.SetActive(false);
        }

        public void MostrarGameOver() => panelGameOver?.SetActive(true);
        public void MostrarPausa()    => panelPausa?.SetActive(true);
        public void OcultarPausa()    => panelPausa?.SetActive(false);
    }
}
