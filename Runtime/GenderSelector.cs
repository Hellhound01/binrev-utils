using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using binrev.scriptables;

namespace binrev
{
	namespace utils
	{
        public class GenderSelector : MonoBehaviour
        {
            public enum Gender{
                Male = 0,
                Female = 1
            };
            
            [SerializeField] public Gender Sex = Gender.Male;
            [SerializeField] public StringScriptableEvent OnSelectEvent;

            void Awake()
            {
                OnSelectEvent?.Invoke(GetValueAsString());
            }

            public void Switch(){
                this.Sex = this.Sex == Gender.Male ? Gender.Female : Gender.Male;
                OnSelectEvent?.Invoke(GetValueAsString());
            }

            public string GetValueAsString(){
                return this.Sex == Gender.Male ? "Male" : "Female";
            }
        }
    }
}