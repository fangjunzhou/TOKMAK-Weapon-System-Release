using System.Threading.Tasks;
using FinTOKMAK.EventSystem.Runtime;
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

        #region Weapon Events

        /// <summary>
        /// The TSC for finish put out event
        /// </summary>
        private TaskCompletionSource<bool> _finishPutoutSource;
        
        /// <summary>
        /// The TSC for finish put in event
        /// </summary>
        private TaskCompletionSource<bool> _finishPutinSource;

        /// <summary>
        /// The TimelineEvent called when finish weapon put out.
        /// </summary>
        [SerializeField]
        [TimelineEvent]
        private string _finishPutoutEvent;

        /// <summary>
        /// The TimelineEvent called when finish weapon put in.
        /// </summary>
        [SerializeField]
        [TimelineEvent]
        private string _finishPutinEvent;

        #endregion

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
            _timelineEventManager.RegisterEvent(_finishPutoutEvent, WeaponPutoutEvent);
            _timelineEventManager.RegisterEvent(_finishPutinEvent, WeaponPutinEvent);
        }

        /// <summary>
        /// Call this method to unregister the timeline events with corresponding weapon events.
        /// </summary>
        protected virtual void UnregisterTimelineEvents()
        {
            _timelineEventManager.UnRegisterEvent(_finishPutoutEvent, WeaponPutoutEvent);
            _timelineEventManager.UnRegisterEvent(_finishPutinEvent, WeaponPutinEvent);
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

        public virtual void OnInitialize()
        {
            // Convert the WeaponConfigData to the WeaponRuntimeData
            _runtimeData = (WeaponRuntimeData) _configData.ToRuntime();

            InitTSC();
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnPutOut()
        {
            // Register event listening.
            RegisterTimelineEvents();
            
            // Play the put out timeline.
            _timelineSystem.PlayTimeline(_runtimeData.putoutTimline);
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