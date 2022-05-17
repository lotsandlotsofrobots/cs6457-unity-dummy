using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    DateTime last;
    float lastAngle;

    // Start is called before the first frame update
    void Start()
    {
        lastAngle = 0;
        last = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        DateTime now = DateTime.Now;
        float seconds = (float) (now - last).TotalSeconds;

        if (seconds < 0.0625)
        {
            return;
        }

        // convert to angle at a rate of (180 degrees / second)

        // if lastAngle = 31.56 and seconds = 1/60 = 0.016
        // angle = 31.56 + (0.016 * 180) => 34.44
        //
        // if lastAngle = 31.56 and seconds = 1
        // angle = 31.56 + (1 * 180) => 211.56
        //
        // if lastAngle = 31.56 and seconds = 2
        // angle = 31.56 + (2 * 180) => 391.56
        //     -> then angle = 31.56

        float angle = seconds * 180f;


        transform.Rotate(new Vector3(0, angle, 0), Space.Self);
        
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        last = now;
    }
}
