using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreeGen))]
public class TreeGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TreeGen treeGen = (TreeGen)target;

        if (GUILayout.Button("Regenerate"))
        {
            WorldGen worldGen = FindObjectOfType<WorldGen>();
            treeGen.Generate(worldGen, true);
        }
    }
}