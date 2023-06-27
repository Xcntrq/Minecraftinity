using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyGround))]
public class MyGroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MyGround ground = (MyGround)target;

        if (GUILayout.Button("Regenerate"))
        {
            ground.Generate();
        }
    }
}