using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    public float jumpForce = 12f;
    
    [Header("Disparo")]
    public GameObject bulletPrefab; // Arrastra aquí el PREFAB de la bala
    public Transform firePoint;     // Arrastra aquí el objeto FirePoint
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;

    void Start() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        // Movimiento lateral
        float x = 0;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x = -1;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x = 1;
        moveInput = new Vector2(x, 0);

        // Salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // DISPARO (Tecla F o Clic Izquierdo)
        if (Keyboard.current.fKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            Shoot();
    }

    void Shoot()
    {
        // Creamos la bala en la posición y rotación del FirePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void FixedUpdate() => rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

    private void OnCollisionEnter2D(Collision2D c) { if (c.gameObject.CompareTag("Ground")) isGrounded = true; }
    private void OnCollisionExit2D(Collision2D c) { if (c.gameObject.CompareTag("Ground")) isGrounded = false; }
}