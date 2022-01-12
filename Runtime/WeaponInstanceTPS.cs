using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The TPS Weapon instance.
    /// </summary>
    public class WeaponInstanceTPS : MonoBehaviour
    {
        #region Private Field

        /// <summary>
        /// The weapon instantiating this instance.
        /// </summary>
        private Weapon _weapon;

        #endregion

        #region Public Field

        public Weapon weapon => _weapon;

        #endregion
    }
}