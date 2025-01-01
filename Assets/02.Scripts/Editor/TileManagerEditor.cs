using Scripts.World;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldInitializer))]
public class TileManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldInitializer worldInitializer = (WorldInitializer)target;

        if (GUILayout.Button("Generate Tiles"))
        {
            worldInitializer.Initialize();
        }
        
        if (GUILayout.Button("Clear Tiles"))
        {
            worldInitializer.Clear();
        }
    }
}