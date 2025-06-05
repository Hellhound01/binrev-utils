/**
  @class  MultiSelectionDrawer
  @date   15/09/2021
  @author Hellhound

  @brief 
  Custom PropertyDrawer for MultiSelection property fields.

  Renders the MultiSelection property in Unity Inspector as PopupField
  and allowes multiple selection of contained values. The selected values
  are shown as list at bottom of the PopupField.

  Selected values are removed dynamically from the PopupFiled until the
  value is deselected by the user.

  @note
  To avoid code duplication in custom editor implementation use a serialized
  property of the MultiSelection in the editor.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace binrev {
    namespace utils
	{
        [CustomPropertyDrawer(typeof(MultiSelection))]
        public class MultiSelectionDrawer : PropertyDrawer
        {
            // reference of the acutal index of selectable values in popup field
            int index = 0;

            /** 
             * @brief The implementation of the PropertyDrawer OnGUI method used for rendering.
             * @param positon The GUI Rect start values of this drawable.
             * @param property The SerializedProperty of this drawable.
             * @param label The GUIContent label of this drawable.
             */
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                var countSelected = property.FindPropertyRelative("count");

                Rect popupRect = new Rect(position.x, position.y, position.width - 22, GUI.skin.textField.lineHeight + 18);

                // determine the title used for this property
                var title = property.FindPropertyRelative("label");
                var t = title.stringValue;

                // detect selectable elements and render those as popup
                var values = new List<string>();
                var pList = property.FindPropertyRelative("values");
                for(int i=0; i< pList.arraySize; i++){
                    values.Add(pList.GetArrayElementAtIndex(i).stringValue);
                }

                this.index = EditorGUI.Popup(popupRect, title.stringValue, this.index, values.ToArray());
                GUI.enabled = (values.Count > 0);


                Rect buttonRect = new Rect(position.width-2, position.y, 20, GUI.skin.textField.lineHeight+3);

                /* detect actual selected values and add a + button for selection. If this button is pressed
                   the list of actual selected values is expanded and the available values reduced dynamically. */
                var sList = property.FindPropertyRelative("selected");
                var selected = new List<string>();
                if(GUI.Button(buttonRect, "+"))
                { 	
                    sList.InsertArrayElementAtIndex(sList.arraySize);
                    SerializedProperty prop = sList.GetArrayElementAtIndex (sList.arraySize - 1);
                    string value = values[this.index];
                    prop.stringValue = value;
                    pList.DeleteArrayElementAtIndex(this.index);

                    pList.serializedObject.ApplyModifiedProperties();
                    sList.serializedObject.ApplyModifiedProperties();
                    this.index = -1;
                }
                GUI.enabled = true;

                Rect selectedRect = new Rect(position.x, position.y + 20, position.width - 22, GUI.skin.textField.lineHeight + 3);

                /* render actual selected values and add a - button for deselection. If this button is pressed
                   the list of actual selected values is reduced and the available values expanded dynamically. */
                for(int i=0; i< sList.arraySize; i++){
                    var value = sList.GetArrayElementAtIndex(i).stringValue;

                    // render selected entry as uneditable label with textfield layout style
                    EditorGUI.LabelField(selectedRect, " ", value, EditorStyles.textField);
                    if(GUI.Button(new Rect(position.width-2, selectedRect.y, 20, GUI.skin.textField.lineHeight+3), "-"))
                    { 	
                        pList.InsertArrayElementAtIndex(pList.arraySize);
                        SerializedProperty prop = pList.GetArrayElementAtIndex (pList.arraySize - 1);
                        prop.stringValue = value;
                        sList.DeleteArrayElementAtIndex(i);

                        pList.serializedObject.ApplyModifiedProperties();
                        sList.serializedObject.ApplyModifiedProperties();
                    }
                    selectedRect.y += 20; 
                }

                // store actual number of selected elements for height value computation
                countSelected.intValue = sList.arraySize;
                property.serializedObject.ApplyModifiedProperties();

                EditorGUI.indentLevel = indent;
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
                 var countSelected = property.FindPropertyRelative("count");
                 return base.GetPropertyHeight(property, label) + countSelected.intValue * 20;
            }
        }
    }
}
#endif