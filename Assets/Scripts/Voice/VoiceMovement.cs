using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class VoiceMovement : MonoBehaviour
{
    private KeywordRecognizer _keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    [SerializeField] private UIInteractions UIInteractions;
    [SerializeField] private FlatCalendar2 flatCalendar2;
    public Text voiceText;
    
    private void Start()
    {
        actions.Add("how are you", Good);
        actions.Add("hello", Forward);
        actions.Add("open calendar", UIInteractions.CalendarPress);
        actions.Add("close calendar", UIInteractions.BackPressCalendar);
        actions.Add("open account", UIInteractions.AccountPress);
        actions.Add("close account", UIInteractions.BackPressAccount);
        actions.Add("open camera", UIInteractions.AREnable);
        actions.Add("close camera", UIInteractions.AREnable);
        actions.Add("create gym schedule", GenerateTrainingSchedule);
        _keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        _keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        voiceText.text = voiceText.text +"\n " +speech.text;
        actions[speech.text].Invoke();
    }

    private void Forward()
    {
        transform.Translate(1, 0 , 0);
    }

    private void Good()
    {
        voiceText.text = voiceText.text +"\n " + "Good";
    }

    void GenerateTrainingSchedule()
    {
       

        for (int i = 0; i < 7; i++)
        {
            DateTime dateTime = new DateTime(DateTime.Now.AddDays(i).Year, DateTime.Now.AddDays(i).Month,
                DateTime.Now.AddDays(i).Day);
            List<FlatCalendar2.EventObj> eventList = flatCalendar2.getEventList(dateTime.Year, dateTime.Month, dateTime.Day);
            
            SetBookings(SetBookingsByExistingEvents(eventList), dateTime);
        }
    }

    void SetBookings(TypicalDay day, DateTime dateTime)
    {
        int i = 0;
        bool lastSlot = false;
        int maxDay = 1;
        int currentSlotsBookedInDay = 0;
        foreach (var timeSlot in day.timeSlots)
        {
            
           
            if (i + 1 >= day.timeSlots.Length)
            {
                lastSlot = true;
            }
            if (!timeSlot.Booked() && timeSlot.StartHour >= 8)
            {
                if (!lastSlot && !day.timeSlots[i + 1].Booked() && currentSlotsBookedInDay < maxDay)
                {
                    timeSlot.setBooked(true);
                    flatCalendar2.addEvent(dateTime.Year, dateTime.Month, dateTime.Day,
                        new FlatCalendar2.EventObj("Gym", "Working out in the gym.",
                            new TimeSpan(timeSlot.StartHour, timeSlot.StartMinute, 0)));
                    currentSlotsBookedInDay++;
                }
            }
            
            i++;
        }
    }

    TypicalDay SetBookingsByExistingEvents( List<FlatCalendar2.EventObj> eventList)
    {
        TypicalDay day = new TypicalDay();
        foreach (var events in eventList)
        {
            TimeSpan? time = events.time;
            foreach (var timeSlot in day.timeSlots)
            {
                //8:00
                
                
                // ex: if booking is at 15:30, then book at 15:30 and 16:00 time slots
                if (timeSlot.StartHour == time.Value.Hours && time.Value.Minutes >= 30 && timeSlot.StartMinute == 30)
                {
                    timeSlot.setBooked(true);
                }
                else if (timeSlot.StartHour == time.Value.Hours+1 && time.Value.Minutes >= 30 && timeSlot.StartMinute == 0 )
                {
                    timeSlot.setBooked(true);
                }

                //ex: if booking is at 16:00, then book at 16:00 and 16:30 time slots
                else if (timeSlot.StartHour == time.Value.Hours && time.Value.Minutes < 30 && timeSlot.StartMinute == 0)
                {
                    timeSlot.setBooked(true);
                }
                else if (timeSlot.StartHour == time.Value.Hours && time.Value.Minutes < 30 && timeSlot.StartMinute == 30)
                {
                    timeSlot.setBooked(true);
                }

                // 15:45 -- Book 16:30-17:00 time slot
                else if (timeSlot.StartHour == time.Value.Hours+1 && time.Value.Minutes > 30 && timeSlot.StartMinute == 30)
                {
                    timeSlot.setBooked(true);
                }
                
                // // 16:15 -- Book 17:00-17:30 time slot
                else if (timeSlot.StartHour == time.Value.Hours+1 && time.Value.Minutes < 30 && time.Value.Minutes > 0 && timeSlot.StartMinute == 0)
                {
                    timeSlot.setBooked(true);
                }
            }
        }

        return day;
    }
}
