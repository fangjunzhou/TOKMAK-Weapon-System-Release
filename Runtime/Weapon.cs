using System.Threading.Tasks;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    public class Weapon : ScriptableObject, IWeapon
    {
        #region Hide Public Field

        public string id { get; }

        public IWeaponData configData => _configData;

        public IWeaponData runtimeData => _runtimeData;

        #endregion

        #region Private Field

        [SerializeField]
        private WeaponConfigData _configData;

        [SerializeField]
        private WeaponRuntimeData _runtimeData;

        #endregion

        public virtual void OnInitialize()
        {
            
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