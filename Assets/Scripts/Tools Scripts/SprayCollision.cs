using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCollision : MonoBehaviour
{

    Transform transform; // the transform of the collider and its transform components
    Vector3 location;
    Quaternion rotation;
    Vector3 scale;

    private List<GameObject> shotObjects; //list of objects that would be collided with by the shot ray

    // Start is called before the first frame update
    void Awake()
    {
        transform = this.gameObject.GetComponent<Transform>();
        location = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        shotObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        readCollide(scale.z);
    }

    //increaseSize method increases the size of an object and moves it along its forward causing it to grow in a direction
    public void increaseSize(float toIncrease)
    {

        scale.z += toIncrease;
        transform.localScale = scale;
    }

    //increaseSize method increases the size of an object and moves it along its forward causing it to grow in a direction, overload has a maxsize which limits how large the object can grow
    public void increaseSize(float toIncrease, float maxSize)
    {
        float newIncrease;
        if (scale.z < maxSize - toIncrease)
        {
            newIncrease = toIncrease;
        }
        else if (scale.z < maxSize)
        {
            newIncrease = maxSize - scale.z;
        }
        else
        {
            newIncrease = 0;
        }

        scale.z += newIncrease;
        transform.localScale = scale;
    }

    private void readCollide(float distance)
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.forward.normalized;
        Vector3 raystart = transform.position;
        if (Physics.Raycast(raystart, rayDirection, out hit, distance))
        {
            Debug.DrawRay(raystart, rayDirection * scale.z, Color.green);
            shotObjects.Add(hit.collider.gameObject);
            scale.z  = hit.distance;
            transform.localScale = scale;
        }
    }

    private void scaleToZero()
    {
        scale.z -= scale.z;
        transform.localScale = scale;

    }

    //return list of shot objects
    public List<GameObject> GetShotObjects()
    { 
        return shotObjects; 
    }

    //clear list of shot objects
    public void ClearShotObjects()
    {
        shotObjects.Clear();
    }

}