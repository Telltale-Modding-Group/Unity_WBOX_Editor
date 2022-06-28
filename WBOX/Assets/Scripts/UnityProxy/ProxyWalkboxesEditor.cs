using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProxyWalkboxes))]
[CanEditMultipleObjects]
public class ProxyWalkboxesEditor : Editor
{
    SerializedProperty mTris;
    SerializedProperty mVerts;
    SerializedProperty mNormals;
    SerializedProperty mQuads;

    void OnEnable()
    {
        mTris = serializedObject.FindProperty("mTris");
        mVerts = serializedObject.FindProperty("mVerts");
        mNormals = serializedObject.FindProperty("mNormals");
        mQuads = serializedObject.FindProperty("mQuads");
    }

    public override void OnInspectorGUI()
    {
        int guiSpaceSize = 10;

        ProxyWalkboxes scriptObject = serializedObject.targetObject as ProxyWalkboxes;

        serializedObject.Update();

        //-------------------------------------------------------------
        //mName

        string wboxName = scriptObject.mName;
        uint mNameBlockSize = 4 + (uint)scriptObject.mName.Length; //mName Block Size [4 bytes] //mName block size (size + string len)

        EditorGUILayout.Space(guiSpaceSize);
        EditorGUILayout.LabelField("mName", EditorStyles.whiteLargeLabel);
        EditorGUILayout.LabelField(string.Format("mName_BlockSize: {0}", mNameBlockSize), EditorStyles.boldLabel);
        EditorGUILayout.LabelField(string.Format("mName: {0}", wboxName), EditorStyles.boldLabel);


        //-------------------------------------------------------------
        //mTris

        EditorGUILayout.Space(guiSpaceSize);
        EditorGUILayout.LabelField("mTris", EditorStyles.whiteLargeLabel);
        EditorGUILayout.LabelField(string.Format("mTris_Size: {0}", scriptObject.mTris.Count), EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(mTris);

        //-------------------------------------------------------------
        //mVerts

        EditorGUILayout.Space(guiSpaceSize);
        EditorGUILayout.LabelField("mVerts", EditorStyles.whiteLargeLabel);
        EditorGUILayout.LabelField(string.Format("mVerts_Size: {0}", scriptObject.mVerts.Count), EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(mVerts);

        //-------------------------------------------------------------
        //mNormals

        EditorGUILayout.Space(guiSpaceSize);
        EditorGUILayout.LabelField("mNormals", EditorStyles.whiteLargeLabel);
        EditorGUILayout.LabelField(string.Format("mNormals_Size: {0}", scriptObject.mNormals.Count), EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(mNormals);

        //-------------------------------------------------------------
        //mQuads

        EditorGUILayout.Space(guiSpaceSize);
        EditorGUILayout.LabelField("mQuads", EditorStyles.whiteLargeLabel);
        EditorGUILayout.LabelField(string.Format("mQuads_Size: {0}", scriptObject.mQuads.Count), EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(mQuads);

        if (GUILayout.Button("Export .wbox"))
        {
            scriptObject.ExportWalkbox();
        }

        serializedObject.ApplyModifiedProperties();
    }
}