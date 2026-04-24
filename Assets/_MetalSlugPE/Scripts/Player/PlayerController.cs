using UnityEngine;
using UnityEngine.InputSystem;
using MetalSlugPE.Weapons;

namespace MetalSlugPE.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const string GROUND_TAG = "Ground";

        [Header("Salud")]
        public int health = 1;
        public float knockbackForce = 5f;

        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("Vida restante: " + health);

            if (health <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("¡MORISTE!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        [Header("Movimiento")]
        public float speed = 8f;
        public float jumpForce = 18f;

        [Header("Disparo")]
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 0.15f;
        private float nextFireTime = 0f;

        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isGrounded;
        private bool isCrouching;
        private Vector2 aimDirection = Vector2.right;
        private Keyboard keyboard;
        private Mouse mouse;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            keyboard = Keyboard.current;
            mouse = Mouse.current;
        }

        void OnEnable()
        {
            keyboard = Keyboard.current;
            mouse = Mouse.current;
        }

        void Update()
        {
            HandleInput();
            HandleAiming();
            HandleJumping();
            HandleShooting();
        }

        private void HandleInput()
        {
            if (keyboard == null)
            {
                moveInput = Vector2.zero;
                isCrouching = false;
                return;
            }

            bool wPressed = keyboard.wKey.isPressed;
            bool sPressed = keyboard.sKey.isPressed;
            bool aPressed = keyboard.aKey.isPressed;
            bool dPressed = keyboard.dKey.isPressed;

            isCrouching = sPressed;

            float x = 0;
            if (aPressed || keyboard.leftArrowKey.isPressed) x = -1;
            else if (dPressed || keyboard.rightArrowKey.isPressed) x = 1;
            moveInput = new Vector2(x, 0);

            if (x > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (x < 0) transform.localScale = new Vector3(-1, 1, 1);
        }

        private void HandleAiming()
        {
            if (keyboard == null)
            {
                return;
            }

            bool wPressed = keyboard.wKey.isPressed;
            bool sPressed = keyboard.sKey.isPressed;
            bool aPressed = keyboard.aKey.isPressed;
            bool dPressed = keyboard.dKey.isPressed;

            aimDirection = transform.localScale.x >= 0f ? Vector2.right : Vector2.left;

            Vector2 aimInput = Vector2.zero;
            if (wPressed) aimInput.y = 1;
            if (sPressed) aimInput.y = -1;
            if (aPressed) aimInput.x = -1;
            if (dPressed) aimInput.x = 1;

            if (aimInput != Vector2.zero)
            {
                aimDirection = aimInput.normalized;
            }

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            if (isCrouching && !aPressed && !dPressed)
                angle = Mathf.Clamp(angle, -75f, 75f);

            if (aimDirection.x < 0) firePoint.rotation = Quaternion.Euler(0, 180, angle);
            else firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void HandleJumping()
        {
            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame && isGrounded && !isCrouching)
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void HandleShooting()
        {
            bool keyboardShoot = keyboard != null && keyboard.fKey.isPressed;
            bool mouseShoot = mouse != null && mouse.leftButton.isPressed;

            if ((keyboardShoot || mouseShoot) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }

        void Shoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = aimDirection * bullet.GetComponent<Bullet>().speed;
        }

        void FixedUpdate()
        {
            float horizontalVelocity = moveInput.x * speed;
            if (Mathf.Abs(horizontalVelocity) < 0.001f)
            {
                horizontalVelocity = 0f;
            }

            rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
        }

        private void OnCollisionEnter2D(Collision2D c) { if (c.gameObject.CompareTag(GROUND_TAG)) isGrounded = true; }
        private void OnCollisionExit2D(Collision2D c) { if (c.gameObject.CompareTag(GROUND_TAG)) isGrounded = false; }
    }
}
