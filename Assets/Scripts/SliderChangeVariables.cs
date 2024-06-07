using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChangeVariables : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _objectToChange;
    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((v) => {
            foreach(Transform child in _objectToChange.transform)
            {
                child.GetComponent<Renderer>().material.SetFloat("_decreaseAlpha", v);
            }
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
