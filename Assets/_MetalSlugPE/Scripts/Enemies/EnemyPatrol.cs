using UnityEngine;
using MetalSlugPE.Weapons;
namespace MetalSlugPE.Enemies
{
    public class EnemyPatrol : MonoBehaviour
    {
        public float speed = 3f;
        public float chaseSpeed = 5f;
        public float detectionRange = 5f;
        public Transform groundCheck;
        public float rayDistance = 1f;

        private bool movingRight = true;
        private Rigidbody2D rb;
        private Transform player;

        [Header("Disparo")]
        public GameObject bulletPrefab;  
        public Transform firePoint;      
        public float fireRate = 1f;
        public float shootingRange = 7f;  
        private float nextFireTime = 0f;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        void Update()
        {
            if (player == null)
            {
                Patrullar();
                return;
            }

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < detectionRange)
            {
                Perseguir();
            }
            else
            {
                Patrullar();
            }

            if (distanceToPlayer < shootingRange && Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }

        void Patrullar()
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, rayDistance);
            if (groundInfo.collider == false) Flip();
        }

        void Perseguir()
        {
            if (player.position.x > transform.position.x && !movingRight) Flip();
            else if (player.position.x < transform.position.x && movingRight) Flip();

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), chaseSpeed * Time.deltaTime);

            // Apuntar hacia el jugador
            if (firePoint != null && player != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bullet.GetComponent<Bullet>().shooter = gameObject;
                bullet.GetComponent<Rigidbody2D>().linearVelocity = (player.position - firePoint.position).normalized * bullet.GetComponent<Bullet>().speed;
            }
        }

        void Flip()
        {
            movingRight = !movingRight;
            transform.Rotate(0f, 180f, 0f);
        }

        void Shoot()
        {
            if (bulletPrefab != null && firePoint != null)
            {
                // Calcular dirección hacia el jugador
                Vector2 direction = (player.position - firePoint.position).normalized;
                
                // Instanciar la bala
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                
                // Asignar el disparador para evitar auto-daño
                bullet.GetComponent<Bullet>().shooter = gameObject;
                
                // Aplicar velocidad a la bala
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bullet.GetComponent<Bullet>().speed;
                
                // Rotar la bala para que apunte correctamente
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}
