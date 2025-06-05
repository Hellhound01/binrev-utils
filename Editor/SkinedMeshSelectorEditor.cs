/**
  @class  SkinnedMeshSelectorEditor
  @date   22/08/2023
  @author Hellhound

  @brief 
  Selector to change SkinnedMeshRenderer Mesh data.
  
  This selector uses the SkinDataSetSO configuration based on a
  ScriptableObject to define the selectable skins, which could
  be changed at Editor and Runtime.

  @remark
  A SkinDataSetSO definition could be contain multimesh values. 
  In that case this selector also allowes to change between 
  those values too.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace binrev
{
    namespace utils
    {
        [CustomEditor(typeof(SkinnedMeshSelector))]
        public class SkinnedMeshSelectorEditor : Editor
        {
            SerializedProperty skins;
            SerializedProperty skinSelector;
            SerializedProperty meshSelector;

            SkinnedMeshSelector skinnedMeshSelector;

            static GUIContent LABEL_SKIN_DATA = new GUIContent("Skin Data", "The reference to Skin-Set ScriptableObject definitions. ");
            static GUIContent LABEL_SKIN_SELECTOR = new GUIContent("Selected Skin", "The actual aktive Skin of the referenced Skin Data Set.");
            static GUIContent LABEL_MESH_SELECTOR = new GUIContent("Selected Mesh", "The actual aktive Mesh of the active multimesh based Skin.");

            private void OnEnable()
            {
                this.skinnedMeshSelector = target as SkinnedMeshSelector;
                this.skinnedMeshSelector.Renderer = this.skinnedMeshSelector.gameObject.GetComponent<SkinnedMeshRenderer>();


                //this.skinnedMeshSelector.UpdateData();

                this.skins = serializedObject.FindProperty("skins");
                this.skins = serializedObject.FindProperty("skins");
                this.skinSelector = serializedObject.FindProperty("skinSelector");
                this.meshSelector = serializedObject.FindProperty("meshSelector");
            }


            /**
             * @brief Unity Editor rendering method, called each frame to show the ReorderableLists and KeyPool settings in the inspector.
             */
            public override void OnInspectorGUI()
            {
                GUI.enabled = false;
                SerializedProperty prop = serializedObject.FindProperty("m_Script");
                EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
                GUI.enabled = true;

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.skins, LABEL_SKIN_DATA);
                EditorGUILayout.EndHorizontal();
                if (true == EditorGUI.EndChangeCheck())
                {
                    this.serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.skinSelector, LABEL_SKIN_SELECTOR);
                EditorGUILayout.EndHorizontal();
                if (true == EditorGUI.EndChangeCheck())
                {
                    this.serializedObject.ApplyModifiedProperties();
                    this.skinnedMeshSelector.UpdateData();
                }

                var dataSet = this.skins.boxedValue as SkinDataSetSO;
                if (null != dataSet)
                {
                    if (dataSet.UseMultiMesh)
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(this.meshSelector, LABEL_MESH_SELECTOR);
                        EditorGUILayout.EndHorizontal();
                        if (true == EditorGUI.EndChangeCheck())
                        {
                            this.serializedObject.ApplyModifiedProperties();
                            this.skinnedMeshSelector.UpdateData();
                        }
                    }
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Skin: " + this.skinnedMeshSelector.skinID);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Mesh: " + this.skinnedMeshSelector.activeMeshID);
                EditorGUILayout.EndHorizontal();

                // mark editor explicitly as dirty to avoid loosing data on close
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
#endif
