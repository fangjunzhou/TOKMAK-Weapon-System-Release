using System.Threading.Tasks;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    public class Weapon : ScriptableObject, IWeapon
    {
        #region Hide Public Field

        public string id => _configData.id;

        public IWeaponData configData => _configData;

        public IWeaponData runtimeData => _runtimeData;

        #endregion

        #region Private Field

        #region Weapon Data

        
        [SerializeField]
        private WeaponConfigData _configData;
        
        private WeaponRuntimeData _runtimeData;

        #endregion

        #endregion

        public virtual void OnInitialize()
        {
            // Convert the WeaponConfigData to the WeaponRuntimeData
            _runtimeData = (WeaponRuntimeData) _configData.ToRuntime();
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnPutOut()
        {
            
        }

        public async Task OnPutOutAsync()
        {
            OnPutOut();
        }

        public virtual void OnPutIn()
        {
            
        }

        public async Task OnPutInAsync()
        {
            OnPutIn();
        }

        public virtual void OnTriggerDown()
        {
            
        }

        public virtual void OnTriggerUp()
        {
            
        }

        public virtual void OnReload()
        {
            
        }
    }
}