using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VIVE.FacialTracking; // Added according to tutorial  
using System;
using System.Runtime.InteropServices;
using VIVE;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

public class EyeTrackingSample : MonoBehaviour
{
    //Map OpenXR eye shapes to avatar blend shapes
    private static Dictionary<XrEyeShapeHTC, SkinnedMeshRendererShape> ShapeMap;
    public SkinnedMeshRenderer HeadskinnedMeshRenderer;
    private FacialManager facialmanager;
    private Dictionary<XrEyeShapeHTC, float> EyeWeightings = new Dictionary<XrEyeShapeHTC, float>();

    public GameObject leftEye;
    public GameObject rightEye;
    private GameObject[] EyeAnchors;
    private Vector3 GazeDirectionCombinedLocalLeft = Vector3.zero;
    private Vector3 GazeDirectionCombinedLocalRight = Vector3.zero;
    public GameObject GazeDirectionVisualizationLeft, GazeDirectionVisualizationRight;

    
    //For data collection
    private String path = "";
    public String subjectName = "Subject Name";

    string day = DateTime.Now.Day.ToString();
    string month = DateTime.Now.Month.ToString();
    string year = DateTime.Now.Year.ToString();
    string hour = DateTime.Now.Hour.ToString();
    string minutes = DateTime.Now.Minute.ToString();
    string seconds = DateTime.Now.Second.ToString();

    //GameObject references for data collection
    GameObject headGO, leftHandGO, rightHandGO, leftEyeGO, rightEyeGO, leftEyeSphereGO, rightEyeSphereGO, electricStove, teaKettle, sugarHeapOnSpoon;
    ActionToGrabbing leftHandScript, rightHandScript;
    ManageFillLevel mugFillLevelManager;
    StovePlateScript stovePlateScript;
    ManageWaterflow teaKettleScript;
    sugarSpoonScript sugarOnSpoonScript;

    //For sphere cast
    private float sphereCastRadius = 0.07f;
    private Vector3 positionLeftSphereCollision, positionRightSphereCollision = Vector3.down * 100;

