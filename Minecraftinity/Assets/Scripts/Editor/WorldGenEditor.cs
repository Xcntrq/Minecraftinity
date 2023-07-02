using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGen))]
public class WorldGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldGen worldGen = (WorldGen)target;

        if (GUILayout.Button("Regenerate"))
        {
            worldGen.Generate(true);
        }
    }
}