/**
  @class  SkinDataSetSO
  @date   22/08/2023
  @author Hellhound

  @brief 
  ScriptableObject to define Skin-Sets used by the SkinnedMeshSelector.

  Skin-Set definitons are used to separate possible visual representation of
  GameObjects using SkinnedMesh renderer, i.e. to separate types like human,
  elf ect., which could be changed by the SkinnedMeshSelector at runtime.

  @note
  Please take note that each skin-set in a SkinDataSetSO must be based on the
  same skinned mesh conditions, like the armature to avoid render failures.

  @remark
  Each Skin-Set is identified in this configuration by a unique Name-ID
  and could be selected by the SkinnedMeshSelector in Editor- and Runtime
  mode to control the visual presentation of a SkinnedMesh based GameObject.
  
  @note
  Skin-Set definitons are used to separate possible visual representation of
  GameObjects

  @note
  Each Skin-Set in a configuration could be based on a single mesh or either
  a multi mesh value. If multi mesh definition is selected, you have to define
  a TypeDefinition used to separate the meshes by name identifier. This is 
  typically used to separate a skin set i.e. based on gender values.
  
 
  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace binrev
{
    namespace utils
    {
        [CreateAssetMenu(fileName = "SkinDataSetSO", menuName = "BinRev/SyntyTools/SkinDataSet", order = 1)]
        public class SkinDataSetSO : ScriptableObject
        {
            [System.Serializable]
            public class Entry 
            {
                // none-changeable name ID of the entry 
                [SerializeField] private string name;
                public string Name => this.name;

                // the list of mesh data of an entry
                [SerializeField] public List<NamedObjectValue<Mesh>> skins = new List<NamedObjectValue<Mesh>>();

                public Entry(string name)
                {
                    this.name = name;
                }

                public Mesh GetMesh(string name)
                {
                    var skin = skins.FirstOrDefault(s => s.Name == name);
                    return (null != skin) ? skin.Value : null;
                }

#if UNITY_EDITOR
                public void DoLayout()
                {
                    EditorGUILayout.BeginVertical();
                    this.skins.ForEach(skin => skin.DoLayout());
                    EditorGUILayout.EndVertical();
                }
#endif
            };

            [SerializeField] public List<Entry> entries = new List<Entry>();
            [SerializeField] bool multiMeshSupport = false;
            public bool UseMultiMesh => this.multiMeshSupport;

            [SerializeField] private TypeDefinitionSelector multiMeshSelector;

            public IList<string> MultiMeshIDs
            {
                get
                {
                    return this.multiMeshSelector.Config.Labels;
                }
            }

            public IList<string> SkinIDs => entries.Select(e => e.Name).ToList().AsReadOnly();

            public Mesh GetSelectedSkin(string skinName)
            {
                if (string.IsNullOrEmpty(skinName)) return null;

                var entry = this.entries.FirstOrDefault(e => e.Name == skinName);
                return (null != entry) ? entry.GetMesh(this.multiMeshSelector.Selected) : null;
            }

            public bool Contains(string nameID)
            {
                return (this.entries.Count == 0) ? false : null!=this.entries.FirstOrDefault(e => e.Name == nameID);
            }

#if UNITY_EDITOR
            public void CreateEntry(string name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var entry = new Entry(name);
                    if (this.multiMeshSupport)
                    {
                        if (null != this.multiMeshSelector.Config)
                        {
                            foreach (var label in this.multiMeshSelector.Config.Labels)
                            {
                                entry.skins.Add(new NamedObjectValue<Mesh>(label));
                            }
                        }
                    }
                    else
                    {
                        entry.skins.Add(new NamedObjectValue<Mesh>("Mesh"));
                    }
                    entries.Add(entry);
                }
            }

            public void Clear()
            {
                if (null != this.multiMeshSelector) this.multiMeshSelector = null;
                if (this.entries.Count > 0) this.entries.Clear();
            }
#endif
        }
    }
}