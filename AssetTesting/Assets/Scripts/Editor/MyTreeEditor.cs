using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyTree))]
public class MyTreeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MyTree tree = (MyTree)target;

        if (GUILayout.Button("Regenerate"))
        {
            tree.Generate();
        }
    }
}