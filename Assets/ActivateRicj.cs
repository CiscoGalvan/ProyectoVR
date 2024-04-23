using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Video;

public class ActivateRicj : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private VideoPlayer video;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<XROrigin>())
		{
			video.Play();
			video.isLooping = true;
		}
	}
}
