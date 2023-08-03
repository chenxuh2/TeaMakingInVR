using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaBagScript : MonoBehaviour
{

    ManageFillLevel fillLevelManager;

    private bool isInMug = false;
    private bool activateMugFollowing;
    GameObject mug;
    GameObject mugFilling;
    Vector3 offsetPosition, offsetRotation = Vector3.zero;
    [SerializeField] GameObject graspingCollisionObject;     //The object with the collider used to grasp the teabag
   

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if(isInMug && !activateMugFollowing && this.transform.parent.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            //Let the movement of the teabag follow the movement of the mug
            activateMugFollowing = true;
            //offsetPosition = this.transform.parent.position - mugFilling.transform.position;            //The offset is computed in the local ccordinate system of the mug
            offsetPosition = mug.transform.InverseTransformPoint(this.transform.parent.position);            //The offset is computed in the local ccordinate system of the mug
            //offsetRotation = this.transform.parent.rotation.eulerAngles - mugFilling.transform.rotation.eulerAngles;

            //Disallow the teabag to be grabbed while it is in the mug
            graspingCollisionObject.SetActive(false);
        }

        if(activateMugFollowing)
        {
            //this.transform.parent.position = mugFilling.transform.position + offsetPosition;
            this.transform.parent.position = mug.transform.TransformPoint(offsetPosition);
            //this.transform.parent.rotation = Quaternion.Euler(mugFilling.transform.rotation.eulerAngles + offsetRotation);
            this.transform.parent.rotation = mugFilling.transform.rotation * Quaternion.Euler(90, 0, 0);


            //let the tea bag loose, if the mug is rotated too much
            float rotationAroundX = mugFilling.transform.rotation.eulerAngles.x % 360;
            float rotationAroundZ = mugFilling.transform.rotation.eulerAngles.z % 360;
            if ((rotationAroundX > 120 && rotationAroundX < 240) || (rotationAroundZ > 120 && rotationAroundZ < 240))
            {
                freeTeaBagFromMug();
            }
        }

        
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FluidAffectedByTeaBag"))
        {
            try
            {
                //Only attach the tea bag to the mug, if the mug is not rotated -> Used to avoid a "flickering" between trigger enter and exit when removing the tea bag from the mug. Sometimes the ontrigger exit is falsely not called and the bag attaches to the mug even if it is not inside TODO: test it (otherwise reactivate isInMug = false in the trigger exit function)
                //float rotationAroundX = mugFilling.transform.rotation.eulerAngles.x % 360;
                //float rotationAroundZ = mugFilling.transform.rotation.eulerAngles.z % 360;
                //if (!((rotationAroundX > 120 && rotationAroundX < 240) || (rotationAroundZ > 120 && rotationAroundZ < 240)))
                //{
                    ManageFillLevel fillLevelManager = other.gameObject.GetComponent<ManageFillLevel>();

                    fillLevelManager.setTeaBagStatus(true);

                    mugFilling = other.gameObject;
                    mug = mugFilling.transform.parent.gameObject;
                    isInMug = true;
                //}

            } catch
            {
                Debug.Log("Could not find the ManageFillLevel class on the other collider object");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FluidAffectedByTeaBag"))
        {
            try
            {
                ManageFillLevel fillLevelManager = other.gameObject.GetComponent<ManageFillLevel>();

                fillLevelManager.setTeaBagStatus(false);

                //IF the tea bag is lifted out of the mug
                //activateMugFollowing = false;
                Debug.Log("Leave");
                isInMug = false;
            } catch
            {
                Debug.Log("Could not find the ManageFillLevel class on the other collider object");
            }
            
        }
    }

    private void freeTeaBagFromMug()
    {
        activateMugFollowing = false;
        isInMug = false;
        this.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;

        //Allow the teabag to be grabbed again
        graspingCollisionObject.SetActive(true);
    }

}
