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
        instruction = GameObject.Find("Instructions");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeInstructions(Globals.instruction);
        if (Globals.displayUI)
            ShowUI();
        else
            HideUI();
        
    }

    public void ChangeInstructions(string text)
    {
        instruction.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void ShowUI()
    {
        instruction.SetActive(true);
    }

    public void HideUI()
    {
        instruction.SetActive(false);
    }
}
