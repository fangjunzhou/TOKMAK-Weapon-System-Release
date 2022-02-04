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

        #region Weapon Status
        
        /// <summary>
        /// The current WeaponManager's state.
        /// </summary>
        private WeaponManagerState _state;

        private bool _triggerDown = false;

        private bool _reloadDown = false;

        private bool _aimDown = false;

        #endregion

        #region Operation Enable

        private bool _able2Shoot = true;

        private bool _able2Aim = true;

        private bool _able2Reload = true;

        #endregion

        #region Event Hook

        private Action<int> _putOutWeaponEvent;

        private Action<int> _asyncPutOutWepaonEvent;

        private Action _putInWeaponEvent;

        private Action _asyncPutInWeaponEvent;

        #endregion

        #endregion
        
        #region Serialized Private Field

        [SerializeField]
        private MonoBehaviour _remoteAgent;

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

        public IRemoteWeaponAgent<Weapon<ConfigType, RuntimeType>> weaponAgent
        {
            get
            {
                var _remoteAgent1 = _remoteAgent as IRemoteWeaponAgent<Weapon<ConfigType, RuntimeType>>;
                if (_remoteAgent1 == null)
                    throw new InvalidCastException("Remote Agent not implement IRemoteWeaponAgent interface.");
                return _remoteAgent1;
            }
        }

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

        public bool able2Shoot
        {
            get => _able2Shoot;
            set
            {
                // Call the shoot enable callback function.
                if (_currWeapon != null)
                {
                    _currWeapon.OnShootEnableChanged(value);
                }
                _able2Shoot = value;
            }
        }

        public bool able2Aim
        {
            get => _able2Aim;
            set
            {
                // Call the aim enable callback function.
                if (_currWeapon != null)
                {
                    _currWeapon.OnAimEnableChanged(value);
                }
                _able2Aim = value;
            }
        }

        public bool able2Reload
        {
            get => _able2Reload;
            set
            {
                // Call the reload enable callback function.
                if (_currWeapon != null)
                {
                    _currWeapon.OnReloadEnableChanged(value);
                }
                _able2Reload = value;
            }
        }

        public Action<int> putOutWeaponEvent
        {
            get => _putOutWeaponEvent;
            set => _putOutWeaponEvent = value;
        }

        public Action<int> asyncPutOutWeaponEvent
        {
            get => _asyncPutOutWepaonEvent;
            set => _asyncPutOutWepaonEvent = value;
        }

        public Action putInWeaponEvent
        {
            get => _putInWeaponEvent;
            set => _putInWeaponEvent = value;
        }

        public Action asyncPutInWeaponEvent
        {
            get => _asyncPutInWeaponEvent;
            set => _asyncPutInWeaponEvent = value;
        }

        #endregion

        private void Awake()
        {
            // Initialize all the private variables.
            _timelineSystem = gameObject.GetComponent<TimelineSystem.Runtime.TimelineSystem>();
            _timelineEventManager = gameObject.GetComponent<TimelineEventManager>();
            
            // Initialize WeaponManager
            weaponAgent.manager = this;
            
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
            
            UpdateWeaponIndex();

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
            
            UpdateWeaponIndex();
            
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
            if (_state == WeaponManagerState.PuttingIn || _state == WeaponManagerState.PuttingOut)
                return;
            
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
            
            // Invoke the event
            _putOutWeaponEvent?.Invoke(index);
            
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
            
            if (_state == WeaponManagerState.PuttingIn || _state == WeaponManagerState.PuttingOut)
                return;
            
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
            _currWeapon = _carryWeapons[index];
            _currIndex = index;
            
            // Invoke the event.
            _asyncPutOutWepaonEvent?.Invoke(index);
            
            _state = WeaponManagerState.Ready;
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
            
            if (_state == WeaponManagerState.PuttingIn || _state == WeaponManagerState.PuttingOut)
                return;
            
            // Put in current weapon.
            _currWeapon.OnPutIn();

            // There's no currently using weapon.
            _currWeapon = null;
            _currIndex = -1;
            
            // Invoke the event.
            _putInWeaponEvent?.Invoke();
            
            _state = WeaponManagerState.Empty;
        }

        public async Task PutInAsync()
        {
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            
            if (_state == WeaponManagerState.PuttingIn || _state == WeaponManagerState.PuttingOut)
                return;
            
            _state = WeaponManagerState.PuttingIn;
            
            // Async put in.
            await _currWeapon.OnPutInAsync();

            // No weapon being used currently.
            _currWeapon = null;
            _currIndex = -1;
            
            // Invoke the event
            _asyncPutInWeaponEvent?.Invoke();
            
            _state = WeaponManagerState.Empty;
        }

        #endregion

        #region Weapon Operation

        public void TriggerDown()
        {
            if (!_able2Shoot)
            {
                Debug.LogWarning("Not able to shoot.");
                return;
            }
            
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            
            _currWeapon.OnTriggerDown();
            _triggerDown = true;
        }

        public void TriggerUp()
        {
            if (!_triggerDown)
                return;

            if (!_able2Shoot)
            {
                _triggerDown = false;
                return;
            }
            
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                _triggerDown = false;
                return;
            }
            _currWeapon.OnTriggerUp();
            _triggerDown = false;
        }

        public void ReloadDown()
        {
            if (!_able2Reload)
            {
                Debug.LogWarning("Not able to reload.");
                return;
            }
            
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            
            _currWeapon.OnReloadDown();
            _reloadDown = true;
        }

        public void ReloadUp()
        {
            if (!_reloadDown)
                return;

            if (!_able2Reload)
            {
                _reloadDown = false;
                return;
            }
            
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                _reloadDown = false;
                return;
            }
            
            _currWeapon.OnReloadUp();
            _reloadDown = false;
        }

        public void AimDown()
        {
            if (!_able2Aim)
            {
                Debug.LogWarning("Not able to aim.");
                return;
            }
            
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                return;
            }
            
            _currWeapon.OnAimDown();
            _aimDown = true;
        }

        public void AimUp()
        {
            if (!_aimDown)
                return;

            if (!_able2Aim)
            {
                _aimDown = false;
                return;
            }
            
            if (_currWeapon == null)
            {
                Debug.LogWarning("No weapon being used currently.");
                _aimDown = false;
                return;
            }
            _currWeapon.OnAimUp();
            _aimDown = false;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Update the index of all the carry Weapons.
        /// </summary>
        private void UpdateWeaponIndex()
        {
            for (int i = 0; i < _carryWeapons.Count; i++)
            {
                _carryWeapons[i].index = i;
            }
        }

        #endregion
    }
}