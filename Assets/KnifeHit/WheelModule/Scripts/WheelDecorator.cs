using System;
using Assets.KnifeHit.GameSettings;
using Assets.KnifeHit.Knives;
using Pooling;
using UnityEngine;

namespace Assets.KnifeHit.WheelModule
{
    public class WheelDecorator : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D wheelBody;
        [SerializeField] private GameWideSettings gameWideSettings;
        
        private MonoPool<PreStuckKnifeBehaviour> _preStuckKnifePool;
        private Settings _settings;

        private void Awake()
        {
            _settings = gameWideSettings.WheelDecoratorSettings;
            _preStuckKnifePool = new MonoPool<PreStuckKnifeBehaviour>(_settings.knifePoolSettings,_settings.knifePrefab,wheelBody.transform);
            for (int i = 0; i < _settings.preStuckKnifeCount; i++)
            {
                PreStuckKnifeBehaviour preStuckKnife= _preStuckKnifePool.Spawn();
                SetPositionAndRotation(preStuckKnife);
            }
        }

        private void SetPositionAndRotation(PreStuckKnifeBehaviour preStuckKnife)
        {
            var positionOnWheel = UnityEngine.Random.insideUnitCircle;
            positionOnWheel = positionOnWheel.normalized * wheelBody.GetComponent<CircleCollider2D>().radius;
            var direction = -positionOnWheel;
            var preStuckKnifeTransform = preStuckKnife.transform;
            preStuckKnifeTransform.localPosition = positionOnWheel;
            preStuckKnifeTransform.up = direction;
        }

        [Serializable]
        public class Settings
        {
            public int preStuckKnifeCount;
            public PreStuckKnifeBehaviour knifePrefab;
            public MonoPoolSettings knifePoolSettings;
        }
    }
}