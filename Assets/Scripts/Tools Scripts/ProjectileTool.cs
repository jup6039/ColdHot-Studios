﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTool : Tool
{
    enum projectileState { spray, shoot}
    [SerializeField] private GameObject projectile;
    [SerializeField] private projectileState nozzleType;

    // ---------------------------------------- Spray Variables
    [SerializeField] private float growthPerFrame; //how far the spray extends per frame of usage
    [SerializeField] private float maxSize; //how far the spray can extend
    [SerializeField] private float minSize; //the least distance the spray can extend on startup

    private bool sprayActive;
    private GameObject spray;
    private SprayCollision sprayController;

    // Start is called before the first frame update
    void Start()
    {
        nozzleType = projectileState.spray;
        sprayActive = false;
        sprayController = null;
        UsedThisUpdate = false;

        // populate slot list
        AddSlot(Slot.inputType.mod, Slot.slotType.nozzle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!UsedThisUpdate)
        {
            if(sprayActive)
            {
                sprayActive = false;
                sprayController = null;
                Destroy(spray);
            }

        }
    }

    public override void onSlotFilled(Mods _filledMod)
    {
        base.onSlotFilled(_filledMod);

        switch (_filledMod.subtype)
        {
            case Slot.slotType.nozzle:
                maxSize = maxSize * 2.0f;
                break;
        }
    }

    public override void Use()
    {
        switch(nozzleType)
        {
            case projectileState.spray:
                if(!sprayActive)
                {
                    spray = CreateProjectile();
                    sprayController = spray.GetComponentInChildren<SprayCollision>();
                    sprayActive = true;
                    sprayController.increaseSize(minSize);

                }
                else
                {
                    //get our camera's location
                    Transform camTransform = Camera.main.transform;

                    //move the projectile slightly in front of the players camera
                    Vector3 faceVector = camTransform.forward * 2.3f;

                    Vector3 newposition = camTransform.position + faceVector;

                    spray.transform.position = newposition;
                    spray.transform.rotation = camTransform.rotation;

                    //grow the spray based on values
                    sprayController.increaseSize(growthPerFrame, maxSize);

                    //get the list of hit gameobjects
                    List<GameObject> hitObjects = sprayController.GetShotObjects();

                    //for each object shot this frame run its shot if can be shot, for now just debug the object
                    if (hitObjects != null)
                    {
                        for (int i = 0; i < hitObjects.Count; i++)
                        {
                            Sprayable hitSpray;
                            if(hitSpray = hitObjects[i].GetComponent<Sprayable>())
                            {
                                hitSpray.Shot();
                            }
                        }
                    }
                    //remove all objects from list after calling them
                    sprayController.ClearShotObjects();
                }
                break;

            case projectileState.shoot:
                break;
        }
    }

    public GameObject CreateProjectile()
    {
        GameObject toReturn;
        //get our camera's location
        Transform camTransform = Camera.main.transform;

        //move the projectile slightly in front of the players camera
        Vector3 faceVector = camTransform.forward * 2.3f;

        Vector3 newposition = camTransform.position + faceVector;

        toReturn = Instantiate(projectile, newposition, camTransform.rotation);

        return toReturn;
    }
}
