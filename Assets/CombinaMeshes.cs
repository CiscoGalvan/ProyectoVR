using Project.Scripts.Fractures;
using UnityEngine;
using Project.Scripts.Fractures;
public class CombineMeshes : MonoBehaviour
{
	[SerializeField] private Anchor anchor = Anchor.Bottom;
	[SerializeField] private int chunks = 500;
	[SerializeField] private float density = 50;
	[SerializeField] private float internalStrength = 100;

	[SerializeField] private Material insideMaterial;
	[SerializeField] private Material outsideMaterial;
	
	
	[SerializeField] private float mass;
	

	void Start()
	{

		
		// Obtener todos los MeshFilters de los objetos hijos
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

		// Lista para almacenar los meshes combinados
		CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

		// Recorrer los MeshFilters y asignar sus meshes al arreglo de CombineInstance
		for (int i = 0; i < meshFilters.Length; i++)
		{
			combineInstances[i].mesh = meshFilters[i].sharedMesh;
			combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}

		// Crear un nuevo GameObject para contener el mesh combinado
		GameObject combinedObject = new GameObject("CombinedMesh");
		combinedObject.transform.SetParent(transform);

		// Agregar un MeshFilter al nuevo GameObject
		MeshFilter meshFilter = combinedObject.AddComponent<MeshFilter>();

		// Crear un nuevo Mesh que contendrá los meshes combinados
		Mesh combinedMesh = new Mesh();

		// Combinar los meshes utilizando la función CombineMeshes
		combinedMesh.CombineMeshes(combineInstances);

		// Asignar el nuevo Mesh al MeshFilter del nuevo GameObject
		meshFilter.sharedMesh = combinedMesh;


		var fractured = combinedObject.AddComponent<FractureThis>();
		fractured.anchor = anchor;
		fractured.chunks = chunks;
		fractured.density = density;
		fractured.internalStrength = internalStrength;
		fractured.insideMaterial = insideMaterial;
		fractured.outsideMaterial = outsideMaterial;

		var audioSource = combinedObject.AddComponent<AudioSource>();
		audioSource.clip = Resources.Load<AudioClip>("Button Pop");
		audioSource.spatialBlend = 1;

		var rigidBody = combinedObject.AddComponent<Rigidbody>();
		rigidBody.mass = mass;

		// Desactivar los MeshFilters de los objetos hijos para que no se rendericen individualmente
		foreach (MeshFilter filter in meshFilters)
		{
			filter.gameObject.SetActive(false);
		}
	}
}
