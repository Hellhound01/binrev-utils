/**
  @class  TypeDefinitionEditor
  @date   05/09/2017
  @author Hellhound

  @brief 
  Custom UnityEditor for TypeDefinition objects.
  
  An TypeDefinition is a Unity ScriptableObject to hold TypeDefinition type
  informations in an enumeration like behaviour. This editor
  is used to create an Instance of this ScriptableObject and
  perform value settings dynamically at design time.
  
  @remark
  The list with the TypeDefinition string elements used as enumeration
  values is declared private to avoid manipulaton from code at
  runtime. By this reason we have to use the SerializedProperty
  of the TypeDefinition to get access to this list from this custom
  editor.
  
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using binrev.scriptables;
using binrev.utils;

namespace binrev
{
	namespace customizer
	{

		[CustomEditor(typeof(TypeDefinition))]
		public class TypeDefinitionsEditor : Editor
		{
			/* Field to store TypeDefinition input before added to TypeDefinition*/
			private string typeText = null;
			
			/* Reference to the labels of the TypeDefinition to add defined types.*/
			private SerializedProperty property;

			private SerializedProperty nameID;
			
			/**
			* @brief Load the labels as SerializedProperty at start.
			* @throws UnassignedReferenceException if TypeDefinition labels property could not found;
			*/	
			void OnEnable(){	
				SerializedObject obj = serializedObject;
				nameID = serializedObject.FindProperty("nameID");
				property = serializedObject.FindProperty("labels");
				if(property == null){
					throw new UnassignedReferenceException("TypeDefinition Label-List could not be found!");		
				}	
			}
			
			/**
			* @brief Renders the UI frontend of the editor to fill the Asset with values.
			*/	
			public override void OnInspectorGUI()
			{
				// always update the serializedObject each frame
				serializedObject.Update();
				
				var TypeDefinition = target as TypeDefinition;	

				EditorGUI.BeginChangeCheck(); 
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(this.nameID, new GUIContent("Name: ", "The name used as Popup-List label"));
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck()) {
					serializedObject.ApplyModifiedProperties();
				}
				
				var definition = this.target as TypeDefinition;

				EditorGUILayout.BeginHorizontal();
				definition.valueChangeEvent = EditorGUILayout.ObjectField(new GUIContent("Update Event: ", "Reference to TypeDefiniton which contains the used types."), definition.valueChangeEvent, typeof(TypeDefinitionScriptableEvent), false) as TypeDefinitionScriptableEvent;
				EditorGUILayout.EndHorizontal();
				
				if(null == definition.valueChangeEvent){
					EditorGUILayout.BeginHorizontal();
				    EditorGUILayout.HelpBox("Update event invalid! Add ScirptableEvent reference!", MessageType.Error);
					EditorGUILayout.EndHorizontal();
					return;
				}

				EditorGUILayout.BeginHorizontal();
				typeText = EditorGUILayout.TextField("Enter type to create: ", typeText);
				if (GUILayout.Button(new GUIContent("+", "Add a new selectable type entry"), EditorStyles.miniButton, GUILayout.Width(20)))
				{ 
					// expand the array of the serialied property append list entry
					int size = property.arraySize;
					property.InsertArrayElementAtIndex(size);
					property.GetArrayElementAtIndex(size).stringValue = typeText;
					serializedObject.ApplyModifiedProperties();
					typeText = null;
					definition.valueChangeEvent?.Invoke(definition);
				}
				EditorGUILayout.EndHorizontal();
				
				string entryToDelete = null;
				foreach(var entry in TypeDefinition.Labels){
					EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(" ", entry, EditorStyles.textField);
					if (GUILayout.Button(new GUIContent("-", "Remove this type entry"), EditorStyles.miniButton, GUILayout.Width(20))){
						entryToDelete = entry;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical();
				}        

			   /* delete the entry in TypeDefinition list at latest point, when the iteration of
				* the list has been completed to avoid list out of sync exceptions while 
				* looping through the list.		 
				*/
				if (entryToDelete!=null){
					int index = GetIndex(entryToDelete);
					if(index > -1){
						property.DeleteArrayElementAtIndex(index);
						serializedObject.ApplyModifiedProperties();
						definition.valueChangeEvent?.Invoke(definition);
					}
				}
				// mark editor explicitly as dirty to avoid loosing data on close
				if (GUI.changed){
					EditorUtility.SetDirty(TypeDefinition);
				}
			}
			
			/** 
			* @breif Utility method to find index of TypeDefinition list entry.
			* @remark Returns -1 if entry could not found in list.
			* @param type The type string to search for
			* @return int The index of the entry in list or -1.
			*/
			private int GetIndex(string entryToDelete){
				for(int i=0; i<property.arraySize; i++){
					if(property.GetArrayElementAtIndex(i).stringValue == entryToDelete){
						return i;
					}
				}
				return -1;
			}
		}
	}
}
#endif