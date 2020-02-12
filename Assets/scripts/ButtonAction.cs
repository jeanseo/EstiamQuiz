using Assets.scripts.classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public GameObject indiceText;
    protected Parcours parcours;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        parcours = new Parcours();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        parcours.indiceActivated();
        gameObject.SetActive(false);
        indiceText.SetActive(true);

    }
}
