using Project.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CombineColliders : MonoBehaviour
{
	MeshCollider parentCollider;
	void Start()
	{
		// Desactiva el MeshCollider del objeto padre
		parentCollider = GetComponent<MeshCollider>();
		parentCollider.enabled = false;

		// Combinar los MeshColliders de los hijos
		CombineInstance[] combine = new CombineInstance[transform.childCount];
		int i = 0;
		foreach (Transform child in transform)
		{
			MeshFilter childMeshFilter = child.GetComponent<MeshFilter>();
			if (childMeshFilter != null)
			{
				combine[i].mesh = childMeshFilter.mesh;
				combine[i].transform = childMeshFilter.transform.localToWorldMatrix;
				i++;
			}
		}

		// Crear un nuevo Mesh combinando los Mesh de los hijos
		Mesh combinedMesh = new Mesh();
		combinedMesh.CombineMeshes(combine, true);
		

		// Asignar el nuevo Mesh al MeshCollider del objeto padre
		parentCollider.sharedMesh = combinedMesh;
		parentCollider.enabled = true;
		parentCollider.transform.position = transform.position;
	
	}

	
}
