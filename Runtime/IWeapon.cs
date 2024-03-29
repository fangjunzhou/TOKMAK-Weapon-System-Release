﻿using System.Threading.Tasks;
using FinTOKMAK.TimelineSystem.Runtime;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The interface for Weapon class
    /// </summary>
    public interface IWeapon<WeaponType> where WeaponType : IWeapon<WeaponType>
    {
        #region Property

        /// <summary>
        /// The unique id of the weapon.
        /// </summary>
        string id { get; }
        
        /// <summary>
        /// The index of weapon in the manager.
        /// </summary>
        int index { get; set; }
        
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
        IWeaponManager<WeaponType> weaponManager { get; set; }
        
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
        /// Mount the weapon instance to a certain mount point.
        /// </summary>
        /// <param name="instance">The weapon instance GameObject.</param>
        /// <param name="mountPoint">The name of the mount point.</param>
        void Mount(GameObject instance, string mountPoint);
        
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
        /// Callback function called when the WeaponManager's _able2Shoot changed.
        /// </summary>
        /// <param name="enable">the new _able2Shoot value.</param>
        void OnShootEnableChanged(bool enable);

        /// <summary>
        /// Callback function called when the weapon reload key is pressed down.
        /// </summary>
        void OnReloadDown();

        /// <summary>
        /// Callback function called when the weapon reload key is up.
        /// </summary>
        void OnReloadUp();

        /// <summary>
        /// Callback function called when the WeaponManager's _able2Reload changed.
        /// </summary>
        /// <param name="enable">the new _able2Reload value.</param>
        void OnReloadEnableChanged(bool enable);

        /// <summary>
        /// The callback function called when the player start aiming.
        /// </summary>
        void OnAimDown();

        /// <summary>
        /// The callback function called when the player stop aiming.
        /// </summary>
        void OnAimUp();

        /// <summary>
        /// The callback function called when the WeaponManager's _able2Aim changed.
        /// </summary>
        /// <param name="enable">the new _able2Aim value.</param>
        void OnAimEnableChanged(bool enable);
    }
}