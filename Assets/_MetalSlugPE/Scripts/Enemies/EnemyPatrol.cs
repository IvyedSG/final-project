using UnityEngine;
using MetalSlugPE.Core;
using MetalSlugPE.Weapons;

namespace MetalSlugPE.Enemies
{
    public class EnemyPatrol : MonoBehaviour
    {
        [Header("Movimiento")]
        [SerializeField] private float velocidadPatrulla = 3f;
        [SerializeField] private float velocidadPersecucion = 5f;
        [SerializeField] private float rangoDeteccion = 5f;
        [SerializeField] private Transform verificadorSuelo;
        [SerializeField] private float distanciaRayo = 1f;

        [Header("Disparo")]
        [SerializeField] private GameObject prefabBala;
        [SerializeField] private Transform puntoDisparo;
        [SerializeField] private float cadenciaDisparo = 1f;
        [SerializeField] private float rangoDisparo = 7f;

        private bool moviendoDerecha = true;
        private float velocidadHorizontal;
        private float tiempoDisparo;
        private Rigidbody2D cuerpo;
        private Transform jugador;

        private void Start()
        {
            cuerpo = GetComponent<Rigidbody2D>();
            GameObject objetoJugador = GameObject.FindGameObjectWithTag(Etiquetas.Jugador);
            if (objetoJugador != null)
                jugador = objetoJugador.transform;
        }

        private void Update()
        {
            if (jugador == null)
            {
                velocidadHorizontal = moviendoDerecha ? velocidadPatrulla : -velocidadPatrulla;
                return;
            }

            float distancia = Vector2.Distance(transform.position, jugador.position);

            if (distancia < rangoDeteccion)
                Perseguir();
            else
                Patrullar();

            if (distancia < rangoDisparo && Time.time > tiempoDisparo)
            {
                Disparar();
                tiempoDisparo = Time.time + cadenciaDisparo;
            }
        }

        private void FixedUpdate()
        {
            cuerpo.linearVelocity = new Vector2(velocidadHorizontal, cuerpo.linearVelocity.y);
        }

        private void Patrullar()
        {
            velocidadHorizontal = moviendoDerecha ? velocidadPatrulla : -velocidadPatrulla;

            if (verificadorSuelo != null)
            {
                RaycastHit2D infoSuelo = Physics2D.Raycast(
                    verificadorSuelo.position, Vector2.down, distanciaRayo);
                if (!infoSuelo.collider) Voltear();
            }
        }

        private void Perseguir()
        {
            float dirX = jugador.position.x > transform.position.x ? 1f : -1f;
            velocidadHorizontal = dirX * velocidadPersecucion;

            if (jugador.position.x > transform.position.x && !moviendoDerecha) Voltear();
            else if (jugador.position.x < transform.position.x && moviendoDerecha) Voltear();

            if (puntoDisparo != null)
            {
                Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;
                float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                puntoDisparo.rotation = Quaternion.Euler(0f, 0f, angulo);
            }
        }

        private void Voltear()
        {
            moviendoDerecha = !moviendoDerecha;
            Vector3 escala = transform.localScale;
            escala.x *= -1f;
            transform.localScale = escala;
        }

        private void Disparar()
        {
            if (prefabBala == null || puntoDisparo == null || jugador == null) return;

            Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;
            GameObject balaObj     = Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
            Bullet bala            = balaObj.GetComponent<Bullet>();
            Rigidbody2D cuerpoBala = balaObj.GetComponent<Rigidbody2D>();

            bala?.Inicializar(gameObject);

            if (cuerpoBala != null && bala != null)
                cuerpoBala.linearVelocity = direccion * bala.Velocidad;
        }
    }
}
