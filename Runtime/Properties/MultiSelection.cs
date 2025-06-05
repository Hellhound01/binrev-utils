/**
  @class  MultiSelection
  @date   16/09/2021
  @author Hellhound

  @brief 
  A MultiSelection property used to select one ore more values 
  from a given list of strings dynamically in your component,
  i.e. to realize a string based filtering.

  @note
  This component uses a PropertyDrawer to extend the functionality
  of a Popup EditorGUI component in the Unity inspector and shows 
  the selected elements as list at bottom of the popup.
  
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;

using System.Collections.Generic;
using System.Linq;

namespace binrev {

    namespace utils {

        [System.Serializable]
        public class MultiSelection
        {
                /** The actual selected type(s) from list */
                [SerializeField]
                protected string label = "Undefined";

               /**
                * @brief Property used for labeling the multi selection in Unity inspector.
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

               /**
                * @brief Property used to define the values which could be selected.
                * @note If a value is selected, it dynamically removed from this list until deselect.
                * @return IList<string> The selected values as readonly list.
                */
                [SerializeField]
                protected List<string> values = new List<string>(); 
                public IList<string> Values {
                    get { return this.values.AsReadOnly(); }
                }
                
                 /**
                * @brief Property used to hold the selected elements of available values.
                * @note If a value is selected, it dynamically removed from the value list until deselect.
                * @return IList<string> The selected elements as readonly list.
                */
                [SerializeField]
                protected List<string> selected = new List<string>();
                public IList<string> Selected {
                    get { return this.selected.AsReadOnly(); }
                }

                /**
                * @brief Property used to define if the available values should be sorted alphabetically.
                * @note This value is set to true on default.
                */
                protected bool sort = true;
                public bool Sort {
                    get { return this.sort; }
                    set { this.sort = value; }
                }

               /**
                * @brief Property used to get the count of selected entries.
                * @note This value is exposed as property to get access in embedded PropertyDrawers.
                */
                [SerializeField]
                private int count;
                public int Count {
                    get { return count; }
                }

               /**
                * @brief Ctor to create a MultiSelection element with label definiton.
                * @note The label is used as Inspector label for this selection.
                */
                public MultiSelection(string label){
                    if(null!=label && label.Length > 0) this.label = label;
                }

               /**
                * @brief Ctor to create a MultiSelection element with label and tooltip definiton.
                * @note The values are used as Inspector label and tooltip for this selection.
                */
                public MultiSelection(string label, string tooltip){
                    if(null!=label && label.Length > 0) this.label = label;
                    if(null!=tooltip && tooltip.Length > 0) this.tooltip = tooltip;
                }

                /**
                * @brief Ctor to create a MultiSelection element with label and tooltip definiton.
                * @note The values are used as Inspector label and tooltip for this selection.
                */
                public MultiSelection(string label, string tooltip, List<string> values){
                    if(null!=label && label.Length > 0) this.label = label;
                    if(null!=tooltip && tooltip.Length > 0) this.tooltip = tooltip;
                    UpdateValues(values);
                }

               /**
                * @brief Ctor to create a MultiSelection based on given MultiSelection.
                * @note Use this ctor to create a flat copy of the given MultiSelection.
                */
                public MultiSelection(MultiSelection other){
                    if(null!=other){
                        this.label = other.label;
                        this.tooltip = other.tooltip;
                        this.selected = other.selected;
                        this.values = other.values;
                    }
                }

               /** 
                *@brief Update the selectable values by setting a new list.
                *
                * This method holds already selected values if those are still part of
                * the new values, otherwise the selection is removed.
                *
                *@param values The list of string with new selectable entires.
                */
                public void UpdateValues(IList<string> values){
                    if(values!=null){
                        this.values = values.Except(selected).ToList();
                    }
                }

                 /** 
                *@brief Update the selectable values by given MultiSelection reference.
                *@param other The MultiSelection reference to copy values from.
                */
                public void UpdateValues(MultiSelection other){
                    if(null!=other){
                        this.label = other.label;
                        this.tooltip = other.tooltip;
                        this.selected = other.selected;
                        this.values = other.values;
                        this.count = other.count;
                    }  
                    else{
                        Debug.LogWarning("[MultiSelection]: UpdateValues: The required reference object is invalid!");
                    }
                }

                /** 
                *@brief Mark a value entry as selected.
                *@note  Up from this point the type name is not again selectable.
                *@return bool True if selection was successfull, otherwise false.
                */
                public bool SetSelected(string value) {
                    bool result = false;
                    if (this.values.Contains(value)) {
                        var index = this.values.IndexOf(value);
                        this.selected.Add(this.values[index]);
                        this.values.Remove(value);
                        this.count = selected.Count;
                        result = true;
                    }
                    return result;
                }

                /** 
                *@brief Remove an entry from list of selected elements.
                *@note Remove the select marker and make this entry selectable again.
                *@param value The string of the selected type to remove.
                *@return bool True if remove was successfull, otherwise false.
                */
                public bool RemoveSelected(string value){
                    bool result = false;
                    if(this.selected.Contains(value)){
                        this.values.Add(value);
                        this.selected.Remove(value);
                        if(this.sort) this.values.Sort();
                        this.count = selected.Count;
                        result = true;	
                    }
                    return result;
                }

                /** 
                *@brief Remove an entry from list of selected elements.
                *@note Remove the select marker and make this entry selectable again.
                *@param value The string of the selected type to remove.
                */
                public void RemoveAllSelected(){
                    for(int i = this.selected.Count-1; i > -1; i--){
                        this.RemoveSelected(this.selected[i]);
                    }
                    this.count = selected.Count;
                }

                /** 
                *@brief Checks if a given string is part of the actual selected values.
                *@param value The string used to check if selected.
                *@return bool True if string is contained in selected values, otherwise false.
                */
                public bool IsSelected(string value){
                    if(this.selected.Count > 0){
                       return this.selected.Contains(value); 
                    }
                    return false;
                }
        }
    }
}