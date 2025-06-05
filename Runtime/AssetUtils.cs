using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace binrev
{
	namespace utils
	{
		public static class AssetUtils {

		#if UNITY_EDITOR	

			/**
			 * @brief Return the path of an given Unity Asset object reference.
			 * @return string The path of the asset if found, otherwise null.
			 */
			public static string GetAssetPath(UnityEngine.Object asset){
				return AssetDatabase.GetAssetPath(asset);
			}

			/**
			 * @brief Loads an generic Asset of type T based on the GUID.
			 * @param guid The GUID of the asset to load from Asset-Database.
			 * @return T The loaded asset or default(T) if not found.
			 */
			public static T LoadAsset<T>(string guid) where T : UnityEngine.Object {
				var path =  AssetDatabase.GUIDToAssetPath(guid);
				return AssetDatabase.LoadAssetAtPath<T>(path);
			}

			/**
			* @brief Adds a sub-asset at given base asset.
			* @param baseAsset The base asset where the sub-asset will be added to.
			* @param subAsset The sub asset which will be added.
			* @param name The name which will be used for the sub-asset and shown in hierarchy.
			* @param hideFlag HideFlag which should be used (default == none).
			* @throws UnassignedReferenceExceptions if required values are invalid or base asset path could not be found. 
			*/	
			public static void AddSubAsset(UnityEngine.Object baseAsset, UnityEngine.Object subAsset, string name, HideFlags hideFlags = HideFlags.None){
				if(null==baseAsset) throw new UnassignedReferenceException("[AssetUtils]:AddSubAsset: Required base Asset reference is invalid!");
				if(null==subAsset) throw new UnassignedReferenceException("[AssetUtils]:AddSubAsset: Required sub Asset reference is invalid!");

				var path = GetAssetPath(baseAsset);
				if(null != path){
					subAsset.name = name;
					subAsset.hideFlags = hideFlags;

					AssetDatabase.AddObjectToAsset(subAsset, path);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
				else{
					throw new UnassignedReferenceException("[AssetUtils]:AddSubAsset: Could not add asset! The path for base Asset could not be found: " + baseAsset.name);
				}
			}

			/**
			 * @brief Remove a objectt from it's asset.
			 * @param objectToRemove The object to remove from the asset it is owned by.
			 */
			public static void RemoveFromAsset(UnityEngine.Object objectToRemove){
				if(null!=objectToRemove){
					AssetDatabase.RemoveObjectFromAsset(objectToRemove);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}

			/**
			 * @brief Changes the visibility of an asset in the object hierarchy.
			 * @note  Use this method only for debugging and take note, that the effect of value change could be take some time.
			 * @param asset The asset which should be handled.
			 * @param hide If true the asset will be hidden, otherwise it will be become visible.
			 */
			public static void HideAssetInHierarchy(UnityEngine.Object asset, bool hide){
				var path = GetAssetPath(asset);
				if(null != path){
					asset.hideFlags = hide == true ? HideFlags.HideInHierarchy : HideFlags.None;
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
		#endif
        }
    }
}