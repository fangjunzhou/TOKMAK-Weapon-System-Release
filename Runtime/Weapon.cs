using System.Threading.Tasks;
using UnityEngine;

namespace FinTOKMAK.WeaponSystem.Runtime
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "FinTOKMAK/Weapon System/Weapon", order = 0)]
    public class Weapon : ScriptableObject, IWeapon
    {
        public string id { get; }
        
        public void OnInitialize()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void OnPutOut()
        {
            throw new System.NotImplementedException();
        }

        public Task OnPutOutAsync()
        {
            throw new System.NotImplementedException();
        }

        public void OnPutIn()
        {
            throw new System.NotImplementedException();
        }

        public Task OnPutInAsync()
        {
            throw new System.NotImplementedException();
        }

        public void OnTriggerDown()
        {
            throw new System.NotImplementedException();
        }

        public void OnTriggerUp()
        {
            throw new System.NotImplementedException();
        }

        public void OnReload()
        {
            throw new System.NotImplementedException();
        }
    }
}