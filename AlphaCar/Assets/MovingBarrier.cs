using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.name.Equals("Barrier (210)"))
        {
            if (transform.position.x < -165)
                transform.position = new Vector3((float)transform.position.x+1, (float)transform.position.y, (float)transform.position.z);
            else if(transform.position.x >= -165)
                transform.position = new Vector3((float)transform.position.x - 1, (float)transform.position.y, (float)transform.position.z);
        }
    }
}
