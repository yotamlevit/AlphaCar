using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBarrier : MonoBehaviour
{
    private int countMove;
    private bool moveRight;
    private int Startc;
    // Start is called before the first frame update
    void Start()
    {
        Startc = 0;
        countMove = 0;
        moveRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Startc > 200)
        {
            if (transform.name.Equals("Barrier (210)"))
            {
                if (countMove > 100)
                {
                    moveRight = !moveRight;
                    countMove = 0;
                }
                if (moveRight)
                {
                    transform.position = new Vector3((float)(transform.position.x + 0.1f), (float)transform.position.y, (float)transform.position.z);
                    this.countMove++;
                }
                else
                {
                    transform.position = new Vector3((float)(transform.position.x - 0.07f), (float)transform.position.y, (float)transform.position.z);
                    this.countMove++;
                }

            }
        }
        else
            this.Startc++;
        
    }
}

