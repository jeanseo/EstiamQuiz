using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public GameObject cube;
    public bool correctAnswer;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        //Get the Renderer component from the new cube
        var cubeRenderer = cube.GetComponent<Renderer>();
        if (correctAnswer)
        {
            cubeRenderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            cubeRenderer.material.SetColor("_Color", Color.red);
        }        
    }
}
