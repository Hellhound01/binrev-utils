using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace binrev {
    namespace utils {

		public static class GameObjectExtensions
		{ 
			/**
			* @brief Checks wether a LayerMask is set on object by bitwise comparison.
			* @param obj The Unity GameObject to check if the mask value is contained.
			* @param mask The Unity LayerMask value to search for.
			* @return bool True if LayerMask is contained, otherwise false.
			*/
			public static bool IsInLayerMask(this GameObject obj, LayerMask mask) {
				return ((mask.value & (1 << obj.layer)) > 0);
			}

			public static void SetLayerRecursively(this GameObject obj, int newLayer)
			{
				if (null == obj){
					return;
				}

				obj.layer = newLayer;
				foreach (Transform child in obj.transform)
				{
					if (null == child){
						continue;
					}
					SetLayerRecursively(child.gameObject, newLayer);
				}
			}

			/** 
			* @brief Find a single Transform in Transform hierarchy based on given tag ID.
			* @param obj The Unity Transform to start search from recursively.
			* @param name The name ID of the Transform to search for.
			* @return Transform The first match of a Transform with an equal name ID, otherwise null.
			**/

			public static Transform Search(this Transform target, string name)
			{
				if (target.name == name) return target;
				for (int i = 0; i < target.childCount; ++i)
				{
					var result = Search(target.GetChild(i), name);
					if (result != null) return result;
				}
				return null;
			}

			/** 
			* @brief Find any Transform in an GameObject hierarchy based on given tag ID.
			* @param obj The Unity GameObject to start the search from.
			* @param tagID The name ID of the Tag to search for.
			* @return List<Transform> List of Transforms with an equal tag ID.
			**/
			public static List<Transform> FindTransformInChildsByTag(this GameObject gameObject, string tagID){
				List<Transform> result = new List<Transform>();
				foreach (Transform item in  gameObject.GetComponentsInChildren<Transform>(true)) {
					if (item.tag == tagID) {
						result.Add(item);
					}
				}
				return result;
			}

			/** 
			* @brief Find a GameObjec as child based on given name ID.
			* @param obj The Unity GameObject to start the search form.
			* @param name The name ID of the child GameObject to search for.
			* @return GameObject The GameObject searched for or null if not exist.
			**/
			public static GameObject FindObjectInChilds(this GameObject gameObject, string gameObjectName)
			{
				foreach (Transform item in gameObject.GetComponentsInChildren<Transform>(true)) {
					if (item.name == gameObjectName) {
						return item.gameObject;
					}
				}
				return null;
			}

			/** 
			* @brief Find a parent GameObject based on given name ID in object hierarchy.
			* @param obj The Unity GameObject to search the search from recursively.
			* @param name The name ID of the parent GameObject to search for.
			* @return GameObject The GameObject searched for or null if not exist.
			**/
			public static GameObject FindParent(this GameObject gameObject, string name){
				GameObject parent = gameObject.transform.parent.gameObject;
				if(parent.name.Equals(name)) return parent;
				else return parent.FindParent(name);
			}

			/** 
			* @brief Find the root GameObject based in object hierarchy.
			* @param obj The Unity GameObject to search the search from recursively.
			* @return GameObject The root GameObject searched for.
			**/
			public static GameObject FindRoot(this GameObject gameObject){
				Transform parent = gameObject.transform.parent;
				if(parent != null) return parent.gameObject.FindRoot();
				else return gameObject;
			}

			/** 
			* @brief Reparent a GameObject to a given Transform and moves this object to that location.
			* @param obj The Unity GameObject which is reparented.
			* @param newParent The Transform object the GameObject is reparented to.
			* @param rescale If this value is true, the GameObject is rescaled to parent scale (default = false).
			**/
			public static void Reparent(this GameObject obj, Transform newParent, bool rescale = false)
			{	
				obj.transform.SetParent(newParent, rescale);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localRotation = Quaternion.identity;
			}	

			/**
			* @brief Create a GameObject instance based on given prefab.
			* @note  You can rename the instance by an optional given name. Otherwise the name of the prefab
			*        will be used extended with "(Prefab instance)" will be used.
			* @param prefab The Prefab to generate GameObject from.
			* @param layer A possible layer which could be added recursivley to the instance.
			* @param anchor The Transform which should be used as parent for the created intance (default = null).
			* @param name An alternative name for the instance (default = null).
			* @param active Indicates if instance should be active (default = true).
			* @return GameObject The created instance or null if could not created.
			*/
			public static GameObject CreatePrefabInstance(GameObject prefab, LayerMask layer, Transform anchor = null, string name = null,  bool active = true)
			{
				GameObject model = null;
				if (prefab != null)
				{
					model = GameObject.Instantiate(prefab) as GameObject;
					model.SetActive(active);
					model.name = (null!=name) ? name : (prefab.name  + " (Prefab Instance)");

					if(anchor) model.Reparent(anchor);
					model.layer = layer;
					//model.SetLayerRecursively((int)Mathf.Log(layer.value, 2));
				}
				return model;
			}
		}
	}
}
