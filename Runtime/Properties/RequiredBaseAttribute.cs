/**
  @class  RequiredBaseAttribute
  @date   04/07/2022
  @author Hellhound

  @brief 
  PropertyAttribute definiton for required base types.
  
  This is the Unity PropertyAttribute defintion which you have to
  use in your components, which should resolve interfaces and abstract
  classes in the inspector.

  @note
  This component uses a PropertyDrawer to show the attribute reference
  value as ObjectField in the inspector.
    
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;

namespace binrev {
	namespace utils 
	{
		public class RequiredBaseAttribute : PropertyAttribute
		{
			// The type definition of your concrete base class implementation
			public System.Type requiredType { get; private set; }

			/**
			 * @brief Contructor which expect the referenced base class type as parameter.
			 * @param type The System.Type of the base class which should be resolved.
			 * @throws UnassignedReferenceException if the required type is invalid.
			 */
			public RequiredBaseAttribute(System.Type type)
			{
				if(null== type) throw new UnassignedReferenceException("[RequiredBaseAttribute]: Ctor: Ivalid type parameter!");
				this.requiredType = type;
			}
		}
	}
}