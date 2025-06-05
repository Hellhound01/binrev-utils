/**
  @class  Restriction
  @date   08/04/2022
  @author Hellhound

  @brief 
  Generic interface for restriction definitions.

  A restriction is a kind of rule used to check at runtime for a given
  Type represented by the generic in TArg value.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;

#if UNITY_EDITOR
 using UnityEditor;
#endif

namespace binrev
{
	namespace utils
	{    
        public interface IRestriction<in TArg>
        {
            /** 
              * @brief Delegator method invoked by Unity OnEnable calls.
              */
            void OnEnable();

            /** 
              * @brief Return the name identifier for the restriction.
              * @return string The none unique name ID.
              */
            string GetNameID();

            /** 
              * @brief Validation method to handle restriction check.
              * @param comparable The comparable used to check.
              * @return bool True if check was successfully passed, otherwise false.
              */
            bool Validate(TArg comparable);

          #if UNITY_EDITOR
            public void DoLayout(bool showInfo = false);
          #endif
        }
    }
}