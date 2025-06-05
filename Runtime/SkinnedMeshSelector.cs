/**
  @class  SkinnedMeshSelector
  @date   22/08/2023
  @author Hellhound

  @brief 
  Selector to change SkinnedMeshRenderer Mesh data.
  
  This selector uses the SkinDataSetSO configuration based on a
  ScriptableObject to define the selectable skins, which could
  be changed at Editor and Runtime.

  @remark
  A SkinDataSetSO definition could be contain multimesh values. 
  In that case this selector also allowes to change between 
  those values too.

  Updated :
  Status  : FINAL
  Copyright Binary Revolution, Inc.  All rights reserved.
*/
using System.Linq;
using UnityEngine;

namespace binrev
{
    namespace utils
    {
        [RequireComponent(typeof(SkinnedMeshRenderer))]
        public class SkinnedMeshSelector : MonoBehaviour
        {
            [SerializeField] SkinDataSetSO skins;
            [SerializeField] SingleSelection skinSelector;
            [SerializeField] SingleSelection meshSelector;

            // The renderer used to set the selected skin data
            SkinnedMeshRenderer meshRenderer;

            // reference to actual selected skin
            SkinDataSetSO.Entry activeSkin = null;

            [SerializeField] public string activeMeshID = string.Empty;
            [SerializeField] public string skinID = string.Empty;

            /**
             * @brief Check if required references are set otherwise disable the script.
             * @remark Unity lifetime method called once when the MonoBehaviour is invoked.
             */
            private void Awake()
            {
                if (null == this.skinSelector)
                {
                    Debug.LogWarning("Required Selector reference is invalid. Component disabled!");
                    this.gameObject.SetActive(false);
                    return;
                }

                if (null == this.skins)
                {
                    Debug.LogWarning("Required SkinDataSO reference is invalid. Component disabled!");
                    this.gameObject.SetActive(false);
                    return;
                }

                this.meshRenderer = GetComponent<SkinnedMeshRenderer>();
            }

            /**
             * @brief Update used data based on possible referenced SkinDataSetSO changes.
             * @remark Unity lifetime method always the component is enabled for execution.
             */
            private void OnEnable()
            {
                // Update data based on possible SO value changes and activate 
                //UpdateSkinSelector();
                //ActivateSkin(skinSelector.Selected);
                if (string.IsNullOrEmpty(activeMeshID)){
                    Debug.Log("Reset");
                    UpdateSkinSelector();
                    ActivateSkin(skinSelector.Selected);
                }
                else{
                    Debug.Log("Use given");
                    ActivateMesh(activeMeshID);
                }
            }

            /**
            * @brief Activate a skin for rendering based on given ID.
            * @remark This method takes also care about the selected mesh if SkinDataSO uses multimesh based values.
            * @param meshID The name ID of the skin based on SkinDataSO entries.
            */
            public void ActivateSkin(string skinID)
            {
                var skin = this.skins.entries.FirstOrDefault(s => s.Name == skinID);
                if (null != skin)
                {
                    this.activeSkin = skin;
                    this.skinID = skinID;
                    Debug.Log("Activate skin: " + skinID);
                }
            }

            /**
             * @brief Activate a mesh for rendering based on given ID.
             * @remark This method only takes effect if the SkinDataSO uses multimesh values.
             * @param meshID The name ID of the mesh based on SkinDataSO multimesh TypeDefinition.
             */
            public void ActivateMesh(string meshID)
            {
                if (null!=this.activeSkin && this.skins.UseMultiMesh && null != this.meshRenderer)
                {
                    var mesh = activeSkin.GetMesh(meshID);
                    if (null != mesh)
                    {
                        this.meshRenderer.sharedMesh = mesh;
                    }
                }
                Debug.Log("Activate mesh: " + meshID);
                this.activeMeshID = meshID;
                this.meshSelector.SetSelected(meshID);
            }

            /**
             * @brief Utility method to update given SingleSelection elements.
             * 
             * The data handled by this component are related to the SkinDataSO object. To recognize
             * possible value changes, we have to update our selector elements. Please take note that
             * the SingleSelection objects SerializedFields, because they are no UnityObject elements
             * and could not rendered otherwise using a PropertyDrawer.
            */
            private void UpdateSkinSelector()
            {
                // return if reference to SkinDataSetSO is not set
                if (null==this.skins) return;

                this.skinSelector.UpdateValues(this.skins.SkinIDs);

                if (this.skins.UseMultiMesh)
                {
                    this.meshSelector.UpdateValues(this.skins.MultiMeshIDs);
                }
                else
                {
                    this.meshSelector.Clear();
                }
            }

#if UNITY_EDITOR

            public SkinnedMeshRenderer Renderer { set { this.meshRenderer = value; } }

            /**
             * @brief Utility method to update any date from Unity custom Editor.
             * 
             * This method use a custom editor which uses to update the visual data
             * directly in the editor mode.
             *
            */
            public void UpdateData()
            {
                if (null!=this.meshRenderer && null!=this.skins)
                {
                    UpdateSkinSelector();
                    ActivateSkin(this.skinSelector.Selected);
                }
            }
#endif
        }
    }
}