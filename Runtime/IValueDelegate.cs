/**
  @class  IValueProvider
  @date   16/09/2022
  @author Hellhound

  @brief 
  Generic value provider, to force a component to get/set 
  a specific value type. Use this inteface at components, 
  when you want to access a specific value by interface.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace binrev {
    namespace utils {     
        public interface IValueDelegate<TArg>{
            delegate void ValueDelegate(TArg value);
            //event ValueDelegate OnValueChange; 
        }
    }
}