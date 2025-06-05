using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace binrev {
    namespace utils {
		public static class LayerMaskExtensions
		{ 
			/**
			 * @brief Checks wether a LayerMask is set on object by bitwise comparison.
			 * @param obj The Unity GameObject to check if the mask value is contained.
			 * @param mask The Unity LayerMask value to search for.
			 * @return bool True if LayerMask is contained, otherwise false.
			 */
			public static bool IsInLayerMask(this LayerMask mask, GameObject obj) {
				return ((mask.value & (1 << obj.layer)) > 0);
			}
			
			/**
			 * @brief Checks wether a bitfield layer value is contained in a LayerMask.
			 * @param mask The Unity LayerMask to check if the layer is contained.
			 * @param layer The bitfield layer value to search for.
			 * @return bool True if LayerMask is contained, otherwise false.
			 */
			 public static bool IsInLayerMask(this LayerMask mask, int layer) {
				 return ((mask.value & (1 << layer)) > 0);
			 }	

			/**
			 * @brief Returns the entries of a given bitwise LayerMask.
			 * @see http://answers.unity3d.com/questions/644123/layermask-can-someone-explain-why-is-256-8.html
			 * @return List<int> Indices of the LayerMask bitwise entries in range [0..38]. 
			 */	
			public static List<int> GetMaskIndices(this LayerMask mask)
			{
				List<int> indices = new List<int>(32);
				for(int i = 0; i < 32; i++)
				{
					if ((mask.value & (1 << i)) > 0)
						indices.Add(i);
				}
				return indices;
			}	
		}
	}
}
