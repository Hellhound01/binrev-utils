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

        [CustomPropertyDrawer(typeof(TypeDefinitionSelector))]
        public class TypeDefinitonSelectorDrawer : PropertyDrawer
        {
            int rows = 1;

            static GUIContent CONFIG_LABEL = new GUIContent("Config", "The TypeDefinition ScriptableObject reference this selector depends on");
                   


            /// <summary>
            /// Overrides GUI drawing for the attribute.
            /// </summary>
            /// <param name="position">Position.</param>
            /// <param name="property">Property.</param>
            /// <param name="label">Label.</param>
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var config = property.FindPropertyRelative("config");
                var selection = property.FindPropertyRelative("selection");
                EditorGUI.BeginProperty(position, label, property);

                if(null != config){

                    this.rows = 1;

                    EditorGUI.BeginChangeCheck();     

                    Rect refRect = new Rect(position.min.x, position.min.y, position.width, EditorGUIUtility.singleLineHeight);  

                    TypeDefinition type = config.objectReferenceValue as TypeDefinition;
                    var obj = EditorGUI.ObjectField(refRect, (null!=type) ? type.NameID : "Type Definiton Reference", config.objectReferenceValue, typeof(TypeDefinition), false) as TypeDefinition;
                    if(obj is null){
                        Rect infoRect = new Rect(position.x, position.y + 20, position.width, GUI.skin.textField.lineHeight + 32f);
                        EditorGUI.HelpBox(infoRect, "TypeDefinition is invalid! Please add ScriptableObject reference to continue.", MessageType.Error);
                        SetValidationValue(property, false);
                        this.rows = 3;
                        return;
                    }

                    if (EditorGUI.EndChangeCheck()) {

                        // if config reference has changed deregister event listener
                        if(config.objectReferenceValue!=null){
                            var oldConfig = config.objectReferenceValue as TypeDefinition;
                            var selector = selection.objectReferenceValue;
                        }

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
                        SetValidationValue(property, true);
                    }
                    
                    // check if selection value is valid, otherwise show message and stop rendering
                    var selected = selection.FindPropertyRelative("selected");
                    if(selected.stringValue == "Undefined"){
                        Rect infoRect = new Rect(position.x, selectRect.y + 20, position.width, GUI.skin.textField.lineHeight + 22f);
                        EditorGUI.HelpBox(infoRect, "Selection is invalid! Please perform selection to continue.", MessageType.Error);
                        SetValidationValue(property, false);
                        this.rows = 4;
                        return;
                    }
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
                 return base.GetPropertyHeight(property, label) * rows;
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