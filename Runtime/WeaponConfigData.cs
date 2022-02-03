using System.Collections.Generic;
using AudioSystem.Runtime;
using FinTOKMAK.TimelineSystem.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    public class WeaponConfigData : ScriptableObject, IWeaponData
    {
        #region Serialized Private Field

        /// <summary>
        /// The id of the weapon.
        /// </summary>
        [SerializeField]
        [BoxGroup("Info")]
        private string _id;

        #region Timelines

        /// <summary>
        /// The timeline played when put out the weapon.
        /// </summary>
        [SerializeField]
        [BoxGroup("Timeline")]
        private Timeline _putoutTimeline;

        /// <summary>
        /// The timeline played when put in the weapon.
        /// </summary>
        [SerializeField]
        [BoxGroup("Timeline")]
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
        
        /// <summary>
        /// The audio config of the weapon.
        /// </summary>
        [BoxGroup("Audio")]
        public List<WeaponAudioEventConfig> audioEventConfigs;

        /// <summary>
        /// The audio player prefab weapon use.
        /// </summary>
        [FormerlySerializedAs("playerPrefab")]
        [BoxGroup("Audio")]
        public AudioPlayer audioPlayerPrefab;

        /// <summary>
        /// The mount point of all the audio players.
        /// </summary>
        [BoxGroup("Audio")]
        public string audioMountPoint;

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