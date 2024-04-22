using Project.Scripts.Fractures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unfreeze : MonoBehaviour
{
     
	public void prueba(string name)
	{
		//Que algo cambie el nombre
		name = name + "Object";
		GameObject g = GameObject.Find(name);

		
		var a = g.GetComponent<ChunkNode>();
		if (a != null)
		{
			a.Unfreeze();
		}
		else print(name);
	}
}
