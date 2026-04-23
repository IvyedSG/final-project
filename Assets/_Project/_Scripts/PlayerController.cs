using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string GROUND_TAG = "Ground";

    [Header("Salud")]
    public int health = 3;
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
        // Por ahora, solo reiniciamos la escena
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

    void Start() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        HandleInput();
        HandleAiming();
        HandleJumping();
        HandleShooting();
    }

    private void HandleInput()
    {
        bool wPressed = Keyboard.current.wKey.isPressed;
        bool sPressed = Keyboard.current.sKey.isPressed;
        bool aPressed = Keyboard.current.aKey.isPressed;
        bool dPressed = Keyboard.current.dKey.isPressed;

        isCrouching = sPressed;

        float x = 0;
        if (aPressed || Keyboard.current.leftArrowKey.isPressed) x = -1;
        else if (dPressed || Keyboard.current.rightArrowKey.isPressed) x = 1;
        moveInput = new Vector2(x, 0);

        if (x > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (x < 0) transform.localScale = new Vector3(1, 1, 1);
    }

    private void HandleAiming()
    {
        bool wPressed = Keyboard.current.wKey.isPressed;
        bool sPressed = Keyboard.current.sKey.isPressed;
        bool aPressed = Keyboard.current.aKey.isPressed;
        bool dPressed = Keyboard.current.dKey.isPressed;

        aimDirection = Vector2.right;

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
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && !isCrouching)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleShooting()
    {
        if ((Keyboard.current.fKey.isPressed || Mouse.current.leftButton.isPressed) && Time.time >= nextFireTime)
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

    void FixedUpdate() => rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

    private void OnCollisionEnter2D(Collision2D c) { if (c.gameObject.CompareTag(GROUND_TAG)) isGrounded = true; }
    private void OnCollisionExit2D(Collision2D c) { if (c.gameObject.CompareTag(GROUND_TAG)) isGrounded = false; }
}