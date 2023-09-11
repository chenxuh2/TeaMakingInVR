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
        //remove the sugar, if the spoon is rotated too much (kinda buggy since I don't know the rotation of the spoon-mesh)
        /*float rotationAroundX = this.transform.parent.transform.rotation.eulerAngles.x % 360;
        float rotationAroundZ = this.transform.parent.transform.rotation.eulerAngles.z % 360;
        if (sugarOnSpoon && ((rotationAroundX > 0 && rotationAroundX < 270) || (rotationAroundZ > 110 && rotationAroundZ < 290)))
        {
            removeSugarFromSpoon();
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(sugarOnSpoon && other.gameObject.CompareTag("FluidAffectedByTeaBag"))
        {
            removeSugarFromSpoon();

            //Tell the tea there is sugar now
            other.gameObject.GetComponent<ManageFillLevel>().setSugarAdded(true);
        }

        if(other.gameObject.CompareTag("Sugar"))
        {
            //if(rotations are not the one to lose the sugar immediately)
            //float rotationAroundX = this.transform.parent.transform.rotation.eulerAngles.x % 360;
            //float rotationAroundZ = this.transform.parent.transform.rotation.eulerAngles.z % 360;
            //if (!sugarOnSpoon && !((rotationAroundX > 0 && rotationAroundX < 270) || (rotationAroundZ > 110 && rotationAroundZ < 290)))
            //{
                putSugarOnSpoon(); 
            //}
            
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
        sugarParticleSystem.Play();
    }

    public bool getIsSugarOnSpoon()
    {
        return sugarOnSpoon;
    }
}
