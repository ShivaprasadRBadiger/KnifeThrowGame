using Assets.KnifeHit.InGameScreens;
using Assets.KnifeHit.KnifeThrow;
using Assets.KnifeHit.Knives;
using Assets.KnifeHit.WheelModule;
using UnityEngine;

namespace Assets.KnifeHit.GameSettings
{
    [CreateAssetMenu(fileName = "NewGameSettings",menuName = "ScriptableObjects/GameWideSettings")]
    public class GameWideSettings:ScriptableObject
    {
        [SerializeField]
        private KnifeBehaviour.Settings knifeBehaviourSettings;
        public KnifeBehaviour.Settings KnifeBehaviourSettings=>knifeBehaviourSettings;
        
        [SerializeField]
        private WheelBehaviour.Settings wheelBehaviourSettings;
        public WheelBehaviour.Settings WheelBehaviourSettings=>wheelBehaviourSettings;

        [SerializeField]
        private KnifeThrowBehaviour.Settings knifeThrowSettings;
        public KnifeThrowBehaviour.Settings KnifeThrowSettings=>knifeThrowSettings;
        
        [SerializeField]
        private RemainingKnifeView.Settings remainingKnifeViewSettings;
        public RemainingKnifeView.Settings RemainingKnifeViewSettings=>remainingKnifeViewSettings;
        
        [SerializeField]
        private WheelDecorator.Settings wheelDecoratorSettings;
        public WheelDecorator.Settings WheelDecoratorSettings=>wheelDecoratorSettings;
    
    }
}