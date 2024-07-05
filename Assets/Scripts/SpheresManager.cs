using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SpheresManager : MonoBehaviour
{
    [SerializeField] private Shader _shader;
    [SerializeField] private GameObject _togglePanel;
    [SerializeField, Range(0.1f, 5) ] private float _animationTimeDelta;
    private int plotNum = 1;


    public void editorAddPlots() {
        var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "png", true);
        var urlArr = new List<string>(paths.Length);
        for (int i = 0; i < paths.Length; i++) {
            urlArr.Add(new System.Uri(paths[i]).AbsoluteUri);
        }
        StartCoroutine(NewPlotsRoutine(urlArr.ToArray()));
    }

    private IEnumerator NewPlotsRoutine(string[] urlArr) {
        for (int i = 0; i < urlArr.Length; i++) {
            using (UnityWebRequest loader = UnityWebRequestTexture.GetTexture(urlArr[i])) {
                yield return loader.SendWebRequest();
                if (loader.result == UnityWebRequest.Result.Success) {
                    //create selected texture
                    Texture2D newTex = DownloadHandlerTexture.GetContent(loader);
                    
                    //generate new color for the new plot
                    Color newColor = Color.HSVToRGB(getColorRate(plotNum), 1, 1);

                    //add sphere to the scene
                    string name = System.IO.Path.GetFileNameWithoutExtension(urlArr[i]);
                    GameObject newSphere = addSphere(newTex, name, newColor);
                    
                    //create toggle that sets active the sphere and attach it to TogglePanel
                    _togglePanel.GetComponent<TogglePanelManager>().addToggle(newSphere, newColor, false);
                    plotNum++;
                }
            }
        }
    }

    public void editorAddPlotSeries() {
        var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "png", true);
        var urlArr = new List<string>(paths.Length);
        for (int i = 0; i < paths.Length; i++) {
            urlArr.Add(new System.Uri(paths[i]).AbsoluteUri);
        }
        StartCoroutine(NewPlotSeriesRoutine(urlArr.ToArray()));
    }

    private IEnumerator NewPlotSeriesRoutine(string[] urlArr) {
            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 0; i < urlArr.Length; i++) {
                using (UnityWebRequest loader = UnityWebRequestTexture.GetTexture(urlArr[i])) {
                    yield return loader.SendWebRequest();
                    if (loader.result == UnityWebRequest.Result.Success) {

                        //create selected texture and add to the list
                        Texture2D newTex = DownloadHandlerTexture.GetContent(loader);
                        textures.Add(newTex);

                    }
                }
            }
            if (urlArr.Length > 0) {
                //generate new color for the new plot
                Color newColor = Color.HSVToRGB(getColorRate(plotNum), 1, 1);
                plotNum++;

                //add sphere to the scene and set up animation manager
                string name = System.IO.Path.GetFileNameWithoutExtension(urlArr[0]);
                GameObject newSphere = addSphere(textures.First(), name, newColor);
                SphereAnimationManager animationManager = newSphere.AddComponent<SphereAnimationManager>();
                animationManager.textures = textures;
                animationManager.animationTimeDelta = _animationTimeDelta;
                _togglePanel.GetComponent<TogglePanelManager>().addToggle(newSphere, newColor, true);
            }
        }

    public GameObject addSphere(Texture2D texture, string nom, Color color)
    {
        GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newSphere.name = nom;
        Renderer rend = newSphere.GetComponent<Renderer>();
        Material tmpMaterial = new Material(rend.sharedMaterial);
        tmpMaterial.shader = _shader;
        tmpMaterial.mainTexture = texture;
        tmpMaterial.SetColor("_Color", color);
        rend.sharedMaterial = tmpMaterial;
        newSphere.transform.parent = transform;
        return newSphere;
        
    }

    public void changeBackgroundAlpha(float alpha)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<Renderer>().material.SetFloat("_decreaseAlpha", alpha);
        }
    }

    private float getColorRate(int num) {
        float phi = (1 + Mathf.Sqrt(5))/2;
        return num * phi - Mathf.Floor(num * phi);
    }

    public string[] getPlotsNamesArray()
    {
        string[] plotsNames = new string[_togglePanel.transform.childCount];
        for (int i=0; i<_togglePanel.transform.childCount; i++) {
            plotsNames[i] = _togglePanel.transform.GetChild(i).gameObject.GetComponent<SphereToggle>().getSphereName();
        }
        return plotsNames;
    }

    public int getPlotsCount()
    {
        return _togglePanel.transform.childCount;
    }

    public void deletePlot(int index)
    {
        _togglePanel.transform.GetChild(index).gameObject.GetComponent<SphereToggle>().destroySphere();
        plotNum--;
    }

    public void changeColorPlot(int index, Color color)
    {
        _togglePanel.transform.GetChild(index).gameObject.GetComponent<SphereToggle>().applyColor(color);
    }

    public void changeNamePlot(int index, string newName)
    {
        _togglePanel.transform.GetChild(index).gameObject.GetComponent<SphereToggle>().modifyName(newName);            
    }
}
