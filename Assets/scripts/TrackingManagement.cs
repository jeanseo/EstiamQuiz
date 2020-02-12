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
    public GameObject canvas;
    protected GameObject questionCanvas;
    protected GameObject indiceCanvas;
    protected GameObject resultCanvas;
    protected GameObject getIndiceButton;
    public String doorId;
    public GameObject ui;
    protected Color answerColor;
    protected bool inFirst = false;
    protected Question nextQuestion = new Question();
    protected Parcours parcours;
    protected bool indiceUsed;
    protected List<string> previousBadAnswer = new List<string>();

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
        indiceUsed = false;
        //Chargement des questions
        parcours = new Parcours();
        //Initialisation des objets
        resultCanvas = canvas.transform.Find("Result").gameObject;
        questionCanvas = canvas.transform.Find("Question").gameObject;
        indiceCanvas = canvas.transform.Find("Indice").gameObject;
        getIndiceButton = canvas.transform.Find("IndiceAction").gameObject;

        //Initialisation des textes

        resultCanvas.GetComponent<Text>().text = "";
        indiceCanvas.GetComponent<Text>().text = "";
        questionCanvas.GetComponent<Text>().text = "";
        getIndiceButton.SetActive(false);

        indiceCanvas.SetActive(false);

        trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (trackableBehaviour)
            trackableBehaviour.RegisterTrackableEventHandler(this);

    }

    private void onTrackingLost()
    {
        Debug.Log("TRACKING PERDU");
        Globals.displayInformation = true;
        
        
    }

    private void OnTrackingFound()
    {
        Globals.displayInformation = false;
        //On efface le texte de la question si la porte n'est plus utilisée
        if (Globals.lastDoor != doorId)
        {
            indiceCanvas.GetComponent<Text>().text = "";
            questionCanvas.GetComponent<Text>().text = "";
        }
        //On efface les instructions
        Debug.Log("TRACKING TROUVE");
        // Si on est sur la porte Entrée
        if(doorId == Globals.firstDoorID)
        {
            // On affiche la question si on est bien au début du formulaire
            if (parcours.getCurrentQuestion().id == 0)
            {
                //On démarre le jeu
                if (!parcours.IsGameStarted())
                    parcours.StartGame();
                //On affiche la question
                DisplayQuestionText(parcours.getCurrentQuestion());
                Globals.instruction = "Trouve la porte qui correspond à la réponse";

            }
        }
        //On est sur une autre porte
        else
        {
            //On vérifie si le jeu est lancé
            Debug.Log("JEU LANCE???: " + parcours.IsGameStarted());
            if (parcours.IsGameStarted())
            {
                //On vérifie si la porte correspond à la réponse
                if (parcours.IsCorrectAnswer(doorId))
                {
                    //On indique que la réponse est correcte
                    parcours.SetScore();
                    parcours.PassToNextQuestion();
                    Globals.lastDoor = doorId;
                    //on affiche réponse juste puis la prochaine question
                    DisplayAnswer(true);
                    //On incrémente le score

                }
                else
                {
                    //On affiche que c'est la mauvaise réponse si ce n'est pas la porte sur laquelle apparait la question en cours
                    if (Globals.lastDoor != doorId)
                    {
                        DisplayAnswer(false);
                        //Si on a jamais proposé cette réponse pour cette question
                        if (!previousBadAnswer.Contains(doorId))
                        {
                            //On ajoute cette réponse à la liste des mauvaises réponses
                            previousBadAnswer.Add(doorId);
                            //On retire un point
                            parcours.SetPenalty();
                        }


                    }


                }
            }
            
        }
    }

    private void DisplayAnswer(bool correct)
    {
        if (correct)
        {
            resultCanvas.GetComponent<Text>().text = "BONNE REPONSE";
            resultCanvas.GetComponent<Text>().color = Color.green;
            StartCoroutine("DisplayResult");
            //On affiche également la prochaine question
            StartCoroutine("DisplayQuestion");
        }
        else
        {
            resultCanvas.GetComponent<Text>().text = "MAUVAISE REPONSE";
            resultCanvas.GetComponent<Text>().color = Color.red;
            StartCoroutine("DisplayResult");
        }


    }

    private IEnumerator DisplayResult()
    {
        //On attend une seconde, puis on affiche pendant 3 secondes
        inFirst = true;
        Debug.Log("REPONSE");
        yield return new WaitForSeconds(1);
        resultCanvas.SetActive(true);
        yield return new WaitForSeconds(2);
        //On affiche la prochaine question si ce n'est pas la fin du questionnaire
        resultCanvas.SetActive(false);
        if (!parcours.IsLastAnswer())
        {
            inFirst = false;
        }
        else
        {
            parcours.Finish();
        }
    }

    private IEnumerator DisplayQuestion()
    {
        while (inFirst)
            yield return new WaitForSeconds(0.1f);
        DisplayQuestionText(parcours.getCurrentQuestion());

    }

    private void DisplayQuestionText(Question newQuestion)
    {
        Debug.Log("QUESTION A AFFICHER" + newQuestion.question);
        questionCanvas.GetComponent<Text>().text = newQuestion.question.ToString();
        indiceCanvas.GetComponent<Text>().text = newQuestion.indice;
        getIndiceButton.SetActive(true);
    }

}
