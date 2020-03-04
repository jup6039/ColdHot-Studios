using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthTest : MonoBehaviour
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
        if(Input.GetKey(KeyCode.K))
        {
            increaseSize(0.2f);
        }

        if(Input.GetKey(KeyCode.C))
        {
            increaseSize(0.3f, 11f);
        }

        if(Input.GetKey(KeyCode.V))
        {
            increaseSize(-0.2f);
        }
        
        if(Input.GetKey(KeyCode.G))
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

        scale.y += newIncrease;
        Vector3 move = transform.up.normalized;
        move = move * newIncrease;
        location += move;
        transform.position = location;
        transform.localScale = scale;
    }

    //increaseSize method increases the size of an object and moves it along its forward causing it to grow in a direction, overload has a maxsize which limits how large the object can grow
    void increaseSize(float toIncrease, float maxSize)
    {
        float newIncrease;
        if (scale.y < maxSize - toIncrease)
        {
            newIncrease = toIncrease;
        }
        else if (scale.y < maxSize)
        {
            newIncrease = maxSize - scale.y;
        }
        else
        {
            newIncrease = 0;
        }

        if (wouldCollide(newIncrease) != -1)
        {
            newIncrease = (wouldCollide(newIncrease));
        }

        scale.y += newIncrease;
        Vector3 move = transform.up.normalized;
        move = move * newIncrease;
        location += move;
        transform.position = location;
        transform.localScale = scale;
    }

    private float wouldCollide(float distance)
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.up.normalized;
        Vector3 raystart = transform.position;
        raystart = raystart - (rayDirection * scale.y);
        if(Physics.Raycast(transform.position, rayDirection, out hit, distance + (scale.y)))
        {
            shotObject = hit.collider.gameObject;
            return hit.distance - (scale.y);
        }
        return -1;
    }

    private void scaleToZero()
    {
        Vector3 move = transform.up.normalized;
        move = move * -scale.y;
        location += move;
        transform.position = location;
        scale.y -= scale.y;
        transform.localScale = scale;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
        //get current length
        float currentLength = scale.y;
        //set scale to 0
        scaleToZero();
        
        //increase size back up to length
        increaseSize(currentLength);
        
    }
}
