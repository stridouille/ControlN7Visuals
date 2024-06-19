using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpheresManager))]
public class SpheresManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SpheresManager manager = (SpheresManager) target;
        if (GUILayout.Button("Add Plots"))
        {
            manager.editorAddPlots();
        }
        if (GUILayout.Button("Add Plot Series"))
        {
            manager.editorAddPlotSeries();
        }
    }
}
