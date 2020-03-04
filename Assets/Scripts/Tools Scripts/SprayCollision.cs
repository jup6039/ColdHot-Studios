using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCollision : MonoBehaviour
{
    Transform transform;
    Vector3 location;
    Quaternion rotation;
    Vector3 scale;
    GameObject shotObject;
    // Start is called before the first frame update
    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        location = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            increaseSize(0.2f);
        }

        if (Input.GetKey(KeyCode.C))
        {
            increaseSize(0.3f, 11f);
        }

        if (Input.GetKey(KeyCode.V))
        {
            increaseSize(-0.2f);
        }

        if (Input.GetKey(KeyCode.G))
        {
            scaleToZero();
        }
    }

    //increaseSize method increases the size of an object and moves it along its forward causing it to grow in a direction
    void increaseSize(float toIncrease)
    {
        float newIncrease = toIncrease;

        if (wouldCollide(newIncrease) != -1)
        {
            newIncrease = (wouldCollide(newIncrease));
        }

        scale.z += newIncrease;
        transform.localScale = scale;
    }

    //increaseSize method increases the size of an object and moves it along its forward causing it to grow in a direction, overload has a maxsize which limits how large the object can grow
    void increaseSize(float toIncrease, float maxSize)
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

        if (wouldCollide(newIncrease) != -1)
        {
            newIncrease = (wouldCollide(newIncrease));
        }

        scale.z += newIncrease;
        transform.localScale = scale;
    }

    private float wouldCollide(float distance)
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.forward.normalized;
        Vector3 raystart = transform.position;
        raystart = raystart - (rayDirection * scale.z);
        if (Physics.Raycast(transform.position, rayDirection, out hit, distance + (scale.z * 2)))
        {
            shotObject = hit.collider.gameObject;
            float toReturn = hit.distance - (scale.z * 2);
            if (toReturn < - scale.z)
            {
                toReturn = -scale.z;
            }
            return toReturn;
        }
        return -1;
    }

    private void scaleToZero()
    {
        scale.z -= scale.z;
        transform.localScale = scale;

    }

}