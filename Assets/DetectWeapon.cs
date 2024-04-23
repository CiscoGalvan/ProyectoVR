using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DetectWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    XRRayInteractor interactor;
    void Start()
    {
        interactor= GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    public void changeForceGrab(bool newValue)
    {
        interactor.useForceGrab = newValue;
    }
    
}
