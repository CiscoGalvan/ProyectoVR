using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Weapon
{
    public class FireGun : MonoBehaviour
    {
        [SerializeField] private Transform barrelEnd;
        [SerializeField] private Transform dir;
        [SerializeField] private Transform usedBulleTransform;

        private Vector3 shootDir;
        public bool grabbed = false;
		[SerializeField]
		private GameObject prefab;

		[SerializeField]
		private GameObject usedBulletPrefab;
		[SerializeField] private float radius = 0.1f;
        [SerializeField] private float velocity = 1000f;
        [SerializeField] private float mass = .5f;

        [SerializeField] private float bulletNum = 0f;
        [SerializeField] private AudioSource audio;

        AudioClip sonidoNoAmmo;
        AudioClip sonidoSiAmmo;

		private void Start()
		{
            sonidoNoAmmo = Resources.Load<AudioClip>("Quad Out of Ammo (Fortnite Sound) - Sound Effect for editing (mp3cut.net)");
            sonidoSiAmmo = audio.clip;
		}
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


        public void updateGrabbed(bool newValue)
        {
            grabbed = newValue;
        }

		void Update()
        {
			shootDir = dir.position - barrelEnd.position;
            if (Input.GetMouseButtonDown(0) && grabbed)
            {
                if (bulletNum > 0)
                    FireBullet();
                else
                    CantShoot();
              
            }
            
        }

        private void CantShoot()
        {
            audio.clip = sonidoNoAmmo;
            audio.Play();
        }
        public void FireBullet()
        {
            if (bulletNum < 0)
            {

                CantShoot();
            }
            else
            {
                audio.clip = sonidoSiAmmo;
                var bullet = GameObject.Instantiate(prefab);
                bullet.transform.position = barrelEnd.position;
                bullet.transform.forward = shootDir;
                bullet.transform.localScale = Vector3.one * Radius;

                var rb = bullet.AddComponent<Rigidbody>();
                rb.velocity = bullet.transform.forward * Velocity;
                rb.mass = mass;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;


                var usedBullet = GameObject.Instantiate(usedBulletPrefab);
                usedBullet.transform.position = usedBulleTransform.position;
                usedBullet.transform.forward = shootDir;
                usedBullet.transform.localScale = Vector3.one * Radius;


                var rbUsed = usedBullet.AddComponent<Rigidbody>();
                rbUsed.velocity = usedBullet.transform.right * Velocity / 100;
                rbUsed.angularVelocity = usedBullet.transform.up * Random.Range(-100, 100) + usedBullet.transform.forward * Random.Range(-100, 100);
                rbUsed.mass = mass;
                rbUsed.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;


                audio.Play();
                bulletNum--;
                Destroy(bullet, 3);
            }
		
		}
    }
}