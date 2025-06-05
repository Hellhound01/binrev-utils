using UnityEngine;

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace binrev
{
	namespace utils
	{
		public static class Reflection {

			static List<string> Assemblies = new List<string>();

			/**
			* @brief Return a List of any concrete Type which implments T.
			* @return List<Type> The list of concrete implementations of Type T. 
			*/	
			public static List<Type> GetAnyTypeThatImplements<T>()
			{
				var root = Assembly.GetCallingAssembly();
				var	rootName = root.FullName;

				if(Assemblies.Count == 0){
					
					var visited = new HashSet<string>();

					Queue<Assembly> queue = new Queue<Assembly>();
					if(null!=root) queue.Enqueue(root);

					while(queue.Any()){
						var assembly = queue.Dequeue();
						Assemblies.Add(assembly.FullName);

						var references = assembly.GetReferencedAssemblies();
						foreach(var reference in references){
							var name = reference.FullName;
							var check = name.Contains("Unity");

							if(!name.Contains("Unity") && !name.Contains("System") && !name.Contains("mscorlib")){
								if(!Assemblies.Contains(reference.FullName)){
									Assemblies.Add(reference.FullName);
								}
							}
						}
					}
				}

				// First try to detect requested types from actual root assembly
				List<Type> types = GetAnyTypeFromAssembly<T>(rootName);
				
				// Next try to detect type from Unity default assembly where actual application scripts are compiled
				//types.AddRange(GetAnyTypeFromAssembly<T>("Assembly-CSharp"));

                foreach (var name in Assemblies)
                {
                    if (name == rootName) continue;
                    var assembly = Assembly.Load(name);
                    if (null != assembly)
                    {
                        var list = GetAnyTypeFromAssembly<T>(name);
                        foreach (var item in list){
                            if (!types.Contains(item)){
                                types.Add(item);
                            }
                        }
                    }
                }

                return types.Where(t => false == t.IsInterface && false == t.IsAbstract).ToList();
			}
			
			
			public static List<Type> GetAnyTypeFromAssembly<T>(string name){
				List<Type> result = new List<Type>();
				if(name!=null){
					var assembly = Assembly.Load(name);
					if(null!=assembly){
						result = assembly.GetTypes().Where(
									t => typeof(T).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsInterface
						).ToList();
                    }
				}
				return result;
			}

			public static void GetFieldValue<T>(string name){
				// Get the type of FieldsClass.
        		Type type = typeof(T);
				if(null!=type){
					// Get an array of FieldInfo objects.
        			FieldInfo[] fields = type.GetFields(BindingFlags.Public);
					int x = 0;
				}
			}
		}
	}
}