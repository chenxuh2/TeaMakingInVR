using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

namespace UnityEngine.XR.OpenXR.Samples.ControllerSample
{
    public class ActionToGrabbing : MonoBehaviour
    {
        public GameObject EnviromentGO;

        public InputActionReference action;
        public InputActionReference hapticAction;
        public float _amplitude = 1.0f;
        public float _duration = 0.1f;
        public float _frequency = 0.0f;
        

        private GameObject objectToGrab, grabbedObject;
        private bool isObjectToGrabInReach = false;
        private bool objectIsGrabbed = false;
        private bool handTouchesPlate = false;

        private void Start()
        {
            if (action == null)
                return;

            action.action.Enable();
            hapticAction.action.Enable();

            action.action.performed += (ctx) =>
            {
                var control = action.action.activeControl;
                if (null == control)
                    return;

                

                //this.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                //GRabbing of the object
                if (isObjectToGrabInReach && objectToGrab != null && !objectIsGrabbed)
                {
                    //Do a haptic feedback
                    OpenXRInput.SendHapticImpulse(hapticAction.action, _amplitude, _frequency, _duration, control.device);

                    //grab the object
                    objectToGrab.transform.parent.SetParent(this.gameObject.transform);
                    grabbedObject = objectToGrab.transform.parent.gameObject;
                    objectIsGrabbed = true;


                    //Deactivate the gravity and other movement while the object is grabbed
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                    //Activate haptic feedback for this hand if the grabbed object touches something
                    grabbedObject.GetComponent<GenerateHapticFeedbackOnCollision>().setGrabbed(this);
                    
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

            if (other.gameObject.CompareTag("StovePlate"))
            {
                    handTouchesPlate = true;
                    other.gameObject.GetComponent<StovePlateScript>().setWaterKettle(this.gameObject);               
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if(other.gameObject == objectToGrab && !objectIsGrabbed)
            {
                isObjectToGrabInReach = false;
                objectToGrab = null;
            }

            if (other.gameObject.CompareTag("StovePlate"))
            {
                handTouchesPlate = false;
            }
        }

        public bool getHandTouchesStove()
        {
            return handTouchesPlate;
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

        public void generateHapticFeedback(float amplitude = 1f, float freq = 0.0f, float duration = 0.1f)
        {
            var control = action.action.activeControl;
            if (null == control)
                return;

            OpenXRInput.SendHapticImpulse(hapticAction.action, amplitude, freq, duration, control.device);
        }

        private void OnDisable()
        {
            action.action.Disable();
            hapticAction.action.Disable();
        }

    }
}
