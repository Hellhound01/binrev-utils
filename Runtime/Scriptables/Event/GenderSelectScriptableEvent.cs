using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using binrev.scriptables;

namespace binrev
{
	namespace utils
	{
        [CreateAssetMenu(fileName="GenderSelectEvent", menuName = "BinRev/Scriptables/Event/Gender Select Event", order = 1)]
        public class GenderSelectScriptableEvent : ScriptableEvent<GenderSelector.Gender>
        {}
    }
}