/**
  @class  RequiredBaseAttributeDrawer
  @date   04/07/2022
  @author Hellhound

  @brief 
  Custom PropertyDrawer for fields marked as RequiredBaseAttribute.

  Renders an ObjectField in Unity Inspector to allow you reference definitons
  for interfaces and abstract classes which is normally not supported by Unity.

  @note
  To get this thing workable, you have to add an UnityEngine.Object to your
  component and mark it by the RequiredBaseAttribute with concrete System.Type
  of your interface of abstract class which should be referenced.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace binrev {
	namespace utils 
	{
		[CustomPropertyDrawer(typeof(RequiredBaseAttribute))]
		public class RequiredBaseAttributeDrawer : PropertyDrawer
		{
			/*
			 * @brief Overrides the Unity OnGUI drawing method to show the ObjectField.
			 * @note  The SerializedProperty value must be the UnityEngine.Object element.
			 * @param position The actual Rectangle position used for UI placement.
			 * @param property The reference to the SerializedProperty value to render.
			 * @param label The label which should be shown in UI.
			 */
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				// Check if this is reference type property.
				if (property.propertyType == SerializedPropertyType.ObjectReference)
				{
					var requiredAttribute = this.attribute as RequiredBaseAttribute;
					if(null!=requiredAttribute){
						EditorGUI.BeginProperty(position, label, property);
						property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, requiredAttribute.requiredType, true);
						EditorGUI.EndProperty();
					}
					else{
						EditorGUILayout.HelpBox("Could not get serialized property value!", MessageType.Error);
					}
				}
				else
				{
					// Display error message in the inspector.
					EditorGUILayout.HelpBox("Property value is not a reference type!", MessageType.Error);
				}
			}
		}
	}
}
#endif