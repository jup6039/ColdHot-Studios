using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTool : Tool
{
    enum projectileState { spray, shoot}
    [SerializeField] private GameObject projectile;
    [SerializeField] private projectileState nozzleType;

    private bool sprayActive;
    private GameObject spray;
    private bool usedThisUpdate;

    // Start is called before the first frame update
    void Start()
    {
        nozzleType = projectileState.spray;
        sprayActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        usedThisUpdate = false;
        if(Input.GetKey(KeyCode.Q))
        {
            Use();
        }

        if(!usedThisUpdate)
        {
            if(sprayActive)
            {
                sprayActive = false;
                Destroy(spray);
            }

        }
    }

    public override void Use()
    {
        usedThisUpdate = true;
        switch(nozzleType)
        {
            case projectileState.spray:
                if(!sprayActive)
                {
                    spray = CreateProjectile();
                    sprayActive = true;
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
