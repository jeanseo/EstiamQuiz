using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System.Threading.Tasks;
using Assets.scripts.classes;
using Assets.scripts;

public class TrackingManagement : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour trackableBehaviour;
    public GameObject cube;
    public GameObject questionCanvas;
    public GameObject indiceCanvas;
    public GameObject resultCanvas;
    public String doorId;
    public GameObject ui;
    protected Color answerColor;
    protected bool inFirst = false;

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
    async void Start()
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
        Globals.instruction = "Trouve la porte qui correspond à la réponse";
    }

    private async void OnTrackingFound()
    {
        Globals.instruction = "";
        Debug.Log("TRACKING TROUVE");
        if (IsCorrectAnswer())
        {
            Debug.Log("REPONSE CORRECTE");
            if(Globals.correctAnswer != "Entrée")
            {
                await displayAnswer(true);
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

    private async Task displayAnswer(bool correct)
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

    private IEnumerator DisplayResult()
    {
        inFirst = true;
        Debug.Log("REPONSE");
        yield return new WaitForSeconds(1);
        resultCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        resultCanvas.SetActive(false);
        inFirst = false;
    }

    private IEnumerator DisplayQuestion()
    {
        while (inFirst)
            yield return new WaitForSeconds(0.1f);


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
