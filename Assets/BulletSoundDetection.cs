using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BulletSoundDetection : MonoBehaviour
{
    // Start is called before the first frame update
    bool soundPlayed = false;
    private AudioSource audio;
    void Start()
    {
        audio= GetComponent<AudioSource>();
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (!soundPlayed)
        {
			audio.Play();
            soundPlayed = true;
            Destroy(this, 30);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
