using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The TPS Weapon instance.
    /// </summary>
    public class WeaponInstanceTPS<ConfigType, RuntimeType> : MonoBehaviour where ConfigType : WeaponConfigData where RuntimeType : WeaponRuntimeData
    {
        #region Private Field

        /// <summary>
        /// The weapon instantiating this instance.
        /// </summary>
        private Weapon<ConfigType, RuntimeType> _weapon;

        #endregion

        #region Public Field

        public Weapon<ConfigType, RuntimeType> weapon => _weapon;

        #endregion
    }
}