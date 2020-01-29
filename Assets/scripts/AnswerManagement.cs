using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAnswer : MonoBehaviour
{
    public GameObject cube;
    public bool correctAnswer;
    // Start is called before the first frame update
    void Start()
    {
        var cubeRenderer = cube.GetComponent<Renderer>();

        if (correctAnswer)
        {
            cubeRenderer.material.SetColor("_Color", Color.green);
        }
    }

    // Update is called once per frame

}
