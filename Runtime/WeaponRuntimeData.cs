using System;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    public class WeaponRuntimeData : ScriptableObject, IWeaponData
    {
        #region Public Field

        /// <summary>
        /// The id of the
        /// </summary>
        public string _id;

        #endregion
        
        #region IWeaponData Interface

        public string id => _id;

        public WeaponDataType weaponDataType => WeaponDataType.Runtime;

        public IWeaponData DeepCopy()
        {
            return ScriptableObject.CreateInstance<WeaponRuntimeData>();
        }

        public IWeaponData ToRuntime()
        {
            throw new InvalidOperationException("Current WeaponData is already a runtime data.");
        }

        #endregion
    }
}