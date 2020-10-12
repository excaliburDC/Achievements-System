using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class TestCase : MonoBehaviour
{
    //
    //Declaration
    //Instantiation
    //Invocation
    //

    //Declaration
    public delegate void SimpleDelegate();

    public void MyFunc() {
        Debug.Log("MY FUNCTION");
    }

    void Start()
    {
        //Instantiation
        SimpleDelegate SDelegate = new SimpleDelegate(MyFunc);
        //Invocation
        SDelegate();
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //fire the event
        }
    }
    */

}

public class Clock
{
    
    public delegate void SecondChangeHandler(object clock, TimeInfoEventArgs timeInformation);

    public event SecondChangeHandler SecondChange;

    // The method which fires the Event
    protected void OnSecondChange(object clock, TimeInfoEventArgs timeInformation)
    {
        if (SecondChange != null)
        {
            SecondChange(clock, timeInformation);
        }
    }

    int one = 1;
    public void Run()
    {
        // Create the TimeInfoEventArgs object to pass to the subscribers
        TimeInfoEventArgs timeInformation = new TimeInfoEventArgs(one);
        // If anyone has subscribed, notify them
        OnSecondChange(this, timeInformation);
    }
}


//
public class TimeInfoEventArgs : EventArgs
{
    public TimeInfoEventArgs(int hour)
    {
        this.hour = hour;
    }

    public readonly int hour;
    public readonly int minute;
    public readonly int second;
}


//Subscriber Class
public class DisplayClock
{
    // Given a clock, subscribe to
    // its SecondChangeHandler event
    public void Subscribe(Clock theClock)
    {
        theClock.SecondChange += new Clock.SecondChangeHandler(TimeHasChanged);
    }

    public void TimeHasChanged(object theClock, TimeInfoEventArgs ti)
    {
        Console.WriteLine("Current Time:");
    }
}
