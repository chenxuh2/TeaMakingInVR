using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

public class GenerateHapticFeedbackOnCollision : MonoBehaviour
{
    private bool isGrabbed = false;
    private ActionToGrabbing grabbingHand = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(isGrabbed)
        {
            grabbingHand.generateHapticFeedback();
        }
    }

    public void setGrabbed(ActionToGrabbing grabbingHandScript)
    {
        isGrabbed = true;
        grabbingHand = grabbingHandScript;
    }
}
