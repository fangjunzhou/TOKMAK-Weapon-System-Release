using System.Threading.Tasks;
using FinTOKMAK.TimelineSystem.Runtime;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The interface for Weapon class
    /// </summary>
    public interface IWeapon
    {
        #region Property

        /// <summary>
        /// The unique id of the weapon.
        /// </summary>
        string id { get; }
        
        /// <summary>
        /// The config data of the weapon.
        /// </summary>
        IWeaponData configData { get; }
        
        /// <summary>
        /// The runtime data of the weapon.
        /// </summary>
        IWeaponData runtimeData { get; }
        
        /// <summary>
        /// The weapon manager carrying the current Weapon.
        /// </summary>
        IWeaponManager weaponManager { get; set; }
        
        /// <summary>
        /// The TimelineSystem with the WeaponManager.
        /// </summary>
        TimelineSystem.Runtime.TimelineSystem timelineSystem { get; set; }
        
        /// <summary>
        /// The TimelineEventManager used by the Timeline System.
        /// </summary>
        TimelineEventManager timelineEventManager { get; set; }

        #endregion

        #region Weapon Logic Callback

        /// <summary>
        /// The method called when the weapon is initialized for the first time.
        /// </summary>
        void OnInitialize();

        /// <summary>
        /// The method is called each Unity Update when the weapon is put out and able to use.
        /// </summary>
        void OnUpdate();

        #endregion
        
        /// <summary>
        /// The callback function called when the weapon is put out.
        /// This method should put out the weapon immediately.
        /// </summary>
        void OnPutOut();

        /// <summary>
        /// The callback function called when the weapon finish put out.
        /// This method should enable the weapon fire status and restore some data.
        /// </summary>
        void OnFinishPutOut();

        /// <summary>
        /// The async method to put out weapon.
        /// Called when the weapon is put out.
        /// Finish the async when the put out process complete (finish put out animation).
        /// </summary>
        /// <returns>Async Task</returns>
        Task OnPutOutAsync();

        /// <summary>
        /// The callback method called when the weapon is put in.
        /// This method should disable some weapon function such as reload and fire.
        /// </summary>
        void OnPutIn();

        /// <summary>
        /// The callback method called when the weapon finish put in.
        /// This method should store some weapon state.
        /// </summary>
        void OnFinishPutIn();

        /// <summary>
        /// The async method to put in weapon.
        /// Called when the weapon is put in.
        /// Finish the async when the put in process complete (finish put in animation).
        /// </summary>
        /// <returns>Async Task</returns>
        Task OnPutInAsync();

        /// <summary>
        /// Callback function called when the weapon trigger is pull down.
        /// </summary>
        void OnTriggerDown();

        /// <summary>
        /// Callback function called when the weapon trigger is up.
        /// </summary>
        void OnTriggerUp();

        /// <summary>
        /// Callback function called when the weapon reload key is pressed down.
        /// </summary>
        void OnReloadDown();

        /// <summary>
        /// Callback function called when the weapon reload key is up.
        /// </summary>
        void OnReloadUp();

        /// <summary>
        /// The callback function called when the player start aiming.
        /// </summary>
        void OnAimStart();

        /// <summary>
        /// The callback function called when the player stop aiming.
        /// </summary>
        void OnAimStopped();
    }
}