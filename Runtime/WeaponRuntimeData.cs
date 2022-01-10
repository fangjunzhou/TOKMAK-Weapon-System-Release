using System;
using FinTOKMAK.TimelineSystem.Runtime;
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

        #region Timelines

        /// <summary>
        /// The timeline played when put out the weapon.
        /// </summary>
        [SerializeField]
        private Timeline _putoutTimeline;

        /// <summary>
        /// The timeline played when put in the weapon.
        /// </summary>
        [SerializeField]
        private Timeline _putinTimline;

        #endregion

        #endregion

        #region Public Field

        /// <summary>
        /// The timeline played when put out the weapon.
        /// </summary>
        public Timeline putoutTimline
        {
            get => _putoutTimeline;
            set => _putoutTimeline = value;
        }

        /// <summary>
        /// The timeline played when put in the weapon.
        /// </summary>
        public Timeline putinTimeline
        {
            get => _putinTimline;
            set => _putinTimline = value;
        }

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