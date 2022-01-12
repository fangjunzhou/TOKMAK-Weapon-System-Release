using System;
using System.Runtime.Serialization;
using FinTOKMAK.TimelineSystem.Runtime;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    /// <summary>
    /// The weapon usage status.
    /// </summary>
    public enum WeaponUsingStatus
    {
        /// <summary>
        /// The weapon is currently being used.
        /// </summary>
        Using,
        /// <summary>
        /// The weapon is not being used by the WeaponManager, buy carried by the manager.
        /// </summary>
        Background,
        /// <summary>
        /// The weapon is not carried by any WeaponManager.
        /// </summary>
        Abandoned
    }
    
    [System.Serializable]
    public class WeaponRuntimeData : ScriptableObject, IWeaponData, ISerializable
    {
        #region Private Field

        /// <summary>
        /// The current weapon using status.
        /// </summary>
        private WeaponUsingStatus _usingStatus;

        #endregion
        
        #region Serialized Private Field

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
        /// The current weapon using status.
        /// </summary>
        public WeaponUsingStatus usingStatus
        {
            get => _usingStatus;
            set => _usingStatus = value;
        }

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
        
        #region ISerializable

        /// <summary>
        /// The constructor used by ISerializable
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public WeaponRuntimeData(SerializationInfo info, StreamingContext context)
        {
            DeserializeObjectData(info, context);
        }

        public virtual void DeserializeObjectData(SerializationInfo info, StreamingContext context)
        {
            _id = (string) info.GetValue("id", typeof(string));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", _id, typeof(string));
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