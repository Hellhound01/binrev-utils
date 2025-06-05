/**
  @class  RangeAttribute
  @date   07/10/2020
  @author Hellhound

  @brief 
  Unity PropertyAttribute implementation for a range [min,max].
  
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using UnityEngine;

using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace binrev {
    namespace utils
	{
        public class RangeAttribute : PropertyAttribute {

            public float min;
            public float max;

            public string tooltip;

            public RangeAttribute(float min, float max)
            {
                this.min = min;
                this.max = max;
                this.tooltip = "Range: " + min + " to " + max;
            }

            public RangeAttribute(float min, float max, string tooltip)
            {
                this.min = min;
                this.max = max;
                this.tooltip = tooltip;
            }

            #if UNITY_EDITOR
            public static  RangeAttribute GetRangeAttribute(SerializedProperty prop, bool inherit) 
                        {
                            if (prop == null) { return null; }
                        
                            Type t = prop.serializedObject.targetObject.GetType();
                        
                            FieldInfo f = null;
                            PropertyInfo p = null;
                            foreach (var name in prop.propertyPath.Split('.')) {
                                f = t.GetField(name, (BindingFlags)(-1));
                        
                                if (f == null) {
                                    p = t.GetProperty(name, (BindingFlags)(-1));
                                    if (p == null) {
                                        return null;
                                    }
                                    t = p.PropertyType;
                                } else {
                                    t = f.FieldType;
                                }
                            }
 
                            RangeAttribute[] attributes;
                        
                            if (f != null) {
                                attributes = f.GetCustomAttributes(typeof(RangeAttribute), inherit) as RangeAttribute[];
                            } else if (p != null) {
                                attributes = p.GetCustomAttributes(typeof(RangeAttribute), inherit) as RangeAttribute[];
                            } else {
                                return null;
                            }
                                return attributes.Length > 0 ? attributes[0] : null;
                        }
            #endif
        }  
    }
}