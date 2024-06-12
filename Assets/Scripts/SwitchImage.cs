using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImage : MonoBehaviour
{
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2; 

    private bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeImage()
    {
        if (isOn)
        {
            GetComponent<Image>().sprite = sprite2;
            isOn = false;
        }
        else
        {
            GetComponent<Image>().sprite = sprite1;
            isOn = true;
        }
    }
}
