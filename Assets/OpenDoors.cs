using Project.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
	[SerializeField]public GameObject leftDoor; 
	[SerializeField]public GameObject rightDoor;



	bool openDoors = false;
	bool closeDoors = false;


	private float fixedTime;
	public float segundos = 2.5f;

	void Update()
	{
		if (openDoors)
		{
			fixedTime += Time.deltaTime;
			if(fixedTime >= segundos)
			{
				openDoors = false; fixedTime = 0;
			}
			leftDoor.transform.position = new Vector3(leftDoor.transform.position.x, leftDoor.transform.position.y, leftDoor.transform.position.z - Time.deltaTime);
			rightDoor.transform.position = new Vector3(rightDoor.transform.position.x, rightDoor.transform.position.y, rightDoor.transform.position.z + Time.deltaTime);
		}

		if (closeDoors)
		{
			fixedTime += Time.deltaTime;
			if (fixedTime >= segundos)
			{
				closeDoors = false; fixedTime = 0;
			}
			leftDoor.transform.position = new Vector3(leftDoor.transform.position.x, leftDoor.transform.position.y, leftDoor.transform.position.z + Time.deltaTime);
			rightDoor.transform.position = new Vector3(rightDoor.transform.position.x, rightDoor.transform.position.y, rightDoor.transform.position.z - Time.deltaTime);
		}
			
		
	}
	private void OnTriggerEnter(Collider other)
	{
		
		if (other.GetComponent<XROrigin>())
		{
			openDoors = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<XROrigin>())
		{
			closeDoors = true;
		}
	}

}
