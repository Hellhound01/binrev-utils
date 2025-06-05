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

namespace binrev
{
	namespace utils
	{
        [System.Serializable]
		public class TypeDefinitionSelector { 

            [SerializeField]
            private TypeDefinition config;
            public TypeDefinition Config { get { return this.config; }}

            [SerializeField]
            private SingleSelection selection = null;
            public SingleSelection Selection {get{return this.selection;}}

			[SerializeField]		
            private bool isValid = false;
            public bool IsValid { get{return this.isValid;} }

            /** 
             * @brief Standard Ctor to create a TypeDefinitionSelector instance.
             * @note  This instance used a default Label and tooltip for the Selection PopupField.
             */
            public TypeDefinitionSelector(){
                this.selection = new SingleSelection("Type", "The type of the entity");
            }

            /** 
             * @brief Standard Ctor to create a TypeDefinitionSelector instance with given label and tooltip.
             * @param label The label used for the PopupField.
             * @param tooltip The tooltip used for the PopupField.
             */
            public TypeDefinitionSelector(string label, string tooltip){
                this.selection = new SingleSelection(label, tooltip);
            }

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
            * @return string The acutal selection value. 
            */
            public string Selected {
                get { return this.selection.Selected; }
            }

            /**
            *  @brief Check if given string value is contained in available selection values.
            *  @return True if contained, otherwise false.
            */
            public bool Contains(string value)
            {
                return this.config.Contains(value);
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
                    this.isValid = (this.selection.Selected == "Undefined") ? false : this.isValid;
                }
            }
        }
    }
}