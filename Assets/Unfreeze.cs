using Project.Scripts.Fractures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unfreeze : MonoBehaviour
{
     
	public void prueba()
	{
		//Que algo cambie el nombre
		GameObject.Find("Fracture").GetComponent<ChunkNode>().Unfreeze();
	}
}
