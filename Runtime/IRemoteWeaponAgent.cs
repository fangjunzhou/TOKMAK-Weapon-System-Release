namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The interface to call remote weapon RPC method.
    /// </summary>
    public interface IRemoteWeaponAgent<WeaponType> where WeaponType : IWeapon<WeaponType>
    {
        /// <summary>
        /// The WeaponManager.
        /// </summary>
        IWeaponManager<WeaponType> manager { get; set; }

        /// <summary>
        /// The method to call the RPC on remote weapons.
        /// </summary>
        /// <param name="index">The index of the weapon.</param>
        /// <param name="method">The method name.</param>
        /// <param name="methodParams">The method parameters.</param>
        void CallRPC(int index, string method, object[] methodParams);
    }
}