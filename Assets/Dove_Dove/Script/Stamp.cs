using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    public GameObject ButtonText;
    public GameObject tamp;
    public void StartEvent()
    {
        ButtonText.SetActive(false);
        tamp.SetActive(true);
    }
    public void EndEvent()
    {
        GameObject.Find("CreateItemCanvas").SetActive(false);
        gameObject.SetActive(false);

    }
}
