using System;
using UnityEngine;
using UnityEngine.UI;
using MetalSlugPE.Player;

namespace MetalSlugPE.UI
{
    public class HUDVidas : MonoBehaviour
    {
        [SerializeField] private Text textoVidas;

        private void OnEnable()  => PlayerHealth.OnVidasCambiadas += ActualizarHUD;
        private void OnDisable() => PlayerHealth.OnVidasCambiadas -= ActualizarHUD;

        private void ActualizarHUD(int actuales, int maximas)
        {
            if (textoVidas == null) return;
            textoVidas.text = "Vidas: " + Mathf.Max(actuales, 0) + " / " + maximas;
        }
    }
}
