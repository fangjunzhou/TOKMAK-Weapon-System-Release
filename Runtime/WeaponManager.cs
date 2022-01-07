using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    public class WeaponManager : MonoBehaviour, IWeaponManager
    {
        #region Public Field

        /// <summary>
        /// All the weapons that should be load in awake.
        /// </summary>
        public List<Weapon> preLoadWeapons;

        #endregion

        #region Hide Public Field

        public Action onInitialize
        {
            get
            {
                return _onInitialize;
            }
        }

        public Action onFinishInitialize
        {
            get
            {
                return _onFinishInitialize;
            }
        }

        public List<IWeapon> carryWeapons
        {
            get
            {
                return _carryWeapons;
            }
        }

        public WeaponManagerState state
        {
            get
            {
                return _state;
            }
        }

        #endregion

        #region Private Field

        /// <summary>
        /// The action event called when the WeaponManager starts initialization.
        /// </summary>
        private Action _onInitialize;

        /// <summary>
        /// The action event called when the WeaponManager finish initialization.
        /// </summary>
        private Action _onFinishInitialize;

        /// <summary>
        /// All the weapons current WeaponManager is carrying.
        /// </summary>
        private List<IWeapon> _carryWeapons;

        /// <summary>
        /// The weapon player is currently using.
        /// </summary>
        private IWeapon _currWeapon;

        /// <summary>
        /// The current WeaponManager's state.
        /// </summary>
        private WeaponManagerState _state;

        #endregion

        private void Awake()
        {
            // Start initialization.
            onInitialize?.Invoke();

            // Load all the preLoadWeapons.
            foreach (Weapon preLoadWeapon in preLoadWeapons)
            {
                // Instantiate the weapon.
                Weapon weaponInstance = Instantiate(preLoadWeapon);
                _carryWeapons.Add(weaponInstance);
                // Initialize the weapon.
                weaponInstance.OnInitialize();
            }
            
            // Finish initialization.
            onFinishInitialize?.Invoke();
        }

        #region IWeaponManager Interface

        public void PutOut(int index)
        {
            throw new NotImplementedException();
        }

        public void PutOut(string id)
        {
            throw new NotImplementedException();
        }

        public void PutOut(IWeapon weapon)
        {
            throw new NotImplementedException();
        }

        public Task PutOutAsync(int index)
        {
            throw new NotImplementedException();
        }

        public Task PutOutAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task PutOutAsync(IWeapon weapon)
        {
            throw new NotImplementedException();
        }

        public void PutIn()
        {
            // Put in current weapon.
            _currWeapon.OnPutIn();

            // There's no currently using weapon.
            _currWeapon = null;
            _state = WeaponManagerState.Empty;
        }

        public async Task PutInAsync()
        {
            _state = WeaponManagerState.PuttingIn;
            
            // Async put in.
            await _currWeapon.OnPutInAsync();

            // No weapon being used currently.
            _currWeapon = null;
            _state = WeaponManagerState.Empty;
        }

        public void TriggerDown()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            _currWeapon.OnTriggerDown();
        }

        public void TriggerUp()
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}