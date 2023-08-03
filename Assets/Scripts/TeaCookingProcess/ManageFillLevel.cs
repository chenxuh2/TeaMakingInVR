using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManageFillLevel : MonoBehaviour
{
    private bool mugIsEmpty;
    private MeshRenderer thisRenderer;
    [SerializeField] private Material waterMaterial, teaMaterial;


    //Brewing variables
    private float totalBrewingTimeSoFar = 0f;                               //How long was the tea brewing at a certain time?
    [SerializeField] private float teaCookingTime = 10f;                    //The time, the tea needs to finish brewing in seconds
    [SerializeField] private bool teaIsBrewing;                             //Bool, if the tea is brewing right now
    private bool teaIsDone;
    private bool teaBagInMug = false;

    //Water information (if it s cooking and its cooking representation)
    private bool waterIsCooking = false;
    [SerializeField] GameObject steamParticleSystemObject;
    ParticleSystem steamParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<MeshRenderer>();

        //get the steam particle system (must be before the emptyMug() line!)
        steamParticleSystem = steamParticleSystemObject.GetComponent<ParticleSystem>();
        steamParticleSystem.Stop();

        //empty the mug (and initialize the bool "mugIsEmpty")
        emptyMug();
    }

    // Update is called once per frame
    void Update()
    {
        //Change the color of the water/tea, if the tea bag is inside the water and the water is cooking (this last property is included in teaIsBrewing)
        if(teaIsBrewing && totalBrewingTimeSoFar <= teaCookingTime)
        {
            changeColorOfFillingToTea(totalBrewingTimeSoFar);
            totalBrewingTimeSoFar += Time.deltaTime;
        } else if(teaIsBrewing && totalBrewingTimeSoFar >= teaCookingTime)
        {
            teaIsDone = true;
        }

        //Check if the tea was spilled (mug rotated to much)
        float rotationAroundX = transform.rotation.eulerAngles.x % 360;
        float rotationAroundZ = transform.rotation.eulerAngles.z % 360;
        if (!mugIsEmpty && (rotationAroundX > 90 && rotationAroundX < 270) || (rotationAroundZ > 90 && rotationAroundZ < 270))
        {
            emptyMug();
        }
    }

    //CHange the size of this gamobject into the given direction
    public void Resize(float amount, Vector3 direction)
    {
        transform.localPosition += (direction * amount); // Move the object in the direction of scaling, so that the corner on ther side stays in place
        transform.localScale += direction * amount; // Scale object in the specified direction
    }



    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("ParticleCollision");
        if(transform.localScale.y < 0.04f)
        {
            //Actual "filling" of the mug
            Resize(0.0005f, Vector3.up);
            

            //Get the kttle pouring in the water to see if it is cooking
            try
            {
                waterIsCooking = other.transform.parent.GetComponent<ManageWaterflow>().getIsWaterCooking();
                if(waterIsCooking)
                {
                    steamParticleSystem.Play();
                }
            }
            catch 
            { 
                Debug.Log("The script of the water source parent object could not be found");
            }

            //In the case the mug is starting to fill
            if (mugIsEmpty)
            {
                startFillingMug();
            }

            //remove the particle
            //GameObject.Destroy(other);        //This accidentaly deytroyed the particle system, not the particle -> Todo: Change thant, if important
        }
        
    }

    private void emptyMug()
    {
        //Change the size of the "filling" of the mug
        transform.localScale = new Vector3(0.0784799978f, 0.001f, 0.080449f);
        transform.localPosition = new Vector3(-0.0195999742f, -0.0429999828f, 0);

        //Deactivate the visibility of the mug filling
        thisRenderer.enabled = false;
        
        //set the bools
        mugIsEmpty = true;
        teaIsDone = false;

        //reset the brewing timer
        totalBrewingTimeSoFar = 0f;
        changeColorOfFillingToTea(totalBrewingTimeSoFar);
        teaIsBrewing = false;                                       //Todo: MAybe remove this line, depending on the method I use to implement the tea

        //Deactivate the steam if necessary
        if(steamParticleSystem.isPlaying)
        {
            steamParticleSystem.Stop();
        }
    }

    private void startFillingMug()
    {
        //Activate the visibility of the mug filling
        thisRenderer.enabled = true;

        //set the bool and check if brewing now starts
        mugIsEmpty = false;
        setIfTeaIsBrewing();

        //set the color (to water)
        thisRenderer.material.color = waterMaterial.color;
    }

    private void changeColorOfFillingToTea(float timePassed)
    {
        float progression = Mathf.Clamp(timePassed / teaCookingTime, 0, 1);     //progression must be between 0 and 1 for Color.Lerp
        thisRenderer.material.color = Color.Lerp(waterMaterial.color, teaMaterial.color, progression);
    }

    public bool getTeaIsBrewing()
    {
        return teaIsBrewing;
    }

    public bool getTeaIsDone()
    {
        return teaIsDone;
    }

    public bool getTeaBagInMug()
    {
        return teaBagInMug;
    }

    public bool getWaterInMug()
    {
        return !mugIsEmpty;
    }

    public void setTeaBagStatus(bool value)
    {
        teaBagInMug = value;
        setIfTeaIsBrewing();
    }

    //Checks the conditions for brewing tea(mug is not empty and a tea bag is in the mug)
    public void setIfTeaIsBrewing()
    {
        if(!mugIsEmpty && teaBagInMug && waterIsCooking)
        {
            teaIsBrewing = true;
        } else              //e.g. after removing a tea bag
        {
            teaIsBrewing = false;
        }
    }
}
