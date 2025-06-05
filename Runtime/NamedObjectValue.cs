/**
  @class  NamedObjectValue
  @date   25/08/2023
  @author Hellhound

  @brief 
  Generic UnityEnngine.Object value representation based on a name.

  This generic value ensure that the used value always is linked with 
  a name identifier. Use this Type to easily separate values of the 
  same types by name i.e. in a list.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEditor;
using UnityEngine;

namespace binrev
{
    namespace utils
    {
        [System.Serializable]
        public class NamedObjectValue<T> where T : UnityEngine.Object
        {

            [SerializeField] string name;
            public string Name => this.name;

            [SerializeField] T value;
            public T Value => this.value;

            public NamedObjectValue(string Name)
            {
                this.name = Name;
                this.value = default(T);
            }

#if UNITY_EDITOR
            public void DoLayout()
            {
                EditorGUILayout.BeginHorizontal();
                this.value = EditorGUILayout.ObjectField(this.Name, value, typeof(T), false) as T;
                EditorGUILayout.EndHorizontal();
            }
#endif
        }
    }
}
