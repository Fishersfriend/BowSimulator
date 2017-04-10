using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fence))]
public class FenceInspector : Editor {

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if(EditorGUI.EndChangeCheck() && Application.isPlaying) {
            (target as Fence).Generate();
        }
    }
}
