using System.Threading.Tasks;

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
        /// The async method to put out weapon.
        /// Called when the weapon is put out.
        /// Finish the async when the put out process complete (finish put out animation).
        /// </summary>
        /// <returns>Async Task</returns>
        Task OnPutOutAsync();

        /// <summary>
        /// The callback method called when the weapon is put in.
        /// </summary>
        void OnPutIn();

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
    }
}