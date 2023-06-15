using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

namespace UnityEngine.XR.OpenXR.Samples.ControllerSample
{
    public class ActionToGrabbing : MonoBehaviour
    {
        public InputActionReference action;
        public GameObject EnviromentGO;

        private GameObject objectToGrab, grabbedObject;
        private bool isObjectToGrabInReach = false;
        private bool objectIsGrabbed = false;

        private void Start()
        {
            if (action == null)
                return;

            action.action.Enable();
            action.action.performed += (ctx) =>
            {
                var control = action.action.activeControl;
                if (null == control)
                    return;
                
                //this.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                //GRabbing of the object
                if(isObjectToGrabInReach && objectToGrab != null && !objectIsGrabbed)
                {
                    //grab the object
                    objectToGrab.transform.parent.SetParent(this.gameObject.transform);
                    grabbedObject = objectToGrab.transform.parent.gameObject;
                    objectIsGrabbed = true;


                    //Deactivate the gravity and other movement while the object is grabbed
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    
                }
            };

            action.action.canceled += (ctx) =>
            {
                var control = action.action.activeControl;
                if (null == control)
                    return;

                //Let the oject fall
                if (isObjectToGrabInReach && objectToGrab != null && objectIsGrabbed)
                {
                    //Make the environment parent of the objec again
                    grabbedObject.transform.SetParent(EnviromentGO.transform);
                    
                    objectIsGrabbed = false;

                    //Reactivate the gravity and other movement
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = false;

                }

                //this.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.5f);
            };

        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Liftable" && !objectIsGrabbed)
            {
                objectToGrab = other.gameObject;
                isObjectToGrabInReach = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if(other.gameObject == objectToGrab && !objectIsGrabbed)
            {
                isObjectToGrabInReach = false;
                objectToGrab = null;
            }
        }

        public bool getIsAnObjectGrabbed()
        {
            return objectIsGrabbed;
        }

        public string getObjectGrabbedName()
        {
            if (objectIsGrabbed)
            {
                return grabbedObject.name;
            }
            else
            {
                return "None";
            }
        }
        public string getPositionOfGrabbedObject()
        {
            if(objectIsGrabbed)
            {
                return grabbedObject.transform.position.x.ToString() + "; " + grabbedObject.transform.position.y.ToString() + "; " + grabbedObject.transform.position.z.ToString();
            }
            else
            {
                return "No; object; grabbed";
            }
        }
    }
}
