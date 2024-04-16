using Project.Scripts.Fractures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectColl : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == this.transform.parent.gameObject || other.gameObject.GetComponent<FixedJoint>()) return;
		
		for(int i = 0; i < this.transform.parent.childCount - 1; i++)
        {
			
			GameObject g = this.transform.parent.GetChild(i).gameObject;
		
			if (gameObject.name != "Empty")
			{
				ChunkNode a = g.GetComponent<ChunkNode>();
				//Descomentar
				g.GetComponent<Rigidbody>().isKinematic = true;
				g.GetComponent<Rigidbody>().detectCollisions = true;
				g.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
				a.falll();
			}
			
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
