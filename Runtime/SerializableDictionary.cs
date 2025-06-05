using UnityEngine;
using System.Collections.Generic;

namespace binrev
{
    namespace utils{
        [System.Serializable]
        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
        {
            [SerializeField, HideInInspector]
            private List<TKey> keyData = new List<TKey>();
            
            [SerializeField, HideInInspector]
            private List<TValue> valueData = new List<TValue>();

            public List<TKey> KeysList => keyData;
            public List<TValue> ValuesList => valueData;

            void ISerializationCallbackReceiver.OnAfterDeserialize()
            {
                this.Clear();
                for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
                {
                    this[this.keyData[i]] = this.valueData[i];
                }
            }

            void ISerializationCallbackReceiver.OnBeforeSerialize()
            {
                this.keyData.Clear();
                this.valueData.Clear();

                foreach (var item in this)
                {
                    this.keyData.Add(item.Key);
                    this.valueData.Add(item.Value);
                }
            }
        }

    }
}