/**
  @class  TypeDefinition
  @date   18/02/2022
  @author Hellhound

  @brief 
  SriptableObject implementation for item types as enum style.
  
  Enumerations are static values defined in code. Any change would
  result in code changes. This class is used to define types for 
  the item objects in an enumeration like manner dynamically at
  design time if required, without changing a line of code. 
  
  @remark
  This class supports single value and multiple value selection
  of the defined entries, controlled by the multiselect flag and 
  visualized by self when rendering in the UnityEditor. 

  @note
  In code you have to load the created asset of this implementation
  to get access to the defined elements.

  @note
  While this class also holds the information about the selected element
  of the list, you always have to create a new instance of this class on
  usage, otherwise the values will be shared between different object
  instances.
  
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using binrev.scriptables;

namespace binrev
{
	namespace utils
	{
		[CreateAssetMenu(fileName="TypeDefinition", menuName = "BinRev/Utils/Type Definition", order = 1)]
		public class TypeDefinition : ScriptableObject
		{	
			/** Name identifier to seperate TypeDefinitions*/
			[SerializeField] string nameID = "Config";
			public string NameID {get{return this.nameID;}}

			[SerializeField]
			public TypeDefinitionScriptableEvent valueChangeEvent;
			
			/* List of available types as string */
			[SerializeField]
			protected List<string> labels = new List<string>();
				
			/** 
			*@brief Return the available types.
			*@return IList<string> The types as readonly elements.
			*/
			public IList<string> Labels { get { return this.labels.AsReadOnly();} }

			/** 
			*@brief Compares if other TypeDefinition is equals to this item.
			*@return bool True if other TypeDefinition is equals, otherwise false.
			*/
			public bool Equals(TypeDefinition other)
			{
				if(Object.ReferenceEquals(null, other)) return false; 
				if(!this.labels.OrderBy(t=>t).SequenceEqual(other.labels.OrderBy(t=>t))) return false;
				return true;
			}

			/**
            *  @brief Check if given string value is contained in available selection values.
            *  @return True if contained, otherwise false.
            */
            public bool Contains(string value)
            {
                return this.labels.Contains(value);
            }
		}
	}
}