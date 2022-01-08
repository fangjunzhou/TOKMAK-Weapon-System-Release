using System.Threading.Tasks;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "FinTOKMAK/Weapon System/Weapon", order = 0)]
    public class Weapon : ScriptableObject, IWeapon
    {
        public string id { get; }
        
        public virtual void OnInitialize()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnPutOut()
        {
            
        }

        public async Task OnPutOutAsync()
        {
        }

        public virtual void OnPutIn()
        {
            
        }

        public async Task OnPutInAsync()
        {
        }

        public virtual void OnTriggerDown()
        {
            
        }

        public virtual void OnTriggerUp()
        {
            
        }

        public virtual void OnReload()
        {
            
        }
    }
}