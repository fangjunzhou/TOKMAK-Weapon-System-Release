using FinTOKMAK.TimelineSystem.Runtime;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    [CreateAssetMenu(fileName = "Weapon Config Data", menuName = "FinTOKMAK/Weapon System/Weapon Data/Config Data", order = 0)]
    public class WeaponConfigData : ScriptableObject, IWeaponData
    {
        #region Serialized Private Field

        /// <summary>
        /// The id of the weapon.
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
        private Timeline _putinTimeline;

        #endregion

        #endregion

        #region Public Field

        /// <summary>
        /// The timeline played when put out the weapon.
        /// </summary>
        public Timeline putoutTimeline => _putoutTimeline;

        /// <summary>
        /// The timeline played when put in the weapon.
        /// </summary>
        public Timeline putinTimeline => _putinTimeline;

        #endregion
        
        #region IWeaponData Interface

        public string id
        {
            get => _id;
            set => _id = value;
        }

        public WeaponDataType weaponDataType => WeaponDataType.Config;

        public IWeaponData DeepCopy()
        {
            return ScriptableObject.Instantiate(this);
        }

        public virtual IWeaponData ToRuntime()
        {
            WeaponRuntimeData runtimeData = ScriptableObject.CreateInstance<WeaponRuntimeData>();
            runtimeData.id = _id;
            runtimeData.putoutTimline = _putoutTimeline;
            runtimeData.putinTimeline = _putinTimeline;
            return runtimeData;
        }

        #endregion
    }
}