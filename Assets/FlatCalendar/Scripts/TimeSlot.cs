using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlot : MonoBehaviour
{
    private int startHour;

    private int startMinute;

    private bool booked;

    public TimeSlot(int startHour, int startMinute, bool booked) {
        this.startHour = startHour;
        this.startMinute = startMinute;
        this.booked = booked;
    }

    public int StartHour {
        get { return startHour; }
    }

    public int StartMinute {
        get { return startMinute; }
    }

    public bool Booked()
    {
        return booked;
    }

    public void setBooked(bool value)
    {
        this.booked = value;
    }
}