    // Start is called before the first frame update
    void Start()
    {
        //Set the mapping relations between OpenXR eye shapes (i.e. "what we get") and avatar blend shapes (i.e. "what we display in Unity")
        ShapeMap = new Dictionary<XrEyeShapeHTC, SkinnedMeshRendererShape>();
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_BLINK_HTC, SkinnedMeshRendererShape.Eye_Left_Blink);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_WIDE_HTC, SkinnedMeshRendererShape.Eye_Left_Wide);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_BLINK_HTC, SkinnedMeshRendererShape.Eye_Right_Blink);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_WIDE_HTC, SkinnedMeshRendererShape.Eye_Right_Wide);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_SQUEEZE_HTC, SkinnedMeshRendererShape.Eye_Left_Squeeze);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_SQUEEZE_HTC, SkinnedMeshRendererShape.Eye_Right_Squeeze);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_DOWN_HTC, SkinnedMeshRendererShape.Eye_Left_Down);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_DOWN_HTC, SkinnedMeshRendererShape.Eye_Right_Down);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_OUT_HTC, SkinnedMeshRendererShape.Eye_Left_Left);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_IN_HTC, SkinnedMeshRendererShape.Eye_Right_Left);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_IN_HTC, SkinnedMeshRendererShape.Eye_Left_Right);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_OUT_HTC, SkinnedMeshRendererShape.Eye_Right_Right);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_UP_HTC, SkinnedMeshRendererShape.Eye_Left_Up);
        ShapeMap.Add(XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_UP_HTC, SkinnedMeshRendererShape.Eye_Right_Up);
        
        //Create anchors for left and right eyes
        EyeAnchors = new GameObject[2];
        EyeAnchors[0] = new GameObject();
        EyeAnchors[0].name = "EyeAnchor_" + 0;
        EyeAnchors[0].transform.SetParent(gameObject.transform);
        EyeAnchors[0].transform.localPosition = leftEye.transform.localPosition;
        EyeAnchors[0].transform.localRotation = leftEye.transform.localRotation;
        EyeAnchors[0].transform.localScale = leftEye.transform.localScale;
        EyeAnchors[1] = new GameObject();
        EyeAnchors[1].name = "EyeAnchor_" + 1;
        EyeAnchors[1].transform.SetParent(gameObject.transform);
        EyeAnchors[1].transform.localPosition = rightEye.transform.localPosition;
        EyeAnchors[1].transform.localRotation = rightEye.transform.localRotation;
        EyeAnchors[1].transform.localScale = rightEye.transform.localScale;
        
        // Start the eye tracking detection
        facialmanager = new FacialManager();
        facialmanager.StartFramework(XrFacialTrackingTypeHTC.XR_FACIAL_TRACKING_TYPE_EYE_DEFAULT_HTC);


        //Get the game objects for data collection
        headGO = GameObject.Find("Vision");
        leftHandGO = GameObject.Find("LeftHand");
        rightHandGO = GameObject.Find("RightHand");
        leftHandScript = leftHandGO.GetComponent<ActionToGrabbing>();
        rightHandScript = rightHandGO.GetComponent<ActionToGrabbing>();
        mugFillLevelManager = GameObject.Find("FluidSimulationCylinder").GetComponent<ManageFillLevel>();
        electricStove = GameObject.Find("StovePlate");
        stovePlateScript = electricStove.GetComponent<StovePlateScript>();
        teaKettle = GameObject.Find("TeaKettle");
        teaKettleScript = teaKettle.GetComponent<ManageWaterflow>();
        sugarHeapOnSpoon = GameObject.Find("SugarHeap");
        sugarOnSpoonScript = sugarHeapOnSpoon.GetComponent<sugarSpoonScript>();

        //Get the data from the menu for data collection
        subjectName = StaticGameOptions.subjectName;
        String session = StaticGameOptions.sessionNr.ToString();
        String level = StaticGameOptions.levelSelected.ToString();

        //Start data tracking at 100Hz
        path = "D:\\Projects\\Tea Cooking\\DataTeaCooking\\" + subjectName + "_" + "Session" + session + "_" + "Level" + level + "_" + day + "_" + month + "_" + year + "_" + hour + "_" + minutes + "_" + seconds + ".txt";
        InvokeRepeating("CreateText", 1.0f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
        Debug.Log("-------");*/

        //Get the eye tracking detection results
        facialmanager.GetWeightings(out EyeWeightings);

        //Update avatar blend shapes 
        for (XrEyeShapeHTC i = XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_BLINK_HTC; i < XrEyeShapeHTC.XR_EYE_EXPRESSION_MAX_ENUM_HTC; i++)
        {
            HeadskinnedMeshRenderer.SetBlendShapeWeight((int)ShapeMap[i], EyeWeightings[i] * 100f);
        }


        //Update eye rotation and eye gaze direction ----------------------------------------------------------------------
        //Left eye
        GazeDirectionCombinedLocalLeft = Vector3.zero;
        if (EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_IN_HTC] > EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_OUT_HTC])
        {
            GazeDirectionCombinedLocalLeft.x = EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_IN_HTC];
        }
        else
        {
            GazeDirectionCombinedLocalLeft.x = -EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_OUT_HTC];
        }
        if (EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_UP_HTC] > EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_DOWN_HTC])
        {
            GazeDirectionCombinedLocalLeft.y = EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_UP_HTC];
        }
        else
        {
            GazeDirectionCombinedLocalLeft.y = -EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_LEFT_DOWN_HTC];
        }
        GazeDirectionCombinedLocalLeft.z = (float)1.0;
        Vector3 target = EyeAnchors[0].transform.TransformPoint(GazeDirectionCombinedLocalLeft);
        leftEye.transform.LookAt(target);

        //Right eye
        GazeDirectionCombinedLocalRight = Vector3.zero;
        if (EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_IN_HTC] > EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_OUT_HTC])
        {
            GazeDirectionCombinedLocalRight.x = -EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_IN_HTC];
        }
        else
        {
            GazeDirectionCombinedLocalRight.x = EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_OUT_HTC];
        }
        if (EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_UP_HTC] > EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_DOWN_HTC])
        {
            GazeDirectionCombinedLocalRight.y = EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_UP_HTC];
        }
        else
        {
            GazeDirectionCombinedLocalRight.y = -EyeWeightings[XrEyeShapeHTC.XR_EYE_EXPRESSION_RIGHT_DOWN_HTC];
        }
        GazeDirectionCombinedLocalRight.z = (float)1.0;
        target = EyeAnchors[1].transform.TransformPoint(GazeDirectionCombinedLocalRight);
        rightEye.transform.LookAt(target);
        //--------------------------------------------------------------------------------------------------------------------------------------
    }

    private void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 3;

        // This would cast rays only against colliders in layer 3.
        // But instead we want to collide against everything except layer 3. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;


        //DO A RAYCAST--------------------------------------------------------------------
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer, left eye
        if (Physics.Raycast(EyeAnchors[0].transform.position, transform.TransformDirection(GazeDirectionCombinedLocalLeft), out hit, 100, layerMask))
        {
            //Debug.DrawRay(EyeAnchors[0].transform.position, transform.TransformDirection(GazeDirectionCombinedLocalLeft) * hit.distance, Color.yellow);
            GazeDirectionVisualizationLeft.transform.position = hit.point;
            leftEyeGO = hit.transform.gameObject;
        }
        else
        {
            //Debug.DrawRay(EyeAnchors[0].transform.position, transform.TransformDirection(GazeDirectionCombinedLocalLeft) * 1000, Color.white);
            GazeDirectionVisualizationLeft.transform.position = Vector3.down * 100;
            leftEyeGO = null;
        }

        // Does the ray intersect any objects excluding the player layer, right eye ------------------------------------------------------------------
        if (Physics.Raycast(EyeAnchors[1].transform.position, transform.TransformDirection(GazeDirectionCombinedLocalRight), out hit, 100, layerMask))
        {
            //Debug.DrawRay(EyeAnchors[0].transform.position, transform.TransformDirection(GazeDirectionCombinedLocalLeft) * hit.distance, Color.yellow);
            GazeDirectionVisualizationRight.transform.position = hit.point;
            rightEyeGO = hit.transform.gameObject;
        }
        else
        {
            //Debug.DrawRay(EyeAnchors[0].transform.position, transform.TransformDirection(GazeDirectionCombinedLocalLeft) * 1000, Color.white);
            GazeDirectionVisualizationRight.transform.position = Vector3.down * 100;
            rightEyeGO = null;
        }

        //DO A SPHERECAST-----------------------------------------------------------------------------------
        // Does the sphere intersect any objects excluding the player layer, left eye
        if (Physics.SphereCast(EyeAnchors[0].transform.position, sphereCastRadius, transform.TransformDirection(GazeDirectionCombinedLocalLeft), out hit, 100, layerMask))
        {
            positionLeftSphereCollision = hit.point;
            leftEyeSphereGO = hit.transform.gameObject;
        }
        else
        {
            positionLeftSphereCollision = Vector3.down * 100;
            leftEyeSphereGO = null;
        }
        //Debug.Log(getNameOfGameObject(leftEyeSphereGO));

        // Does the ray intersect any objects excluding the player layer, right eye ------------------------------------------------------------------
        if (Physics.SphereCast(EyeAnchors[1].transform.position, sphereCastRadius, transform.TransformDirection(GazeDirectionCombinedLocalRight), out hit, 100, layerMask))
        {
            positionRightSphereCollision = hit.point;
            rightEyeSphereGO = hit.transform.gameObject;
        }
        else
        {
            positionRightSphereCollision = Vector3.down * 100;
            rightEyeSphereGO = null;
        }
        //Debug.Log(getNameOfGameObject(rightEyeSphereGO));
    }

    //This function is called, when the according GameObject is destroyed
    private void OnDestroy()
    {
        // Stop the eye tracking detection
        facialmanager.StopFramework(XrFacialTrackingTypeHTC.XR_FACIAL_TRACKING_TYPE_EYE_DEFAULT_HTC);
    }

    //Writes the data into the  data tracking file
    void CreateText()
    {
        // create file if it doesn't exist
        if (!File.Exists(path))
        {

            File.WriteAllText(path, "Unity time; " +
                                    "LeftEyeGazeDirectionGlobalX; LeftEyeGazeDirectionGlobalY; LeftEyeGazeDirectionGlobalZ;" +
                                    "RightEyeGazeDirectionGlobalX; RightEyeGazeDirectionGlobalY; RightEyeGazeDirectionGlobalZ;" +
                                    "LeftEyeGazeDirectionLocalX; LeftEyeGazeDirectionLocalY; LeftEyeGazeDirectionLocalZ;" +
                                    "RightEyeGazeDirectionLocalX; RightEyeGazeDirectionLocalY; RightEyeGazeDirectionLocalZ;" +
                                    "LeftEyeGazePositionX; LeftEyeGazePositionY; LeftEyeGazePositionZ; " +
                                    "RightEyeGazePositionX; RightEyeGazePositionY; RightEyeGazePositionZ; " +
                                    "FocusedObjectLeftEye; FocusedObjectRightEye;" +
                                    "FocusedObjectLeftEyeSphereCast; FocusedObjectRightEyeSphereCast;" +
                                    "LeftEyeGazePositionXSphere; LeftEyeGazePositionYSphere; LeftEyeGazePositionZSphere; " +
                                    "RightEyeGazePositionXSphere; RightEyeGazePositionYSphere; RightEyeGazePositionZSphere; " +
                                    "PositionLeftEyeX; PositionLeftEyeY; PositionLeftEyeZ; " +
                                    "PositionRightEyeX; PositionRightEyeY; PositionRightEyeZ; " +
                                    "HeadRotationAroundX; HeadRotationAroundY; HeadRotationAroundZ; " +
                                    "LeftHandPositionX; LeftHandPositionX; LeftHandPositionZ; " +
                                    "RightHandPositionX; RightHandPositionY; RightHandPositionZ; " +
                                    "LeftHandRotationAroundX; LeftHandRotationAroundY; LeftHandRotationAroundZ; " +
                                    "RightHandRotationAroundX; RightHandRotationAroundY; RightHandRotationAroundZ; " +
                                    "IsAnObjectGrabbedLeft; IsAnObjectGrabbedRight; GrabbedObjectNameLeft; GrabbedObjectNameRight; " +
                                    "LeftGrabbedObjectPositionX; LeftGrabbedObjectPositionY; LeftGrabbedObjectPositionZ; " +
                                    "RightGrabbedObjectPositionX; RightGrabbedObjectPositionY; RightGrabbedObjectPositionZ; " +
                                    "StoveStatus; WaterIsCooking; " +
                                    "TeaBagInMug; IsWaterInCup ;TeaIsBrewing; TeaIsDone; " +
                                    "SugarOnSpoon; SugarInCup " +
                                    "\n");;

        }

        //additional info?
        ////foreach (string x in allVar)
        //for (int i=0; i<allVar.Count; i+=10)
        //{

        //    // File.AppendAllText(path, x.ToString() + "\n");
        //    File.AppendAllText(path, allVar[i].ToString() + "\n");

        //}

        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine(Time.realtimeSinceStartup.ToString() + ";" +
            transform.TransformDirection(GazeDirectionCombinedLocalLeft).x.ToString() + "; " + transform.TransformDirection(GazeDirectionCombinedLocalLeft).y.ToString() + "; " + transform.TransformDirection(GazeDirectionCombinedLocalLeft).z.ToString() + "; " +
            transform.TransformDirection(GazeDirectionCombinedLocalRight).x.ToString() + "; " + transform.TransformDirection(GazeDirectionCombinedLocalRight).y.ToString() + "; " + transform.TransformDirection(GazeDirectionCombinedLocalRight).z.ToString() + "; " +
            GazeDirectionCombinedLocalLeft.x.ToString() + "; " + GazeDirectionCombinedLocalLeft.y.ToString() + "; " + GazeDirectionCombinedLocalLeft.z.ToString() + "; " +
            GazeDirectionCombinedLocalRight.x.ToString() + "; " + GazeDirectionCombinedLocalRight.y.ToString() + "; " + GazeDirectionCombinedLocalRight.z.ToString() + "; " +
            GazeDirectionVisualizationLeft.transform.position.x.ToString() + "; " + GazeDirectionVisualizationLeft.transform.position.y.ToString() + "; " + GazeDirectionVisualizationLeft.transform.position.z.ToString() + "; " +
            GazeDirectionVisualizationRight.transform.position.x.ToString() + "; " + GazeDirectionVisualizationRight.transform.position.y.ToString() + "; " + GazeDirectionVisualizationRight.transform.position.z.ToString() + "; " +
            getNameOfGameObject(leftEyeGO) + "; " + getNameOfGameObject(rightEyeGO) + "; " +
            getNameOfGameObject(leftEyeSphereGO) + "; " + getNameOfGameObject(rightEyeSphereGO) + "; " +
            positionLeftSphereCollision.x.ToString() + "; " + positionLeftSphereCollision.y.ToString() + "; " + positionLeftSphereCollision.z.ToString() + "; " +
            positionRightSphereCollision.x.ToString() + "; " + positionRightSphereCollision.y.ToString() + "; " + positionRightSphereCollision.z.ToString() + "; " +
            EyeAnchors[0].transform.position.x.ToString() + "; " + EyeAnchors[0].transform.position.y.ToString() + "; " + EyeAnchors[0].transform.position.z.ToString() + "; " +
            EyeAnchors[1].transform.position.x.ToString() + "; " + EyeAnchors[1].transform.position.y.ToString() + "; " + EyeAnchors[1].transform.position.z.ToString() + "; " +
            headGO.transform.rotation.x.ToString() + "; " + headGO.transform.rotation.y.ToString() + "; " + headGO.transform.rotation.z.ToString() + "; " +
            leftHandGO.transform.position.x.ToString() + "; " + leftHandGO.transform.position.y.ToString() + "; " + leftHandGO.transform.position.z.ToString() + "; " +
            rightHandGO.transform.position.x.ToString() + "; " + rightHandGO.transform.position.y.ToString() + "; " + rightHandGO.transform.position.z.ToString() + "; " +
            leftHandGO.transform.rotation.x.ToString() + "; " + leftHandGO.transform.rotation.y.ToString() + "; " + leftHandGO.transform.rotation.z.ToString() + "; " +
            rightHandGO.transform.rotation.x.ToString() + "; " + rightHandGO.transform.rotation.y.ToString() + "; " + rightHandGO.transform.rotation.z.ToString() + "; " +
            leftHandScript.getIsAnObjectGrabbed().ToString() + "; " + rightHandScript.getIsAnObjectGrabbed().ToString() + "; " + leftHandScript.getObjectGrabbedName() + "; " + rightHandScript.getObjectGrabbedName() + "; " +
            leftHandScript.getPositionOfGrabbedObject() + "; " +
            rightHandScript.getPositionOfGrabbedObject() + "; " +
            getStoveStatusString() + "; " + teaKettleScript.getIsWaterCooking().ToString() + "; " +
            mugFillLevelManager.getTeaBagInMug().ToString() + "; " + mugFillLevelManager.getWaterInMug().ToString() + "; " + mugFillLevelManager.getTeaIsBrewing().ToString() + "; " + mugFillLevelManager.getTeaIsDone().ToString() + "; " +
            sugarOnSpoonScript.getIsSugarOnSpoon().ToString() + "; " + mugFillLevelManager.getSugarAdded().ToString()
            );

        writer.Close();

    }


    public string getNameOfGameObject(GameObject obj)
    {
        if(obj != null)
        {
            return obj.name;
        } else {
            return "NO OBJECT";
        }
    }

    //Returns the status of the stove: 0 - cold (and off); 1- heating; 2 - hot, 3 - cooling down
    private int getStoveStatus()
    {
        //The stove is heated up and on
        if(stovePlateScript.getIsPlateHot())
        {
            return 2;   //the plate is hot
        } else if (stovePlateScript.getIsStoveActive())
        {
            return 1;   //The stove is not hot yet, but active
        }
        else
        {
            if(stovePlateScript.getPlateTemperature() > 0)
            {
                return 3;       // it's not hot or active, but has soe temperature -> it's cooling down
            }
            
            return 0;   //neither is the stove hot, nor activated -> it is off
        }
    }

    private string getStoveStatusString()
    {
        //The stove is heated up and on
        if (stovePlateScript.getIsPlateHot())
        {
            return "Hot";   //the plate is hot
        }
        else if (stovePlateScript.getIsStoveActive())
        {
            return "Heating";   //The stove is not hot yet, but active
        }
        else
        {
            if (stovePlateScript.getPlateTemperature() > 0)
            {
                return "Cooling";       // it's not hot or active, but has soe temperature -> it's cooling down
            }

            return "Deactivated";   //neither is the stove hot, nor activated -> it is off
        }
    }
}
