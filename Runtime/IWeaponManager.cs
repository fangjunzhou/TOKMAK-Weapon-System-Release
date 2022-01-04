using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The interface of WeaponManager
    /// </summary>
    public interface IWeaponManager
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
        /// All the weapon current manager is carrying.
        /// </summary>
        List<IWeapon> carryWeapons { get; }

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
        void PutOut(IWeapon weapon);

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
        Task PutOutAsync(IWeapon weapon);

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
    }
}