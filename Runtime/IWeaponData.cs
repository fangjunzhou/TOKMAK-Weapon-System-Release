using System;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// Two basic WeaponData types
    /// </summary>
    public enum WeaponDataType
    {
        /// <summary>
        /// The weapon data stored for weapon config.
        /// </summary>
        Config,
        /// <summary>
        /// The runtime WeaponData for Weapon System usage.
        /// </summary>
        Runtime
    }
    
    /// <summary>
    /// The interface for WeaponData
    /// </summary>
    public interface IWeaponData
    {
        /// <summary>
        /// The ID of the WeaponData
        /// </summary>
        string id { get; set; }
        
        /// <summary>
        /// The WeaponDataType of current WeaponData class.
        /// </summary>
        public WeaponDataType weaponDataType { get; }

        /// <summary>
        /// Get a deep copy of current WeaponData.
        /// </summary>
        /// <returns>Deep copy version of current WeaponData</returns>
        public IWeaponData DeepCopy();

        /// <summary>
        /// Convert Config WeaponData to Runtime WeaponData.
        /// </summary>
        /// <returns>The Runtime WeaponData generated from current Config WeaponData</returns>
        /// <exception cref="InvalidOperationException">when current WeaponData class is already a Runtime WeaponData.</exception>
        public IWeaponData ToRuntime();
    }
}