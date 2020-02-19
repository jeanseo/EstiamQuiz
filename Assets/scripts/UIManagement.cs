using Assets.scripts.classes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    private GameObject instruction;
    private GameObject score;
    private Parcours parcours;
    private GameObject ranking;
    private GameObject nameInput;
    private GameObject sendForm;

    // Start is called before the first frame update
    void Start()
    {
        instruction = GameObject.Find("Instructions");
        score = GameObject.Find("Score");
        ranking = GameObject.Find("Ranking");
        nameInput = GameObject.Find("InputField");
        sendForm = GameObject.Find("Submit");
        sendForm.GetComponent<Button>().onClick.AddListener(OnClick);
        parcours = new Parcours(); 
    }

    private void OnClick()
    {
        Debug.Log("Nom submit");
        //Effacer le formulaire
        Globals.displayForm = false;

        parcours.SendScore();
    }

    // Update is called once per frame
    void Update()
    {
        instruction.GetComponent<TextMeshProUGUI>().text = Globals.instruction;
        score.GetComponent<TextMeshProUGUI>().text = parcours.getScore().ToString()+" PTS";
        ranking.GetComponent<TextMeshProUGUI>().text = Globals.ranking;
        instruction.SetActive(Globals.displayInformation);
        score.SetActive(Globals.displayScore);
        ranking.SetActive(Globals.displayRanking);
        nameInput.SetActive(Globals.displayForm);
        sendForm.SetActive(Globals.displayForm);
        Globals.playerName = nameInput.GetComponent<InputField>().text;

    }

    public void ChangeInstructions(string text)
    {
        instruction.GetComponent<TextMeshProUGUI>().text = text;
    }

}
