/**
  @class  SingleSelectionDrawer
  @date   20/02/2022
  @author Hellhound

  @brief 
  Custom PropertyDrawer for SingleSelection property fields.

  Renders the SingleSelection property in Unity Inspector as PopupField
  and allowes a single selection of contained values. .

  @note
  To avoid code duplication in custom editor implementation use a serialized
  property of the SingleSelection in the editor or other PropertyDrawables.

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
        [CustomPropertyDrawer(typeof(SingleSelection))]
        public class SingleSelectionDrawer : PropertyDrawer
        {
            /** 
             * @brief The implementation of the PropertyDrawer OnGUI method used for rendering.
             * @param positon The GUI Rect start values of this drawable.
             * @param property The SerializedProperty of this drawable.
             * @param label The GUIContent label of this drawable.
             */
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);
      
                // determine the title used for this property
                var title = property.FindPropertyRelative("label");

                // detect selectable elements and render those as popup
                var values = new List<string>();
                var pList = property.FindPropertyRelative("values");
                for(int i=0; i< pList.arraySize; i++){
                    values.Add(pList.GetArrayElementAtIndex(i).stringValue);
                }

                // grab actual selection value and identify index based on this value
                var selected = property.FindPropertyRelative("selected");
                int index = values.IndexOf(selected.stringValue);

                /* handle the case that the actual selection is not part of the selection anymore, while
                   values have been changed. In that case reset the selection value to "Undefined" */
                if(index == -1 && selected.stringValue != "Undefined"){
                    Debug.Log("Force reset of selection, while value is not part of selection anymore.");
                    selected.stringValue = "Undefined";
                    selected.serializedObject.ApplyModifiedProperties();
                }

                /* render selection as popup with value change check to ensure that the 
                   setting of the modification is only executed once if this check was
                   successfully */
                EditorGUI.BeginChangeCheck();

               // string[] options = { "Rigidbody", "Box Collider", "Sphere Collider" };
                //int value = EditorGUI.Popup(position, label, index, options);
                
                var value = EditorGUI.Popup(
                    position,
                    label.text,
                    index,
                    values.ToArray());

                if (EditorGUI.EndChangeCheck()) {
                    selected.stringValue = values[value];
                    selected.serializedObject.ApplyModifiedProperties();
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
                 return base.GetPropertyHeight(property, label);
            }
        }
    }
}
#endif