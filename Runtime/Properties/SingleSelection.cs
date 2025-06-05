/**
  @class  SingleSelection
  @date   16/09/2021
  @author Hellhound

  @brief 
  A SingleSelection property used to select one value from a given 
  list of strings dynamically in your component, i.e. to realize a 
  string based filtering.

  @note
  This component uses a PropertyDrawer to extend the functionality
  of a Popup EditorGUI component in the Unity inspector and shows 
  the selected elements as list at bottom of the popup.
  
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using System.Collections.Generic;
using UnityEngine;

namespace binrev {

    namespace utils {

        [System.Serializable]
        public class SingleSelection
        {
            [SerializeField]
            protected string label = "Undefined";

            /**
            * @brief Property used for labeling the Single selection in Unity inspector.
            * @note If this value is not set explicitly undefined is shown by PropertyDrawer.
            */
            public string Label {
                get { return label; }
                set { 
                    this.label = value; 
                }
            }

            [SerializeField]
            protected string tooltip = "";

                /**
            * @brief Property used to show a tooltip in Unity inspector.
            * @note If this value is not set explicitly nothing is shown by PropertyDrawer.
            */
            public string Tooltip {
                get { return this.tooltip; }
                set { 
                    this.tooltip = value; 
                }
            }

            /* List of available values as string */
            [SerializeField]
            protected List<string> values = new List<string>(); 

                /**
            * @brief Property used to get available values.
            * @return IList<string> Available values as readonly list.
            */
            public IList<string> Values {
                get { return this.values.AsReadOnly(); }
            }
            
            
            [SerializeField]
            protected string selected = "Undefined";

            /**
            * @brief Property used to hold the selected value from list.
            */

            public string Selected {
                get { return this.selected; }
            }

            public SingleSelection() { }

            public SingleSelection(string label){
                if(null!=label && label.Length > 0) this.label = label;
            }

            public SingleSelection(string label, string tooltip){
                if(null!=label && label.Length > 0) this.label = label;
                if(null!=tooltip && tooltip.Length > 0) this.tooltip = tooltip;
            }

            public SingleSelection(SingleSelection other){
                if(null!=other){
                    this.label = other.label;
                    this.tooltip = other.tooltip;
                    this.selected = other.selected;
                    this.values = other.values;
                }
            }

            /** 
            *@brief Update the selectable values by setting a new list.
            *@note  If the actual selection is not part of the new values, the selection is reset to "Undefined".
            *@param values The list of string with new selectable entires.
            */
            public void UpdateValues(IList<string> values){
                if(values!=null && values.Count > 0 && this.values != values){
                    this.values = new List<string>(values);
                    if(false == this.values.Contains(selected)) this.selected = "Undefined";
                }
            }

                /** 
            *@brief Update the selectable values by given SingleSelection reference.
            *@param other The SingleSelection reference to copy values from.
            */
            public void UpdateValues(SingleSelection other){
                if(null!=other){
                    this.label = other.label;
                    this.tooltip = other.tooltip;
                    this.selected = other.selected;
                    this.values = other.values;
                }  
                else{
                    Debug.LogWarning("[SingleSelection]: UpdateValues: The required reference object is invalid!");
                }
            }

            /** 
            *@brief Mark a value entry as selected.
            *@return bool True if selection was successfull, otherwise false.
            */
            public bool SetSelected(string value) {
                bool result = false;
                if (this.values.Contains(value)) {
                    this.selected = value;
                    result = true;
                }
                return result;
            }

            public void Clear(){
                this.values.Clear();
                this.selected = "Undefined";
            }
        }
    }
}