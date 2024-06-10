using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using UnityEngine.Networking;

[RequireComponent(typeof(Button))]
public class AddSinglePlotScript : MonoBehaviour, IPointerDownHandler

{

    public Texture2D output;

    #if UNITY_WEBGL && !UNITY_EDITOR
        //
        // WebGL
        //
        [DllImport("__Internal")]
        private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

        public void OnPointerDown(PointerEventData eventData) {
            UploadFile(gameObject.name, "OnFileUpload", ".png, .jpg", false);
        }

        // Called from browser
        public void OnFileUpload(string url) {
            StartCoroutine(OutputRoutine(url));
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
            var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "png", false);
            if (paths.Length > 0) {
                StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
            }
        }
    #endif

        private IEnumerator OutputRoutine(string url) {
            using (UnityWebRequest loader = UnityWebRequestTexture.GetTexture(url)) {
                yield return loader.SendWebRequest();
                if (loader.result == UnityWebRequest.Result.Success) {
                    output = DownloadHandlerTexture.GetContent(loader);
                }
            }
        }
}
