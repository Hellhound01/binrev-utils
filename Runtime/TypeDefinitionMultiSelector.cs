/**
  @class  TypeDefinitionSelector
  @date   20/02/2022
  @author Hellhound

  @brief 
  Wrapper class to extend TypeDefinition with single value selection.
  
  This class combines the embedding of TypeDefinition ScriptableObjects
  with a SingleSelection component, which is automatically updated if
  changes at the ScriptableObject has been performed.
  
  @remark
  This wrapper uses a specific PropertyDrawable to simplyfy the selection
  of both elements as single component in the Unity Inspector.
  
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;
using System.Collections.Generic;

namespace binrev
{
	namespace utils
	{
        [System.Serializable]
		public class TypeDefinitionMultiSelector { 

            [SerializeField]
            private TypeDefinition config;

            /**
			* @brief Property used to get the reference to TypeDefiniton ScriptableObject.
			* @note  This config defines the selectable values for this selector.
			*/
            public TypeDefinition Config { get { return this.config; }}

            [SerializeField]
            private MultiSelection selection = null;

           /**
			* @brief Property used to get the reference to the MultiSelection instance.
			* @note  It handles the selection of the config values and use a PropertyDrawer for rendering.
			*/
            public MultiSelection Selection {get{return this.selection;}}

			[SerializeField]		
            private bool isValid = false;

            /** 
             * @brief Standard Ctor to create a TypeDefinitionMultiSelector instance.
             * @note  This instance used a default Label and tooltip for the MultiSelection field.
             */
            public TypeDefinitionMultiSelector(){
                this.selection = new MultiSelection("Entries", "Select the config value which should be regard.");
            }

            /** 
             * @brief Standard Ctor to create a TypeDefinitionMultiSelector instance with given label and tooltip.
             * @param label The label used for the MultiSelection PopupField.
             * @param tooltip The tooltip used for the MultiSelection PopupField.
             */
            public TypeDefinitionMultiSelector(string label, string tooltip){
                this.selection = new MultiSelection(label, tooltip);
            }

             /**
			* @brief Property used to check if the component is still valid.
			* @note  This selector could be become invalid when the config has been changed.
			*/
            public bool IsValid { get{return this.isValid;} }

            /**
            * @brief OnEnable method used to update settings when used first time.
            * @note  This method must be called explicitly while this class is not a unity object.
            */
            public void OnEnable(){
               UpdateSelection(this.config);
            }

            /** 
            * @brief Return the actual selection value by delegation.
            * @note  This value is "Undefined" if selection is not performed or invalid.
            * @return IEnumerable<string> The acutal selection values. 
            */
            public IEnumerable<string>  Selected {
                get { return this.selection.Selected; }
            }

            /**
             * @brief Checks if the label ist contained in the list of selected values.
             * @return True if label is contained, otherwise false.
             */
            public bool Contains(string value){
                return this.selection.Selected.Contains(value);
            }

            /**
             *  @brief Listener to handle possible TypeDefiniton updates.
             *  @param definition The TypeDefinition with update values.
             */
            public void OnValueUpdate(TypeDefinition definition){
                this.UpdateSelection(definition);
            }

            /**
            * @brief Utility-Metod to handle possible updates of the TypeDefinition 
            *        ScriptableObject and delegate those to the SingleSelection.
            * @param definition The TypeDefinition with update values.
            */
            private void UpdateSelection(TypeDefinition definition){
                 if(null != definition){
                    this.selection.UpdateValues(definition.Labels);
                    this.isValid = this.selection.Selected.Count > 0;
                }
            }
        }
    }
}