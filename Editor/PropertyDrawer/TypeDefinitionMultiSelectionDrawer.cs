/**
  @class  SingleSelectionPropertyDrawer
  @date   15/09/2021
  @author Hellhound

  @brief 
  Custom PropertyDrawer for SingleSelection property fields.

  Renders the SingleSelection property in Unity Inspector as PopupField
  and allowes Singleple selection of contained values. The selected values
  are shown as list at bottom of the PopupField.

  Selected values are removed dynamically from the PopupFiled until the
  value is deselected by the user.

  @note
  To avoid code duplication in custom editor implementation use a serialized
  property of the SingleSelection in the editor.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace binrev {
    namespace utils{
        /// <summary>
        /// Drawer for the RequireInterface attribute.
        /// </summary>
        [CustomPropertyDrawer(typeof(TypeDefinitionMultiSelector))]
        public class TypeDefinitonMultiSelectionDrawer : PropertyDrawer
        {
            int rows = 1;


            /// <summary>
            /// Overrides GUI drawing for the attribute.
            /// </summary>
            /// <param name="position">Position.</param>
            /// <param name="property">Property.</param>
            /// <param name="label">Label.</param>
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var name = property.FindPropertyRelative("nameID");
                var config = property.FindPropertyRelative("config");
                var selection = property.FindPropertyRelative("selection");
                EditorGUI.BeginProperty(position, label, property);

                if(null != config){

                    this.rows = 1;

                    EditorGUI.BeginChangeCheck();     

                    Rect refRect = new Rect(position.min.x, position.min.y, position.width, EditorGUIUtility.singleLineHeight);      
                    var obj = EditorGUI.ObjectField(refRect, (null!=name) ? name.stringValue : "Type Definition ", config.objectReferenceValue, typeof(TypeDefinition), false) as TypeDefinition;
                    if(obj is null){
                        Rect infoRect = new Rect(position.x, position.y + 20, position.width, EditorGUIUtility.singleLineHeight + 18.0f);
                        EditorGUI.HelpBox(infoRect, "TypeDefinition is invalid! Please add ScriptableObject reference to continue.", MessageType.Error);
                        SetValidationValue(property, false);
                        this.rows = 3;
                        return;
                    }

                    if (EditorGUI.EndChangeCheck()) 
                    {
                        // add ScriptableObject reference as new config
                        config.objectReferenceValue = obj;
                        config.serializedObject.ApplyModifiedProperties();

                        // while values changed, add new values once to SingleSelection
                        List<string> values = new List<string>(obj.Labels);
                        var vList = selection.FindPropertyRelative("values");
                        
                        // clear and redefine array
                        vList.ClearArray();

                        for(int i=0; i<values.Count; i++){
                            vList.InsertArrayElementAtIndex(i);
                            SerializedProperty prop = vList.GetArrayElementAtIndex (i);
                            prop.stringValue = values[i];
                        }

                        // apply list modifications and update SingleSelection property
                        vList.serializedObject.ApplyModifiedProperties();
                        selection.serializedObject.ApplyModifiedProperties();
                    }

                    // always render SingleSelection by itself, only when config is given
                    this.rows = 2;
                    EditorGUI.BeginChangeCheck();
                    Rect selectRect = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight + 2f, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(selectRect, selection);
                    if (EditorGUI.EndChangeCheck()) {
                        // finally set validation for the wrapper to true
                       // SetValidationValue(property, true);
                    }

                     var counter = selection.FindPropertyRelative("count");
                     this.rows += counter.intValue;
                }
                EditorGUI.EndProperty();
            }

             /**
             * @brief Return the PropertyHeight value for the complete drawable content.
             * @note This value regards the actual selected values dynamically.
             * @param property The SerializedProperty of this drawable.
             * @param label The GUIContent label of this drawable.
             */
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                 return base.GetPropertyHeight(property, label) * rows; // + (rows * 1.5f);
            }

            /**
             * @brief Utility Method to set validation value at validation property.
             * @param property The SerializedProperty of this drawable.
             * @param label The GUIContent label of this drawable.
             */
            private void SetValidationValue(SerializedProperty property, bool value){
                var isValid = property.FindPropertyRelative("isValid");
                if(null!=isValid){
                    isValid.boolValue = value;
                    isValid.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
#endif