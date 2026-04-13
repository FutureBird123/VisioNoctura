using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightsOn : MonoBehaviour
{
    private lightsOutAsylum lightsOutScript;

    public GameObject mainTerminalOffice;
    public GameObject terminalCanvas;
    // Start is called before the first frame update
    void Start()
    {
        lightsOutScript = FindObjectOfType<lightsOutAsylum>();
    }
    public void TurnOnLights()
    {
        if (lightsOutScript != null)
        {
            foreach (GameObject light in lightsOutScript.lights)
            {
                light.SetActive(true);
            }

            mainTerminalOffice.GetComponent<BoxCollider>().enabled = true;
            terminalCanvas.SetActive(true);
            //lightsOutScript.greenLight.SetActive(true);
            //lightsOutScript.redLight.SetActive(false);
        }
    }
    public void OfficeTerminalUnlocked()
    {
        lightsOutScript.greenLightOffice.SetActive(true);
        lightsOutScript.redLightOffice.SetActive(false);
        Debug.Log("Office Terminal Unlocked");
    }
    public void MainTerminalUnlocked()
    {
        lightsOutScript.greenLightMain.SetActive(true);
        lightsOutScript.redLightMain.SetActive(false);
    }
}
