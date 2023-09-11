using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIVE.FacialTracking;
using VIVE;

public class StartAndStopFacialFramework : MonoBehaviour
{

    private FacialManager facialmanager;
    // Start is called before the first frame update
    void Start()
    {
        // Start the eye tracking detection
        facialmanager = new FacialManager();
        facialmanager.StartFramework(XrFacialTrackingTypeHTC.XR_FACIAL_TRACKING_TYPE_EYE_DEFAULT_HTC);
    }

    private void OnDestroy()
    {
        // Stop the eye tracking detection
        facialmanager.StopFramework(XrFacialTrackingTypeHTC.XR_FACIAL_TRACKING_TYPE_EYE_DEFAULT_HTC);
    }
}
