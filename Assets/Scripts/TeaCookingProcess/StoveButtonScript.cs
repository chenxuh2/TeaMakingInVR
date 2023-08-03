using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script recognizes if the button of the stove is pressed and (de-)activates the stove accordingly (plate and status light)
public class StoveButtonScript : MonoBehaviour
{
    bool stoveStatus = false;       //False: stove is turned off, true: stove is turned on
    [SerializeField] GameObject stoveStatusLight, stovePlate;
    MeshRenderer statusLightRenderer;

    // Start is called before the first frame update
    void Start()
    {
        statusLightRenderer = stoveStatusLight.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I'M a button, really!");
        if(other.gameObject.tag == "Player")
        {
            switchStoveStatus();
        }
    }

    private void switchStoveStatus()
    {
        stoveStatus = !stoveStatus;
        stovePlate.GetComponent<StovePlateScript>().setStoveActivity(stoveStatus);

        if(stoveStatus)
        {
            statusLightRenderer.material.EnableKeyword("_EMISSION");
            statusLightRenderer.material.SetColor("_EmissionColor", new Color(1.0f, 0.64f, 0.0f));
        } else
        {
            //statusLightRenderer.material.DisableKeyword("_EMISSION");
        }
        
    }
}
