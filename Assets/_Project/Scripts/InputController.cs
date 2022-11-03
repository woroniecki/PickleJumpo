using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public float AxisHori { get; private set; }
    public float AxisVert { get; private set; }

    public bool Jump { get; private set; }

    // Update is called once per frame
    void Update()
    {
        AxisHori = Input.GetAxis("Horizontal");
        AxisVert = Input.GetAxis("Vertical");

        Jump = Input.GetButtonDown("Jump");
    }
}
