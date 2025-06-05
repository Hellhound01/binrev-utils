/**
  @class  TypeDefinitionScriptableEvent
  @date   08/02/2022
  @author Hellhound

  @brief 
  Application specific ScriptableEvent to communicate TypeDefinition value changes.
  
  This class extend the ScriptableEvent implementation of the BinRev
  Scriptable custom package. It uses a custom data object with the
  application specific values of the event. 

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;

using binrev.scriptables;

namespace binrev{
    namespace utils{

        [CreateAssetMenu(fileName="TypeDefinitionEvent", menuName = "BinRev/Scriptables/Event/Type Definition Event", order = 1)]
        public class TypeDefinitionScriptableEvent : ScriptableEvent<TypeDefinition>
        {
                public override void Invoke(TypeDefinition value){
                    base.Invoke(value);
                    base.OnEventRaised?.Invoke(value);
                }
        }
    }
}
