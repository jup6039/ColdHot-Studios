using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnArrow : MonoBehaviour
{
    Transform transform;
    Vector3 location;

    // Start is called before the first frame update
    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        location = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            location.y += 0.1f;
            transform.position = location;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            location.y -= 0.1f;
            transform.position = location;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("cube collided");
    }
}
