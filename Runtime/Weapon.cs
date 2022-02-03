using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioSystem.Runtime;
using FinTOKMAK.EventSystem.Runtime;
using FinTOKMAK.TimelineSystem.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    [System.Serializable]
    public class WeaponAudioEventConfig
    {
        /// <summary>
        /// The timeline event that trigger the audio.
        /// </summary>
        [TimelineEvent]
        public string eventName;

        /// <summary>
        /// The audio config of the audio to play.
        /// </summary>
        public AudioConfig audioConfig;
    }
    
    public class Weapon<ConfigType, RuntimeType> : ScriptableObject, IWeapon<Weapon<ConfigType, RuntimeType>> where ConfigType : WeaponConfigData where RuntimeType : WeaponRuntimeData
    {
        #region Hide Public Field

        public string id => _configData.id;

        public int index { get; set; }

        public IWeaponData configData => _configData;

        public IWeaponData runtimeData => _runtimeData;

        public IWeaponManager<Weapon<ConfigType, RuntimeType>> weaponManager
        {
            get => _weaponManager;
            set => _weaponManager = (WeaponManager<ConfigType, RuntimeType>)value;
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

        #region Protected Field

        #region Weapon Data
        
        [SerializeField]
        protected ConfigType _configData;

        protected RuntimeType _runtimeData;
        
        protected WeaponManager<ConfigType, RuntimeType> _weaponManager;

        protected TimelineSystem.Runtime.TimelineSystem _timelineSystem;

        protected TimelineEventManager _timelineEventManager;

        #endregion

        #region Weapon Events

        /// <summary>
        /// The TSC for finish put out event
        /// </summary>
        private TaskCompletionSource<bool> _finishPutoutSource;
        
        /// <summary>
        /// The TSC for finish put in event
        /// </summary>
        private TaskCompletionSource<bool> _finishPutinSource;

        #endregion

        #endregion

        #region Private Field

        private Dictionary<string, Action<IEventData>> _audioActions = new Dictionary<string, Action<IEventData>>();

        #endregion

        #region Protected Methods

        /// <summary>
        /// Call this method to initialize all the TaskCompletionSource.
        /// </summary>
        protected virtual void InitTSC()
        {
            _finishPutoutSource = new TaskCompletionSource<bool>();
            _finishPutinSource = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// Call this method to register all the timeline events with corresponding weapon events.
        /// </summary>
        protected virtual void RegisterTimelineEvents()
        {
            foreach (string eventNames in _audioActions.Keys)
            {
                _timelineEventManager.RegisterEvent(eventNames, _audioActions[eventNames]);
            }
            _timelineEventManager.RegisterEvent(_configData.finishPutoutEvent, WeaponPutoutEvent);
            _timelineEventManager.RegisterEvent(_configData.finishPutinEvent, WeaponPutinEvent);
        }

        /// <summary>
        /// Call this method to unregister the timeline events with corresponding weapon events.
        /// </summary>
        protected virtual void UnregisterTimelineEvents()
        {
            foreach (string eventNames in _audioActions.Keys)
            {
                _timelineEventManager.UnRegisterEvent(eventNames, _audioActions[eventNames]);
            }
            _timelineEventManager.UnRegisterEvent(_configData.finishPutoutEvent, WeaponPutoutEvent);
            _timelineEventManager.UnRegisterEvent(_configData.finishPutinEvent, WeaponPutinEvent);
        }

        /// <summary>
        /// The method to call a Weapon RPC method.
        /// </summary>
        /// <param name="methodName">The method name.</param>
        /// <param name="methodParams">The method parameters.</param>
        protected void CallRPC(string methodName, params object[] methodParams)
        {
            _weaponManager.weaponAgent.CallRPC(index, methodName, methodParams);
        }

        #endregion

        #region Audio Player

        private void InitAudioPlayers()
        {
            foreach (WeaponAudioEventConfig config in _configData.audioEventConfigs)
            {
                _configData.audioPlayerPrefab.audioConfig = config.audioConfig;
                AudioPlayer playerInstance = Instantiate(_configData.audioPlayerPrefab,
                    _weaponManager.weaponMountPoint[_configData.audioMountPoint]);
                _audioActions.Add(config.eventName, data =>
                {
                    playerInstance.Play();
                    CallRPC("InvokeAudioEvent", config.eventName);
                });
            }
        }

        /// <summary>
        /// The RPC method to invoke an audio event.
        /// </summary>
        /// <param name="eventName">the name of the audio event.</param>
        public void InvokeAudioEvent(string eventName)
        {
            // Play the fire audio
            _timelineEventManager.InvokeEvent(eventName, new EventData());
        }

        #endregion
        
        #region Timeline Events

        /// <summary>
        /// Timeline event invoked when the weapon finish put out.
        /// </summary>
        /// <param name="data">EventData</param>
        private void WeaponPutoutEvent(IEventData data)
        {
            _finishPutoutSource.SetResult(true);
        }

        /// <summary>
        /// Timeline event invoked when the weapon finish put in.
        /// </summary>
        /// <param name="data">EventData</param>
        private void WeaponPutinEvent(IEventData data)
        {
            _finishPutinSource.SetResult(true);
        }

        #endregion

        #region Life Cycle

        public virtual void OnInitialize()
        {
            InitTSC();
            
            InitAudioPlayers();
            
            // Convert the WeaponConfigData to the WeaponRuntimeData
            _runtimeData = (RuntimeType) _configData.ToRuntime();

            // Change the using status.
            _runtimeData.usingStatus = WeaponUsingStatus.Background;
        }

        public virtual void OnUpdate()
        {
            
        }

        #endregion

        #region Put In And Put Out

        public virtual void OnPutOut()
        {
            // Register event listening.
            RegisterTimelineEvents();
            
            // Play the put out timeline.
            _timelineSystem.PlayTimeline(_runtimeData.putoutTimline);
            
            // Change the using status.
            _runtimeData.usingStatus = WeaponUsingStatus.Using;
        }
        
        public virtual void OnFinishPutOut()
        {
            
        }

        public async Task OnPutOutAsync()
        {
            OnPutOut();
            
            // Wait until the weapon finish put out.
            await _finishPutoutSource.Task;
            
            // Reset the _finishPutoutSource
            _finishPutoutSource = new TaskCompletionSource<bool>();
            
            OnFinishPutOut();
        }

        public virtual void OnPutIn()
        {
            // Play the put in Timeline
            _timelineSystem.PlayTimeline(_runtimeData.putinTimeline);
        }
        
        public virtual void OnFinishPutIn()
        {
            // Unregister event listening
            UnregisterTimelineEvents();
            
            // Change the using status.
            _runtimeData.usingStatus = WeaponUsingStatus.Background;
        }

        public async Task OnPutInAsync()
        {
            OnPutIn();

            // Wait until the weapon finish put in.
            await _finishPutinSource.Task;

            // Reset the _finishPutinSource
            _finishPutinSource = new TaskCompletionSource<bool>();
            
            OnFinishPutIn();
        }

        #endregion

        #region Weapon Operation

        public void Mount(GameObject instance, string mountPoint)
        {
            if (!_weaponManager.weaponMountPoint.ContainsKey(mountPoint))
            {
                throw new InvalidOperationException($"No mount point name {mountPoint}");
            }
            instance.transform.SetParent(_weaponManager.weaponMountPoint[mountPoint]);
        }

        public virtual void OnTriggerDown()
        {
            
        }

        public virtual void OnTriggerUp()
        {
            
        }

        public virtual void OnShootEnableChanged(bool enable)
        {
            
        }

        public virtual void OnReloadDown()
        {
            
        }

        public virtual void OnReloadUp()
        {
            
        }

        public virtual void OnReloadEnableChanged(bool enable)
        {
            
        }

        public virtual void OnAimDown()
        {
            
        }

        public virtual void OnAimUp()
        {
            
        }

        public virtual void OnAimEnableChanged(bool enable)
        {
            
        }

        #endregion
    }
}