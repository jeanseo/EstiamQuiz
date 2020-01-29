using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System.Threading.Tasks;

public class TrackingManagement : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour trackableBehaviour;
    public bool correctAnswer;
    public GameObject cube;

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

        displayAnswer();

    }

    private void onTrackingLost()
    {
        //if (transform.childCount > 0)
        //    SetChildrenActive(true);
    }

    private void OnTrackingFound()
    {
        //if (transform.childCount > 0)
        //    SetChildrenActive(false);
        //displayAnswer();
        if (correctAnswer)
        {

        }
    }
    private void SetChildrenActive(bool activeState)
    {
        //for (int i = 0; i <= transform.childCount; i++)
        //    transform.GetChild(i++).gameObject.SetActive(activeState);
        
    }

    private void displayAnswer()
    {
        StartCoroutine("WaitOneSecond");

    }

    private IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(3);
        var cubeRenderer = cube.GetComponent<Renderer>();
        if (correctAnswer)
            cubeRenderer.material.SetColor("_Color", Color.green);
    }

}
