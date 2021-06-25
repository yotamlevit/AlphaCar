using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSensor : MonoBehaviour
{
    
    public float range = 100f;
    private LineRenderer lr;
    public double distance;
    private string[] checkSens4Wall;
    public ArcadeCar check;//the main object - the car

    //Start is being called on the start of the program - one time
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        if (transform.name.Equals("LaserPoint_4"))
        {
            checkSens4Wall = new string[6];
            checkSens4Wall[0] = "Barrier (160)";
            checkSens4Wall[1] = "Barrier (161)";
            checkSens4Wall[2] = "Barrier (162)";
            checkSens4Wall[3] = "Barrier (163)";
            checkSens4Wall[4] = "Barrier (164)";
            checkSens4Wall[5] = "Barrier (165)";
            //checkSens4Wall[6] = "Barrier (166)";
        }
    }

    // Update is called once per frame - FIxedUpdate because we use Physics
    void FixedUpdate()
    {
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        this.distance = (double)this.range;//the distance that the sensor
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        //checks if go hit
        this.distance = (double)this.range;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range) && hit.collider)
        {
            lr.SetPosition(1, hit.point);// checnge the ray length
            if(hit.transform.tag == "Barrier")
            {
                this.distance = Vector3.Distance(transform.position, hit.point);//set the return value for the sensor
                if (this.distance > this.range)
                    this.distance = (double)this.range;
                if (transform.name.Equals("LaserPoint_3"))
                {
                    if ((hit.transform.name.Equals("Barrier (158)") || (hit.transform.name.Equals("Barrier (166)"))) && this.distance < 15)
                    {
                        check.SetNextMove("left");    
                    }
                    else if (hit.transform.name.Equals("Barrier (210)") && this.distance < 27)
                        check.SetNextMove("weit");
                    //else if (((hit.transform.name.Equals("Barrier (166)")) || (hit.transform.name.Equals("Barrier (167)"))) && this.distance < 15)
                    //    check.SetNextMove("left_166");
                }
                else if(check.GetCountMove() == 0 && transform.name.Equals("LaserPoint_4") && Array.IndexOf(checkSens4Wall, hit.transform.name) > -1 && this.distance < 2.7)//2.7
                {
                    check.SetNextMove("Sleft");
                }
            //else
              //  this.distance = (double)this.range;
            }
        }
        else
        {
            lr.SetPosition(1, transform.forward * range);// set the ray length
        }
        //Debug.Log(transform.name + ": " + distance);//print the sensor return value
    }
}
