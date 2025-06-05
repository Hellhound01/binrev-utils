/**
  @class  SkinDataSetSOEditor
  @date   21/08/2023
  @author Hellhound

  @brief 
  Custom Editor for SkinDataSetSO definitions.
  
  This editor controls the process of the creation of SkinSet definition 
  values. Each Skin-Set definition is based on a unique Name-ID and [1..n]
  Mesh value definition used by a SkinnedMeshRender for visualization.

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

        [CustomEditor(typeof(SkinDataSetSO))]
        public class SkinDataSetSOEditor : Editor
        {
            SerializedProperty multiMeshSupport;
            SerializedProperty onSelectEvent;
            SerializedProperty multiMeshSelector;

            SkinDataSetSO data;
            string skinID = string.Empty;

            private void OnEnable()
            {
                this.multiMeshSupport = serializedObject.FindProperty("multiMeshSupport");
                this.onSelectEvent = serializedObject.FindProperty("onSelectEvent");
                this.multiMeshSelector = serializedObject.FindProperty("multiMeshSelector");
                this.data = target as SkinDataSetSO;
            }

            /**
             * @brief Unity Editor rendering method, called each frame to show the ReorderableLists and KeyPool settings in the inspector.
             */
            public override void OnInspectorGUI()
            {
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                SerializedProperty prop = serializedObject.FindProperty("m_Script");
                EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
                GUI.enabled = true;
                if (GUILayout.Button(new GUIContent("M", "(De-)Activate multi mesh support."), EditorStyles.miniButton, GUILayout.Width(22)))
                {
                    this.multiMeshSupport.boolValue = !this.multiMeshSupport.boolValue;

                    // if multi mesh support is disabled reset any propery value 
                    if (false == this.multiMeshSupport.boolValue)
                    {
                        if (null != this.onSelectEvent.objectReferenceValue) this.onSelectEvent.objectReferenceValue = null;
                        var config = this.multiMeshSelector.FindPropertyRelative("config");
                        if (config != null) config.objectReferenceValue = null;
                    }

                    this.serializedObject.ApplyModifiedProperties();
                }
                EditorGUILayout.EndHorizontal();

                if (this.multiMeshSupport.boolValue)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(this.multiMeshSelector);
                    EditorGUILayout.EndHorizontal();
                    if (EditorGUI.EndChangeCheck())
                    {
                        this.serializedObject.ApplyModifiedProperties();
                    }

                    // check if selector is valid, otherwise return to stop rendering
                    TypeDefinitionSelector types = multiMeshSelector.boxedValue as TypeDefinitionSelector;
                    if (null == types || null == types.Config) return;
                }

                EditorGUILayout.BeginHorizontal();
                this.skinID = EditorGUILayout.TextField("Add Skin Set", this.skinID);
                if (GUILayout.Button(new GUIContent("+", "Create new SkinSet with given Name."), EditorStyles.miniButton, GUILayout.Width(22)))
                {
                    if (!string.IsNullOrEmpty(this.skinID))
                    {
                        if(false == this.data.Contains(this.skinID)){
                            this.data.CreateEntry(this.skinID);
                            this.skinID = string.Empty;
                        }
                        else
                        {
                            Debug.LogWarning("NameID for the new Skin-Set to create is already in use!");
                        }
                    }
                    else Debug.LogWarning("NameID for the new Skin-Set to create is invalid!");
                }
                EditorGUILayout.EndHorizontal();

                RenderEntries();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(this.data);
                }
            }

            /**
             * @brief Utility method to render the already created Skin-Set entries.
             * @remark This method perform a loop through given values, the rendering of the Entries is done by the entry itself.
             */
            private void RenderEntries()
            {
                if (this.data.entries.Count > 0)
                {

                    EditorGUILayout.Space(0.2f);
                    EditorGUILayout.LabelField("Skin-Data Entries ");
                    foreach (SkinDataSetSO.Entry entry in this.data.entries)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(entry.Name);
                        if (GUILayout.Button(new GUIContent("-", "Remove Entry"), EditorStyles.miniButton, GUILayout.Width(22)))
                        {
                            data.entries.Remove(entry);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();

                        // render each entry by itself
                        entry.DoLayout();

                        EditorGUILayout.EndVertical();
                    }
                }
            }
        }
    }
}
#endif
