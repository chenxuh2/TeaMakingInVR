using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script manages the sugar on the spoon -> Collection when in sugar box, letting it loose when the spoon rotates
public class sugarSpoonScript : MonoBehaviour
{
    bool sugarOnSpoon = false;
    MeshRenderer thisRenderer;
    
    [SerializeField] ParticleSystem sugarParticleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<MeshRenderer>();
        thisRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //remove the sugar, if the spoon is rotated too much
        float rotationAroundX = this.transform.parent.transform.rotation.eulerAngles.x % 360;
        float rotationAroundZ = this.transform.parent.transform.rotation.eulerAngles.z % 360;
        if ((rotationAroundX > 120 && rotationAroundX < 240) || (rotationAroundZ > 120 && rotationAroundZ < 240))
        {
            removeSugarFromSpoon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FluidAffectedByTeaBag"))
        {

        }
    }

    public void putSugarOnSpoon()
    {
        sugarOnSpoon = true;
        thisRenderer.enabled = true;
    }

    private void removeSugarFromSpoon()
    {
        sugarOnSpoon = false;

        //deactivate the visuals
        thisRenderer.enabled = false;

        //spawn some particles
    }
}
