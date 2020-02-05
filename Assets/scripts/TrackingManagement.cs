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
    protected GameObject cube;
    protected GameObject questionCanvas;
    protected GameObject indiceCanvas;
    protected GameObject resultCanvas;
    public String doorId;
    protected Color answerColor;

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
        cube = GameObject.Find("Cube");
        questionCanvas = GameObject.Find("Question");
        indiceCanvas = GameObject.Find("Indice");
        resultCanvas = GameObject.Find("Result");
        questionCanvas.SetActive(false);
        indiceCanvas.SetActive(false);
        resultCanvas.SetActive(false);
        trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (trackableBehaviour)
            trackableBehaviour.RegisterTrackableEventHandler(this);

    }

    private void onTrackingLost()
    {
        Debug.Log("TRACKING PERDU");
    }

    private async void OnTrackingFound()
    {
        Debug.Log("TRACKING TROUVE");
        if (IsCorrectAnswer())
        {
            Globals.currentLevel++;
            if(Globals.correctAnswer != "Entrée")
            {
                displayAnswer(true);
            }
            Debug.Log(Globals.correctAnswer);
            Question nextQuestion = new Question();
            await nextQuestion.GetQuestion(Globals.correctAnswer);
            DisplayQuestion(nextQuestion);
            Globals.lastDoor = doorId;
            Globals.correctAnswer = nextQuestion.answer;
        }
        else
        {
            if (Globals.lastDoor != doorId)
                displayAnswer(false);
        }
    }

    private void displayAnswer(bool correct)
    {
        if (correct)
        {
            answerColor = Color.green;
            resultCanvas.GetComponent<Text>().text = "BONNE REPONSE";
            resultCanvas.GetComponent<Text>().color = answerColor;
            StartCoroutine("WaitOneSecond");
        }
        else
        {
            answerColor = Color.red;
            resultCanvas.GetComponent<Text>().text = "MAUVAISE REPONSE";
            resultCanvas.GetComponent<Text>().color = answerColor;
            StartCoroutine("WaitOneSecond");
        }
            
        

    }

    private IEnumerator WaitOneSecond()
    {
        Debug.Log("REPONSE");
        yield return new WaitForSeconds(1);
        resultCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        resultCanvas.SetActive(false);
        //cube.GetComponent<Renderer>().material.SetColor("_Color", answerColor);
    }

    private bool IsCorrectAnswer()
    {
        Debug.Log("REPONSE CORRECTE???");
        Debug.Log(Globals.correctAnswer);
        Debug.Log(doorId);
        if (Globals.correctAnswer ==  doorId)
            return true;
        else
            return false;
    }

    private void DisplayQuestion(Question newQuestion)
    {
        questionCanvas.GetComponent<Text>().text = newQuestion.question;
        indiceCanvas.GetComponent<Text>().text = newQuestion.indice;
        questionCanvas.SetActive(true);
        indiceCanvas.SetActive(true);
    }

}
