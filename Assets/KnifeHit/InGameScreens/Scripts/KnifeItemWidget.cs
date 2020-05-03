using UnityEngine;
using UnityEngine.UI;

namespace Assets.KnifeHit.InGameScreens
{
    public class KnifeItemWidget:MonoBehaviour
    {
        [SerializeField] private Toggle toggle;

        public bool IsSpent
        {
            set => toggle.isOn = !value;
            get => !toggle.isOn;
        }
    }
}