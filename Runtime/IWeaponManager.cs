using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The serializable dictionary that stores all the mount point that can mount weapon instance.
    /// </summary>
    [System.Serializable]
    public class WeaponMountPointDict : SerializableDictionary<string, Transform>
    {
        
    }

    /// <summary>
    /// All the state current WeaponManager is in.
    /// </summary>
    public enum WeaponManagerState
    {
        /// <summary>
        /// There's no weapon in hand.
        /// </summary>
        Empty,
        /// <summary>
        /// The manager is putting out weapon.
        /// </summary>
        PuttingOut,
        /// <summary>
        /// The manager is putting in weapon.
        /// </summary>
        PuttingIn,
        /// <summary>
        /// The manager is ready to fire.
        /// </summary>
        Ready
    }
    
    /// <summary>
    /// The interface of WeaponManager
    /// </summary>
    public interface IWeaponManager<WeaponType> where WeaponType : IWeapon<WeaponType>
    {
        #region Events

        /// <summary>
        /// The Action event triggered when the WeaponManager start initialization.
        /// </summary>
        Action onInitialize { get; }
        
        /// <summary>
        /// The Action event triggered when the WeaponManager finish initialization.
        /// </summary>
        Action onFinishInitialize { get; }

        #endregion

        /// <summary>
        /// If the WeaponManager has local authority.
        /// </summary>
        bool isLocal { get; set; }
        
        /// <summary>
        /// The dictionary that stores all the mount points that can mount weapon instance.
        /// </summary>
        WeaponMountPointDict weaponMountPoint { get; }
        
        /// <summary>
        /// All the weapon current manager is carrying.
        /// </summary>
        List<WeaponType> carryWeapons { get; }
        
        /// <summary>
        /// The currently using weapon.
        /// </summary>
        WeaponType currWeapon { get; }
        
        /// <summary>
        /// The index of current Weapon.
        /// </summary>
        int currIndex { get; }
        
        /// <summary>
        /// The current state of WeaponManager
        /// </summary>
        WeaponManagerState state { get; }

        /// <summary>
        /// If the weapon is currently able to shoot;
        /// </summary>
        bool able2Shoot { get; set; }
        
        /// <summary>
        /// If the weapon is currently able to aim.
        /// </summary>
        bool able2Aim { get; set; }
        
        /// <summary>
        /// If the weapon is currently able to reload.
        /// </summary>
        bool able2Reload { get; set; }

        /// <summary>
        /// Add a new weapon to the last of carry weapon list.
        /// </summary>
        /// <param name="weapon">The IWeapon instance to add.</param>
        void AddWeapon(WeaponType weapon);

        /// <summary>
        /// Add a new weapon to the carry weapon list.
        /// </summary>
        /// <param name="weapon">The IWeapon instance to add.</param>
        /// <param name="index">The index to add the new weapon.</param>
        void AddWeapon(WeaponType weapon, int index);

        /// <summary>
        /// Remove a exist weapon from the carry weapon list.
        /// </summary>
        /// <param name="index">The index of the weapon to remove.</param>
        /// <returns>The removed IWeapon instance.</returns>
        WeaponType RemoveWeapon(int index);

        /// <summary>
        /// Remove a exist weapon from the carry weapon list.
        /// </summary>
        /// <param name="id">The id of the weapon to remove.</param>
        /// <returns>The removed IWeapon instance.</returns>
        WeaponType RemoveWeapon(string id);

        /// <summary>
        /// Put out a weapon using the weapon index in the carryWeapon list.
        /// When there's no weapon in the hand, the WeaponManager will put out weapon directly.
        /// When there's already a weapon in hand, the WeaponManager will put in current using weapon first,
        /// then put out the target weapon.
        /// </summary>
        /// <param name="index">the index of the weapon in the caryWeapons list, start from 0.</param>
        void PutOut(int index);

        /// <summary>
        /// Put out a weapon using the weapon unique string id.
        /// When there's no weapon in the hand, the WeaponManager will put out weapon directly.
        /// When there's already a weapon in hand, the WeaponManager will put in current using weapon first,
        /// then put out the target weapon.
        /// </summary>
        /// <param name="id">the unique string id of the weapon</param>
        void PutOut(string id);

        /// <summary>
        /// Put out a weapon using the weapon object.
        /// When there's no weapon in the hand, the WeaponManager will put out weapon directly.
        /// When there's already a weapon in hand, the WeaponManager will put in current using weapon first,
        /// then put out the target weapon.
        /// </summary>
        /// <param name="weapon">the weapon to put out.</param>
        void PutOut(WeaponType weapon);

        /// <summary>
        /// The async version of method <see cref="PutOut(int)"/>
        /// </summary>
        /// <param name="index">the index of the weapon in the caryWeapons list, start from 0.</param>
        /// <returns>Async Task</returns>
        Task PutOutAsync(int index);
        
        /// <summary>
        /// The async version of method <see cref="PutOut(string)"/>
        /// </summary>
        /// <param name="id">the unique string id of the weapon</param>
        /// <returns>Async Task</returns>
        Task PutOutAsync(string id);
        
        /// <summary>
        /// The async version of method <see cref="PutOut(IWeapon)"/>
        /// </summary>
        /// <param name="weapon">the weapon to put out.</param>
        /// <returns>Async Task</returns>
        Task PutOutAsync(WeaponType weapon);

        /// <summary>
        /// Put in the current weapon.
        /// </summary>
        void PutIn();

        /// <summary>
        /// The async version of method <see cref="PutIn"/>
        /// </summary>
        /// <returns>Async Task</returns>
        Task PutInAsync();


        /// <summary>
        /// The method to pull trigger down.
        /// </summary>
        void TriggerDown();

        /// <summary>
        /// The method to release the trigger.
        /// </summary>
        void TriggerUp();

        /// <summary>
        /// The method called to press down the reload key.
        /// Often start the reload process.
        /// </summary>
        void ReloadDown();

        /// <summary>
        /// The method to release the reload key.
        /// </summary>
        void ReloadUp();

        /// <summary>
        /// The method to start aiming the current weapon.
        /// </summary>
        void AimDown();

        /// <summary>
        /// The method to stop aiming the current weapon.
        /// </summary>
        void AimUp();
    }
}