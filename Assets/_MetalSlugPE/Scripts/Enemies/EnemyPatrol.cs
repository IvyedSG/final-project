using UnityEngine;
using MetalSlugPE.Weapons;

namespace MetalSlugPE.Enemies
{
    public class EnemyPatrol : MonoBehaviour
    {
        public float velocidadPatrulla = 3f;
        public float velocidadPersecucion = 5f;
        public float rangoDeteccion = 5f;
        public Transform verificadorSuelo;
        public float distanciaRayo = 1f;

        private bool moviendoDerecha = true;
        private Rigidbody2D cuerpo;
        private Transform jugador;

        [Header("Disparo")]
        public GameObject prefabBala;
        public Transform puntoDisparo;
        public float cadenciaDisparo = 1f;
        public float rangoDisparo = 7f;
        private float TiempoDisparo = 0f;

        private void Start()
        {
            cuerpo = GetComponent<Rigidbody2D>();
            GameObject objetoJugador = GameObject.FindGameObjectWithTag("Player");
            if (objetoJugador != null)
            {
                jugador = objetoJugador.transform;
            }
        }

        private void Update()
        {
            if (jugador == null)
            {
                Patrullar();
                return;
            }

            float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

            if (distanciaAlJugador < rangoDeteccion)
            {
                Perseguir();
            }
            else
            {
                Patrullar();
            }

            if (distanciaAlJugador < rangoDisparo && Time.time > TiempoDisparo)
            {
                Disparar();
                TiempoDisparo = Time.time + cadenciaDisparo;
            }
        }

        private void Patrullar()
        {
            transform.Translate(Vector2.right * velocidadPatrulla * Time.deltaTime);
            RaycastHit2D infoSuelo = Physics2D.Raycast(verificadorSuelo.position, Vector2.down, distanciaRayo);
            if (infoSuelo.collider == false) Voltear();
        }

        private void Perseguir()
        {
            if (jugador.position.x > transform.position.x && !moviendoDerecha) Voltear();
            else if (jugador.position.x < transform.position.x && moviendoDerecha) Voltear();

            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(jugador.position.x, transform.position.y),
                velocidadPersecucion * Time.deltaTime
            );

            if (puntoDisparo != null && jugador != null)
            {
                Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;
                float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                puntoDisparo.rotation = Quaternion.Euler(0f, 0f, angulo);
            }

            if (Time.time > TiempoDisparo)
            {
                TiempoDisparo = Time.time + cadenciaDisparo;

                if (prefabBala == null || puntoDisparo == null) return;

                GameObject balaInstanciada = Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
                Bullet bala = balaInstanciada.GetComponent<Bullet>();
                Rigidbody2D cuerpoBala = balaInstanciada.GetComponent<Rigidbody2D>();

                if (bala != null)
                {
                    bala.disparador = gameObject;
                }

                if (cuerpoBala != null && bala != null)
                {
                    cuerpoBala.linearVelocity = (jugador.position - puntoDisparo.position).normalized * bala.velocidad;
                }
            }
        }

        private void Voltear()
        {
            moviendoDerecha = !moviendoDerecha;
            transform.Rotate(0f, 180f, 0f);
        }

        private void Disparar()
        {
            if (prefabBala == null || puntoDisparo == null || jugador == null) return;

            Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;
            GameObject balaInstanciada = Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);

            Bullet bala = balaInstanciada.GetComponent<Bullet>();
            Rigidbody2D cuerpoBala = balaInstanciada.GetComponent<Rigidbody2D>();

            if (bala != null)
            {
                bala.disparador = gameObject;
            }

            if (cuerpoBala != null && bala != null)
            {
                cuerpoBala.linearVelocity = direccion * bala.velocidad;
            }

            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            balaInstanciada.transform.rotation = Quaternion.Euler(0f, 0f, angulo);
        }
    }
}
