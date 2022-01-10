using System.Threading.Tasks;
using FinTOKMAK.TimelineSystem.Runtime;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    public class Weapon : ScriptableObject, IWeapon
    {
        #region Hide Public Field

        public string id => _configData.id;

        public IWeaponData configData => _configData;

        public IWeaponData runtimeData => _runtimeData;

        public IWeaponManager weaponManager
        {
            get => _weaponManager;
            set => _weaponManager = (WeaponManager)value;
        }

        public TimelineSystem.Runtime.TimelineSystem timelineSystem
        {
            get => _timelineSystem;
            set => _timelineSystem = value;
        }

        public TimelineEventManager timelineEventManager
        {
            get => _timelineEventManager;
            set => _timelineEventManager = value;
        }

        #endregion

        #region Private Field

        #region Weapon Data
        
        [SerializeField]
        private WeaponConfigData _configData;
        
        private WeaponRuntimeData _runtimeData;
        
        private WeaponManager _weaponManager;

        private TimelineSystem.Runtime.TimelineSystem _timelineSystem;

        private TimelineEventManager _timelineEventManager;

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