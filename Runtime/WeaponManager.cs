using System;
using System.Collections.Generic;
using System.Linq;
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
                return _carryWeapons.Cast<IWeapon>().ToList();
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
        private List<Weapon> _carryWeapons = new List<Weapon>();

        /// <summary>
        /// The weapon player is currently using.
        /// </summary>
        private Weapon _currWeapon;

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

        private void Update()
        {
            // Call the update callback function of the current weapon.
            if (_currWeapon != null)
            {
                _currWeapon.OnUpdate();
            }
        }

        #region IWeaponManager Interface

        public void PutOut(int index)
        {
            // If there's still using weapon.
            if (_currWeapon != null)
            {
                // Put in _currWeapon first
                PutIn();
            }
            
            // Put out the weapon directly
            _carryWeapons[index].OnPutOut();
            _currWeapon = _carryWeapons[index];
            
            _state = WeaponManagerState.Ready;
        }

        public void PutOut(string id)
        {
            // Find the index of weapon with same id
            for (int i = 0; i < carryWeapons.Count; i++)
            {
                if (carryWeapons[i].id == id)
                {
                    PutOut(i);
                    return;
                }
            }

            // No weapon found.
            throw new InvalidOperationException($"No weapon with name {id}");
        }

        public void PutOut(IWeapon weapon)
        {
            if (!_carryWeapons.Contains(weapon))
                throw new InvalidOperationException($"No weapon {weapon}");

            // Try to cast IWeapon to Weapon.
            if (!(weapon is Weapon weapon1))
                throw new InvalidCastException($"{weapon} is not a Weapon instance.");
            
            PutOut(_carryWeapons.IndexOf(weapon1));
        }

        public async Task PutOutAsync(int index)
        {
            // If there's still using weapon.
            if (_currWeapon != null)
            {
                // Put in _currWeapon first
                await PutInAsync();
            }

            _state = WeaponManagerState.PuttingOut;
            // Put out the weapon directly
            await _carryWeapons[index].OnPutOutAsync();
            _state = WeaponManagerState.Ready;
            _currWeapon = _carryWeapons[index];
        }

        public async Task PutOutAsync(string id)
        {
            // Find the index of weapon with same id
            for (int i = 0; i < carryWeapons.Count; i++)
            {
                if (carryWeapons[i].id == id)
                {
                    await PutOutAsync(i);
                    return;
                }
            }

            // No weapon found.
            throw new InvalidOperationException($"No weapon with name {id}");
        }

        public async Task PutOutAsync(IWeapon weapon)
        {
            if (!_carryWeapons.Contains(weapon))
                throw new InvalidOperationException($"No weapon {weapon}");

            // Try to cast IWeapon to Weapon.
            if (!(weapon is Weapon weapon1))
                throw new InvalidCastException($"{weapon} is not a Weapon instance.");
            
            await PutOutAsync(_carryWeapons.IndexOf(weapon1));
        }

        public void PutIn()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            
            // Put in current weapon.
            _currWeapon.OnPutIn();

            // There's no currently using weapon.
            _currWeapon = null;
            _state = WeaponManagerState.Empty;
        }

        public async Task PutInAsync()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            
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
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            _currWeapon.OnTriggerUp();
        }

        public void Reload()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            _currWeapon.OnReload();
        }

        #endregion
    }
}