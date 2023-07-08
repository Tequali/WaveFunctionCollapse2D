using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WFCInput))]
public class WFCEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WFCInput wfcInput = (WFCInput)target;
        if(GUILayout.Button("Erstelle Tilemap"))
        {
            wfcInput.CreateWFC();
            wfcInput.CreateTilemap();
        }
        if (GUILayout.Button("Save Tilemap"))
        {
            wfcInput.SaveTileMap();
        }
    }
}
