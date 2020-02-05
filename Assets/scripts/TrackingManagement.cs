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
    public GameObject cube;
    public GameObject questionCanvas;
    public GameObject indiceCanvas;
    public GameObject resultCanvas;
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
        //Initialisation des textes
        cube = GameObject.Find("Cube");
        //questionCanvas = GameObject.Find("Question");
        //indiceCanvas = GameObject.Find("Indice");
        //resultCanvas = GameObject.Find("Result");
        
        resultCanvas.GetComponent<Text>().text = "";
        indiceCanvas.GetComponent<Text>().text = "";
        questionCanvas.GetComponent<Text>().text = "";

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
            Debug.Log("REPONSE CORRECTE");
            if(Globals.correctAnswer != "Entrée")
            {
                displayAnswer(true);
            }
            Debug.Log(Globals.correctAnswer);
            Question nextQuestion = new Question();
            await nextQuestion.GetQuestion(Globals.correctAnswer);
            Debug.Log("Question récupérée:");
            Debug.Log(nextQuestion.question);
            DisplayQuestion(nextQuestion);
            Globals.lastDoor = doorId;
            Globals.correctAnswer = nextQuestion.answer;
        }
        else
        {
            Debug.Log("REPONSE FAUSSE");
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
        Debug.Log("PORTE ATTENDUE:" + Globals.correctAnswer);
        Debug.Log("PORTE SCANNEE:" + doorId);
        if (Globals.correctAnswer ==  doorId)
            return true;
        else
            return false;
    }

    private void DisplayQuestion(Question newQuestion)
    {
        Debug.Log("QUESTION A AFFICHER" + newQuestion.question);
        questionCanvas.GetComponent<Text>().text = newQuestion.question.ToString();
        indiceCanvas.GetComponent<Text>().text = newQuestion.indice;
    }

}
