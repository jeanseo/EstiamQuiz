using Assets.scripts.classes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    private GameObject instruction;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("C'eST PARTI!!!!!");
        instruction = GameObject.Find("Instructions");
        ChangeInstructions("Veuillez vous rendre au panneau d'entrée");
        Globals.correctAnswer = "Entrée";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeInstructions(string text)
    {
        instruction.GetComponent<TextMeshProUGUI>().text = text;
    }
}
