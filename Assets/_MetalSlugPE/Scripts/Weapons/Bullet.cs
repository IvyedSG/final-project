using UnityEngine;

namespace MetalSlugPE.Weapons
{
    public class Bullet : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";

        public float speed = 20f;
        public float lifeTime = 2f;

        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D hitInfo)
        {
            if (!hitInfo.CompareTag(PLAYER_TAG))
            {
                Debug.Log("Impacto en: " + hitInfo.name);
                Destroy(gameObject);
            }
        }
    }
}
