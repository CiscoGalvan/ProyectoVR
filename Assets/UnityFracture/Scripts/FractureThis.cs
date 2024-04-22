using UnityEngine;
using Random = System.Random;
using Project.Scripts.Fractures;

namespace Project.Scripts.Fractures
{
    public class FractureThis : MonoBehaviour
    {
        [SerializeField] public Anchor anchor = Anchor.Bottom;
        [SerializeField] public int chunks = 500;
        [SerializeField] public float density = 50;
        [SerializeField] public float internalStrength = 100;
            
        [SerializeField] public Material insideMaterial;
        [SerializeField] public Material outsideMaterial;

        private Random rng = new Random();

        private void Start()
        {
            FractureGameobject();
            gameObject.SetActive(false);
        }

        public ChunkGraphManager FractureGameobject()
        {
            var seed = rng.Next();

			return Fracture.FractureGameObject(
                gameObject,
                anchor,
                seed,
                chunks,
                insideMaterial,
                outsideMaterial,
                internalStrength,
                density
            );
        }

       
    }
}