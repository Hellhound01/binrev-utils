using UnityEngine;
using UnityEngine.Events;

using binrev.scriptables;

namespace binrev{
    namespace utils{
        public class GenderSelectScriptableEventListener : ScriptableEventListener<string>
        {
            [SerializeField] UnityEvent OnMaleSelect = new UnityEvent();     
            [SerializeField] UnityEvent OnFemaleSelect = new UnityEvent();    

            public override void OnInvoke(string value)
            {
                if("Male" == value){
                    OnMaleSelect?.Invoke();
                }
                else{
                    OnFemaleSelect?.Invoke();
                }
            }
        }
    }
}