using UnityEngine;
using UnityEngine.Tilemaps;
using MetalSlugPE.Core;

namespace MetalSlugPE.World
{
    public class InfiniteGroundTilemap : MonoBehaviour
    {
        [SerializeField] private Tilemap mapa;
        [SerializeField] private TileBase baldosaTerreno;
        [SerializeField] private Transform objetivo;
        [SerializeField] private int posYSuelo = -9;
        [SerializeField] private int mitadAnchoBaldosas = 60;

        private bool inicializado;
        private int minXGenerado;
        private int maxXGenerado;

        private void Awake()
        {
            if (mapa == null)
                mapa = GetComponent<Tilemap>();
        }

        private void Start()
        {
            if (objetivo == null)
            {
                GameObject jugador = GameObject.FindGameObjectWithTag(Etiquetas.Jugador);
                if (jugador != null)
                    objetivo = jugador.transform;
            }

            if (baldosaTerreno == null)
                baldosaTerreno = mapa.GetTile(new Vector3Int(0, posYSuelo, 0));

            if (mapa == null || baldosaTerreno == null)
            {
                enabled = false;
                return;
            }

            AsegurarSueloAlrededor(ObtenerCeldaXObjetivo());
        }

        private void Update()
        {
            AsegurarSueloAlrededor(ObtenerCeldaXObjetivo());
        }

        private int ObtenerCeldaXObjetivo()
        {
            if (objetivo == null) return 0;
            return mapa.WorldToCell(objetivo.position).x;
        }

        private void AsegurarSueloAlrededor(int centroX)
        {
            int minDeseado = centroX - mitadAnchoBaldosas;
            int maxDeseado = centroX + mitadAnchoBaldosas;

            if (!inicializado)
            {
                RellenarRango(minDeseado, maxDeseado);
                minXGenerado = minDeseado;
                maxXGenerado = maxDeseado;
                inicializado = true;
                return;
            }

            if (minDeseado < minXGenerado)
            {
                RellenarRango(minDeseado, minXGenerado - 1);
                minXGenerado = minDeseado;
            }

            if (maxDeseado > maxXGenerado)
            {
                RellenarRango(maxXGenerado + 1, maxDeseado);
                maxXGenerado = maxDeseado;
            }
        }

        private void RellenarRango(int minX, int maxX)
        {
            for (int x = minX; x <= maxX; x++)
                mapa.SetTile(new Vector3Int(x, posYSuelo, 0), baldosaTerreno);
        }
    }
}
