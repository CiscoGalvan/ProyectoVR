using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Weapon
{
    public class FireGun : MonoBehaviour
    {
        [SerializeField] private Transform barrelEnd;
        [SerializeField] private Transform dir;

        private Vector3 shootDir;

		[SerializeField]
		private GameObject prefab;
		[SerializeField] private float radius = 0.1f;
        [SerializeField] private float velocity = 1000f;
        [SerializeField] private float mass = .5f;

        [SerializeField] private AudioSource audio;

        public float Radius
        {
            get => radius;
            set => radius = value;
        }

        public float Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

		void Update()
        {
			shootDir = dir.position - barrelEnd.position;
            if (Input.GetMouseButtonDown(0))
            {
                FireBullet();
            }
            
        }

        private void FireBullet()
        {
            
            var bullet = GameObject.Instantiate(prefab);
            bullet.transform.position = barrelEnd.position;
            bullet.transform.forward = shootDir;

			bullet.transform.localScale = Vector3.one * Radius;

           
            var rb = bullet.AddComponent<Rigidbody>();
            rb.velocity = bullet.transform.forward * Velocity;
            rb.mass = mass;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            audio.Play();
			Destroy(bullet, 3);
		}
    }
}