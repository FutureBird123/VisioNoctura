using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightsOutAsylum : MonoBehaviour
{
    public List<GameObject> lights;
    private bool alreadyTurnedOff = false;
    public bool lightsOff = false;
    public GameObject redLightMain;
    public GameObject greenLightMain;
    public GameObject redLightOffice;
    public GameObject greenLightOffice;

    public GameObject mainTerminalOffice;
    public GameObject terminalCanvas;

    // Start is called before the first frame update
    void Start()
    {
        alreadyTurnedOff = false;
        lightsOff = false;

    }

    // Update is called once per frame

    public void TurnOffLights()
    {
        if (!lightsOff && !alreadyTurnedOff)
        {
            lightsOff = true;
            alreadyTurnedOff = true;
            greenLightMain.SetActive(false);
            redLightMain.SetActive(true);
            greenLightOffice.SetActive(false);
            redLightOffice.SetActive(true);

            mainTerminalOffice.GetComponent<BoxCollider>().enabled = false;
            terminalCanvas.SetActive(false);
            foreach (GameObject light in lights)
            {
                light.SetActive(false);
            }
        }
    }
}
