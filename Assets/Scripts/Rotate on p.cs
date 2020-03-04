using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateonp : MonoBehaviour
{
    Transform thisTransform;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            Quaternion rotation = thisTransform.rotation;
            rotation.y++;
            thisTransform.rotation = rotation;
        }
    }
}
