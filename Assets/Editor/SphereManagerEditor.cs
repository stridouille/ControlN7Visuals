using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlasticGui.WorkspaceWindow.Items;

[CustomEditor(typeof(SpheresManager))]
public class SpheresManagerEditor : Editor
{
    private int choice = 0;
    private Color selectedColor = Color.white;
    private string newName = "";

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Manager's variables (do not modify)", EditorStyles.boldLabel);
        DrawDefaultInspector();
        SpheresManager manager = (SpheresManager) target;

        GUILayout.Label("Plots manager", EditorStyles.boldLabel);

        // Add a button to add a new plot
        if (GUILayout.Button("Add plot(s)"))
        {
            manager.editorAddPlots();
        }

        // Add a button to add a new plot series
        if (GUILayout.Button("Add plot series"))
        {
            manager.editorAddPlotSeries();
        }

        if (manager.getPlotsCount() == 0)
        {
            return;
        }
        
        // Add a drop down menu to choose a plot to modify
        string[] plotsNames = manager.getPlotsNamesArray();

        choice = EditorGUILayout.Popup("Modify plot:", choice, plotsNames);

        // Modify name
        EditorGUILayout.BeginHorizontal();
        newName = EditorGUILayout.TextField("Name:", newName);
        if (GUILayout.Button("Change name"))
        {
            if (newName.Length > 0) {
                manager.changeNamePlot(choice, newName);
            }
        }
        EditorGUILayout.EndHorizontal();
        // Modify color
        EditorGUILayout.BeginHorizontal();
        selectedColor = EditorGUILayout.ColorField("Color:", selectedColor);
        if (GUILayout.Button("Change color")) {
            manager.changeColorPlot(choice, selectedColor);
        }
        EditorGUILayout.EndHorizontal();

        // Delete the plot
        if (GUILayout.Button("Delete")) {
            manager.deletePlot(choice);
        }
    }    
}
