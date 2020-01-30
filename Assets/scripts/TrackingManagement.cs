using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System.Threading.Tasks;
using Assets.scripts.classes;

public class TrackingManagement : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour trackableBehaviour;
    public bool correctAnswer;
    public GameObject cube;
    public String doorId;
    private Color answerColor;

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            OnTrackingFound();
        else
            onTrackingLost();
    }

    // Start is called before the first frame update
    void Start()
    {
        trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (trackableBehaviour)
            trackableBehaviour.RegisterTrackableEventHandler(this);

    }

    private void onTrackingLost()
    {
        Debug.Log("TRACKING PERDU");
        ResetDoor();
    }

    private void OnTrackingFound()
    {
        Debug.Log("TRACKING TROUVE");
        if (IsCorrectAnswer())
        {
            Globals.currentLevel++;
            displayAnswer(true);
        }
        else
        {
            displayAnswer(false);
        }
    }

    private void displayAnswer(bool correct)
    {
        if (correct)
        {
            answerColor = Color.green;
            StartCoroutine("WaitOneSecond");
        }
        else
        {
            answerColor = Color.red;
            StartCoroutine("WaitOneSecond");
        }
            
        

    }

    private IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1);
        cube.GetComponent<Renderer>().material.SetColor("_Color", answerColor);
    }

    private bool IsCorrectAnswer()
    {
        Debug.Log("REPONSE CORRECTE???");
        Debug.Log(Globals.currentLevel);
        Debug.Log(doorId);
        if (Globals.currentLevel == 0 && doorId == "A")
            return true;
        else if (Globals.currentLevel == 1 && doorId == "B")
            return true;
        return false;
    }

    private void ResetDoor()
    {
        cube.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

}
