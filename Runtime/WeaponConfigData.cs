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
        [SerializeField]
        private string _id;

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

        public IWeaponData ToRuntime()
        {
            WeaponRuntimeData runtimeData = ScriptableObject.CreateInstance<WeaponRuntimeData>();
            runtimeData.id = _id;
            return runtimeData;
        }

        #endregion
    }
}