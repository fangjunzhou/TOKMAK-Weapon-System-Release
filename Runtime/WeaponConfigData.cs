using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    [CreateAssetMenu(fileName = "Weapon Config Data", menuName = "FinTOKMAK/Weapon System/Weapon Data/Config Data", order = 0)]
    public class WeaponConfigData : ScriptableObject, IWeaponData
    {
        #region Public Field

        /// <summary>
        /// The id of the weapon.
        /// </summary>
        public string _id;

        #endregion
        
        #region IWeaponData Interface

        public string id => _id;

        public WeaponDataType weaponDataType => WeaponDataType.Config;

        public IWeaponData DeepCopy()
        {
            throw new System.NotImplementedException();
        }

        public IWeaponData ToRuntime()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}