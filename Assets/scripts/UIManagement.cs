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
        
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeInstructions(Globals.instruction);
    }

    public void ChangeInstructions(string text)
    {
        instruction.GetComponent<TextMeshProUGUI>().text = text;
    }
}
