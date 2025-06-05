/**
  @class  IValueReciver
  @date   16/09/2022
  @author Hellhound

  @brief 
  Generic value reciver, to force a component to set 
  a specific value type. Use this inteface at components, 
  when you want to access a specific value by interface.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;

namespace binrev
{
	namespace utils
	{
        public interface IValueReciver<TArg>{
            void SetValue(TArg value);
        }
    }
}