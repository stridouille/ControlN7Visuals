using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _spheres;
    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((v) => {
            _spheres.GetComponent<SpheresManager>().changeBackgroundAlpha(v);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
