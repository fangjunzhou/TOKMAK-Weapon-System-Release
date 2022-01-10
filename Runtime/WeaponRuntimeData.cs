using System;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    public class WeaponRuntimeData : ScriptableObject, IWeaponData
    {
        #region Private Field

        /// <summary>
        /// The id of the
        /// </summary>
        [SerializeField]
        private string _id;

        #endregion
        
        #region IWeaponData Interface

        public string id
        {
            get => _id;
            set => _id = value;
        }

        public WeaponDataType weaponDataType => WeaponDataType.Runtime;

        public IWeaponData DeepCopy()
        {
            return ScriptableObject.Instantiate(this);
        }

        public IWeaponData ToRuntime()
        {
            throw new InvalidOperationException("Current WeaponData is already a runtime data.");
        }

        #endregion
    }
}