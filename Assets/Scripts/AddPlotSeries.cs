using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using UnityEngine.Networking;
using UnityEditor;
using System.Linq;
using System.Runtime.InteropServices;

[RequireComponent(typeof(Button))]
public class AddPlotSeries : MonoBehaviour, IPointerDownHandler

{
    
    [SerializeField] private GameObject _spheres;
    [SerializeField] private GameObject _togglePanel;


    #if UNITY_WEBGL && !UNITY_EDITOR
        //
        // WebGL
        //
        [DllImport("__Internal")]
        private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

        public void OnPointerDown(PointerEventData eventData) {
            UploadFile(gameObject.name, "OnFileUpload", ".png", true);
        }

        // Called from browser
        public void OnFileUpload(string url) {
            StartCoroutine(NewPlotRoutine(url.Split(',')));
        }
    #else
        //
        // Standalone platforms & editor
        //
        public void OnPointerDown(PointerEventData eventData) { }

        void Start() {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick() {
            var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "png", true);
            var urlArr = new List<string>(paths.Length);
            for (int i = 0; i < paths.Length; i++) {
                urlArr.Add(new System.Uri(paths[i]).AbsoluteUri);
            }
            StartCoroutine(NewPlotRoutine(urlArr.ToArray()));
        }
    #endif

        private IEnumerator NewPlotRoutine(string[] urlArr) {
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
                //add sphere to the scene and set up animation manager
                GameObject newSphere = _spheres.GetComponent<SpheresManager>().addSphere(textures.First(), "Plot Series");
                newSphere.AddComponent<SphereAnimationManager>();
                newSphere.GetComponent<SphereAnimationManager>().textures = textures;
                _togglePanel.GetComponent<TogglePanelManager>().addToggleSeries(newSphere, textures);
            }
        }
}
