using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypicalDay : MonoBehaviour
{


    private string name;

    public TimeSlot[] timeSlots;

    public TypicalDay()
    {
        this.name = name;
        timeSlots = new TimeSlot[48];
        for (int i = 0; i < 24; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                timeSlots[(i*2)+j] = new TimeSlot(i, j *30, false);
            }
        }
    }

    public string Name
    {
        get { return name; }
    }

    public TimeSlot[] TimeSlots
    {
        get { return this.timeSlots; }
        set { this.timeSlots = value; }
    }
}

 
