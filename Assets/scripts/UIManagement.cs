using Assets.scripts.classes;
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
    // Start is called before the first frame update
    void Start()
    {
        instruction = GameObject.Find("Instructions");
        score = GameObject.Find("Score");

        parcours = new Parcours(); 
    }

    // Update is called once per frame
    void Update()
    {
        instruction.GetComponent<TextMeshProUGUI>().text = Globals.instruction;
        score.GetComponent<TextMeshProUGUI>().text = parcours.getScore().ToString()+" PTS";
        instruction.SetActive(Globals.displayInformation);
        score.SetActive(Globals.DisplayScore);
        
    }

    public void ChangeInstructions(string text)
    {
        instruction.GetComponent<TextMeshProUGUI>().text = text;
    }

}
