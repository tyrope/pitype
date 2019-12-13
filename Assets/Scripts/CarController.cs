using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    enum CarState {
        ARRIVING,
        PARKED,
        LEAVING
    }

    // Preset variables.
    private readonly Vector3 startPos = new Vector3(-15, 0, 9);
    private readonly Vector3 parkPos = new Vector3(0, 0, 9);
    private readonly Vector3 endPos = new Vector3(15, 0, 9);
    private readonly float speed = 3f;

    // State tracking.
    CarState myState = CarState.ARRIVING;
    WordComponent wc;
    float distToPark;
    float distToEnd;
    float startedDrive;


    void Start()
    {
        wc = GetComponent<WordComponent>();
        if(wc == null)
        {
            Debug.LogError("Car spawned without a WordComponent!");
        }

        distToPark = Vector3.Distance(startPos, parkPos);
        distToEnd = Vector3.Distance(parkPos, endPos);
        startedDrive = Time.time;
    }

    private void Update()
    {
        switch (myState)
        {
            case CarState.ARRIVING:
                transform.position = Vector3.Lerp(startPos, parkPos, (Time.time - startedDrive) * speed / distToPark);

                if(transform.position == parkPos)
                {
                    // We've made it!
                    myState = CarState.PARKED;
                    wc.enabled = true;
                }
                break;
            case CarState.PARKED:
                if(wc.enabled == false)
                {
                    // Our wordComponent has disabled itself, drive off.
                    myState = CarState.LEAVING;
                    startedDrive = Time.time;
                }
                break;
            case CarState.LEAVING:
                transform.position = Vector3.Lerp(parkPos, endPos, (Time.time - startedDrive) * speed / distToEnd);

                if(transform.position == endPos)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }
}