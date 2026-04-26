using UnityEngine;
using UnityEngine.InputSystem;
using MetalSlugPE.Weapons;

namespace MetalSlugPE.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const string ETIQUETA_SUELO = "Ground";

        [Header("Movimiento")]
        public float velocidad = 8f;
        public float fuerzaSalto = 18f;

        [Header("Disparo")]
        public GameObject prefabBala;
        public Transform puntoDisparo;
        public float cadenciaDisparo = 0.15f;
        private float TiempoDisparo = 0f;

        private Rigidbody2D cuerpo;
        private Vector2 entradaMovimiento;
        private bool estaEnSuelo;
        private bool agachado;
        private Vector2 direccionApunte = Vector2.right;
        private Keyboard teclado;
        private Mouse raton;

        private void Start()
        {
            cuerpo = GetComponent<Rigidbody2D>();
            teclado = Keyboard.current;
            raton = Mouse.current;
        }

        private void OnEnable()
        {
            teclado = Keyboard.current;
            raton = Mouse.current;
        }

        private void Update()
        {
            ProcesarEntrada();
            ProcesarApuntado();
            ProcesarSalto();
            ProcesarDisparo();
        }

        private void ProcesarEntrada()
        {
            if (teclado == null)
            {
                entradaMovimiento = Vector2.zero;
                agachado = false;
                return;
            }

            bool teclaW = teclado.wKey.isPressed;
            bool teclaS = teclado.sKey.isPressed;
            bool teclaA = teclado.aKey.isPressed;
            bool teclaD = teclado.dKey.isPressed;

            agachado = teclaS;

            float ejeX = 0f;
            if (teclaA || teclado.leftArrowKey.isPressed) ejeX = -1f;
            else if (teclaD || teclado.rightArrowKey.isPressed) ejeX = 1f;

            entradaMovimiento = new Vector2(ejeX, 0f);

            if (ejeX > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
            else if (ejeX < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        private void ProcesarApuntado()
        {
            if (teclado == null || puntoDisparo == null) return;

            bool teclaW = teclado.wKey.isPressed;
            bool teclaS = teclado.sKey.isPressed;
            bool teclaA = teclado.aKey.isPressed;
            bool teclaD = teclado.dKey.isPressed;

            direccionApunte = transform.localScale.x >= 0f ? Vector2.right : Vector2.left;

            Vector2 entradaApunte = Vector2.zero;
            if (teclaW) entradaApunte.y = 1f;
            if (teclaS) entradaApunte.y = -1f;
            if (teclaA) entradaApunte.x = -1f;
            if (teclaD) entradaApunte.x = 1f;

            if (entradaApunte != Vector2.zero)
            {
                direccionApunte = entradaApunte.normalized;
            }

            float angulo = Mathf.Atan2(direccionApunte.y, direccionApunte.x) * Mathf.Rad2Deg;

            if (agachado && !teclaA && !teclaD)
            {
                angulo = Mathf.Clamp(angulo, -75f, 75f);
            }

            if (direccionApunte.x < 0f) puntoDisparo.rotation = Quaternion.Euler(0f, 180f, angulo);
            else puntoDisparo.rotation = Quaternion.Euler(0f, 0f, angulo);
        }

        private void ProcesarSalto()
        {
            if (teclado != null && teclado.spaceKey.wasPressedThisFrame && estaEnSuelo && !agachado)
            {
                cuerpo.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            }
        }

        private void ProcesarDisparo()
        {
            bool disparaTeclado = teclado != null && teclado.fKey.isPressed;
            bool disparaRaton = raton != null && raton.leftButton.isPressed;

            if ((disparaTeclado || disparaRaton) && Time.time >= TiempoDisparo)
            {
                Disparar();
                    TiempoDisparo = Time.time + cadenciaDisparo;
            }
        }

        private void Disparar()
        {
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
                cuerpoBala.linearVelocity = direccionApunte * bala.velocidad;
            }
        }

        private void FixedUpdate()
        {
            float velocidadHorizontal = entradaMovimiento.x * velocidad;
            if (Mathf.Abs(velocidadHorizontal) < 0.001f) velocidadHorizontal = 0f;

            cuerpo.linearVelocity = new Vector2(velocidadHorizontal, cuerpo.linearVelocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(ETIQUETA_SUELO))
            {
                estaEnSuelo = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(ETIQUETA_SUELO))
            {
                estaEnSuelo = false;
            }
        }
    }
}
