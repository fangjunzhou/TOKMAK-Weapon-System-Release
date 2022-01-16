using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinTOKMAK.TimelineSystem.Runtime;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    [RequireComponent(typeof(TimelineSystem.Runtime.TimelineSystem))]
    public class WeaponManager<ConfigType, RuntimeType> : MonoBehaviour, IWeaponManager<Weapon<ConfigType, RuntimeType>> where ConfigType : WeaponConfigData where RuntimeType : WeaponRuntimeData
    {
        #region Private Field

        /// <summary>
        /// The timeline system to play all the timeline.
        /// </summary>
        private TimelineSystem.Runtime.TimelineSystem _timelineSystem;

        /// <summary>
        /// The TimelineEventManager used by the TimelineSystem.
        /// </summary>
        private TimelineEventManager _timelineEventManager;
        
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
        private List<Weapon<ConfigType, RuntimeType>> _carryWeapons = new List<Weapon<ConfigType, RuntimeType>>();

        /// <summary>
        /// The weapon player is currently using.
        /// </summary>
        private Weapon<ConfigType, RuntimeType> _currWeapon;
        
        /// <summary>
        /// The index of _currWeapon;
        /// </summary>
        private int _currIndex = -1;

        /// <summary>
        /// The current WeaponManager's state.
        /// </summary>
        private WeaponManagerState _state;

        #endregion
        
        #region Serialized Private Field
        
        /// <summary>
        /// If the current WeaponManager has local authority.
        /// </summary>
        [SerializeField]
        private bool _isLocal = true;

        /// <summary>
        /// All the weapons that should be load in awake.
        /// </summary>
        [SerializeField]
        private List<Weapon<ConfigType, RuntimeType>> _preLoadWeapons;

        /// <summary>
        /// The dictionary that stores all the mount points that can mount weapon instance.
        /// </summary>
        [SerializeField]
        private WeaponMountPointDict _weaponMountPoint;

        #endregion

        #region Hide Public Field

        public bool isLocal
        {
            get => _isLocal;
            set => _isLocal = value;
        }

        public Action onInitialize => _onInitialize;

        public Action onFinishInitialize => _onFinishInitialize;

        public WeaponMountPointDict weaponMountPoint => _weaponMountPoint;

        public List<Weapon<ConfigType, RuntimeType>> carryWeapons => _carryWeapons;

        public Weapon<ConfigType, RuntimeType> currWeapon => _currWeapon;

        public int currIndex => _currIndex;

        public WeaponManagerState state => _state;

        #endregion

        private void Awake()
        {
            // Initialize all the private variables.
            _timelineSystem = gameObject.GetComponent<TimelineSystem.Runtime.TimelineSystem>();
            _timelineEventManager = gameObject.GetComponent<TimelineEventManager>();
            
            // Start initialization.
            onInitialize?.Invoke();

            // Load all the preLoadWeapons.
            foreach (Weapon<ConfigType, RuntimeType> preLoadWeapon in _preLoadWeapons)
            {
                AddWeapon(preLoadWeapon);
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

        #region Weapon Add & Remove

        public void AddWeapon(Weapon<ConfigType, RuntimeType> weapon)
        {
            int index = _carryWeapons.Count;
            AddWeapon(weapon, index);
        }

        public void AddWeapon(Weapon<ConfigType, RuntimeType> weapon, int index)
        {
            if (!(weapon is Weapon<ConfigType, RuntimeType> weapon1))
            {
                throw new InvalidCastException($"{weapon} is not a Weapon instance.");
            }

            Weapon<ConfigType, RuntimeType> weaponInstance = Instantiate(weapon1);
            _carryWeapons.Insert(index, weaponInstance);
                
            // Set the WeaponManager and TimelineSystem to the Weapon.
            weaponInstance.weaponManager = this;
            weaponInstance.timelineSystem = _timelineSystem;
            weaponInstance.timelineEventManager = _timelineEventManager;

            // Initialize the weapon.
            weaponInstance.OnInitialize();
        }

        public Weapon<ConfigType, RuntimeType> RemoveWeapon(int index)
        {
            // Check the index range.
            if (index < 0 || index >= carryWeapons.Count)
            {
                throw new IndexOutOfRangeException("The index of weapon is out of the range of carry weapons.");
            }

            Weapon<ConfigType, RuntimeType> removedWeapon = _carryWeapons[index];
            _carryWeapons.RemoveAt(index);
            return removedWeapon;
        }

        public Weapon<ConfigType, RuntimeType> RemoveWeapon(string id)
        {
            for (int i = 0; i < _carryWeapons.Count; i++)
            {
                if (_carryWeapons[i].id == id)
                {
                    return RemoveWeapon(i);
                }
            }

            throw new InvalidOperationException($"No weapon named {id} in the carry weapons.");
        }

        #endregion

        #region Put Out & Put In

        public void PutOut(int index)
        {
            // Handle the index out of range exception.
            if (index < 0 || index >= _carryWeapons.Count)
            {
                throw new IndexOutOfRangeException("The index of weapon is out of the range of carry weapons.");
            }
            
            // When the weapon is already in use, refuse to put the weapon out.
            if (_currWeapon != null && _currIndex == index)
            {
                throw new InvalidOperationException("Weapon already in use.");
            }
            
            // If there's still using weapon.
            if (_currWeapon != null)
            {
                // Put in _currWeapon first
                PutIn();
            }
            
            // Put out the weapon directly
            _carryWeapons[index].OnPutOut();
            _currWeapon = _carryWeapons[index];
            _currIndex = index;
            
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

        public void PutOut(Weapon<ConfigType, RuntimeType> weapon)
        {
            if (!_carryWeapons.Contains(weapon))
                throw new InvalidOperationException($"No weapon {weapon}");

            // Try to cast IWeapon to Weapon.
            if (!(weapon is Weapon<ConfigType, RuntimeType> weapon1))
                throw new InvalidCastException($"{weapon} is not a Weapon instance.");
            
            PutOut(_carryWeapons.IndexOf(weapon1));
        }

        public async Task PutOutAsync(int index)
        {
            // Handle the index out of range exception.
            if (index < 0 || index >= _carryWeapons.Count)
            {
                throw new IndexOutOfRangeException("The index of weapon is out of the range of carry weapons.");
            }
            
            // When the weapon is already in use, refuse to put the weapon out.
            if (_currWeapon != null && _currIndex == index)
            {
                Debug.LogWarning("Weapon already in use.");
                return;
            }
            
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
            _currIndex = index;
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

        public async Task PutOutAsync(Weapon<ConfigType, RuntimeType> weapon)
        {
            if (!_carryWeapons.Contains(weapon))
                throw new InvalidOperationException($"No weapon {weapon}");

            // Try to cast IWeapon to Weapon.
            if (!(weapon is Weapon<ConfigType, RuntimeType> weapon1))
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
            _currIndex = -1;
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
            _currIndex = -1;
            _state = WeaponManagerState.Empty;
        }

        #endregion

        #region Weapon Operation

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

        public void ReloadDown()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            _currWeapon.OnReloadDown();
        }

        public void ReloadUp()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            _currWeapon.OnReloadUp();
        }

        public void AimDown()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            _currWeapon.OnAimDown();
        }

        public void AimUp()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            _currWeapon.OnAimUp();
        }

        #endregion

        #endregion
    }
}