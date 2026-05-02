using UnityEngine;

namespace MetalSlugPE.Core
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioSource fuenteSFX;
        [SerializeField] private AudioSource fuenteMusica;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void ReproducirSFX(AudioClip clip)
        {
            if (clip == null || fuenteSFX == null) return;
            fuenteSFX.PlayOneShot(clip);
        }

        public void ReproducirMusica(AudioClip clip, bool repetir = true)
        {
            if (clip == null || fuenteMusica == null) return;
            if (fuenteMusica.clip == clip && fuenteMusica.isPlaying) return;
            fuenteMusica.clip  = clip;
            fuenteMusica.loop  = repetir;
            fuenteMusica.Play();
        }

        public void DetenerMusica() => fuenteMusica?.Stop();
        public void PausarMusica()  => fuenteMusica?.Pause();
        public void ReanudarMusica() => fuenteMusica?.UnPause();

        public void SetVolumenSFX(float v)    { if (fuenteSFX    != null) fuenteSFX.volume    = v; }
        public void SetVolumenMusica(float v) { if (fuenteMusica != null) fuenteMusica.volume = v; }
    }
}
