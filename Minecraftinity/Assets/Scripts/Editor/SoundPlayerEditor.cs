using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundPlayer))]
public class SoundPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SoundPlayer soundPlayer = (SoundPlayer)target;

        if (GUILayout.Button("Test"))
        {
            soundPlayer.PlayRandomSound();
        }
    }
}