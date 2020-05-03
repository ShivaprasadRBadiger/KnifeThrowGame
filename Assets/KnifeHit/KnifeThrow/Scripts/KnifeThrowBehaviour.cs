using System;
using System.Collections.Generic;
using Assets.KnifeHit.GameSettings;
using Assets.KnifeHit.InGameScreens;
using Assets.KnifeHit.Knives;
using EventSystem;
using Pooling;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.KnifeHit.KnifeThrow
{
    public class KnifeThrowBehaviour : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private GameWideSettings gameWideSettings;
        private Settings _settings;

        
        private MonoPool<KnifeBehaviour> _knifePool;
        private List<KnifeBehaviour> _activeKnives;
        private KnifeBehaviour _currentKnifeToThrow;
        private Transform _knifePoolParent;
        private RemainingKnifeView _remainingKnifeView;

      

        private void Awake()
        {
            _settings = gameWideSettings.KnifeThrowSettings;
            _remainingKnifeView = FindObjectOfType<RemainingKnifeView>();
            var knifeSpawnPoint = GameObject.FindWithTag("Respawn");
            _knifePoolParent = knifeSpawnPoint.transform;
            _activeKnives= new List<KnifeBehaviour>();
            _knifePool= new MonoPool<KnifeBehaviour>(_settings.poolSettings,_settings.knifePrefab,_knifePoolParent);
            
            EventManager.RegisterHandler(CustomEventType.OnLevelCompleted,OnLevelCompleteHandler);
            EventManager.RegisterHandler(CustomEventType.OnLevelFailed,OnLevelCompleteHandler);
        }

        private void OnEnable()
        {
            ReadyNextKnife();
        }

        private void OnLevelCompleteHandler(object obj)
        {
            gameObject.SetActive(false);
        }

        void ReadyNextKnife()
        {
            if (_activeKnives.Count >= _settings.availableKnives)
            {
                Debug.LogWarning("Total knife limit reached for the level! End of level?!");
                _currentKnifeToThrow = null;
                return;
            }
            var knife = _knifePool.Spawn();
            _currentKnifeToThrow = knife;
            _activeKnives.Add(knife);
        }

        [Serializable]
        public class Settings
        {
            public KnifeBehaviour knifePrefab;
            public int availableKnives;
            public MonoPoolSettings poolSettings;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_currentKnifeToThrow == null)
            {
                return;
            }
            _currentKnifeToThrow.Throw();
            _remainingKnifeView.OnUserSpentKnife();
            ReadyNextKnife();
        }

        private void OnDestroy()
        {
           EventManager.Dispose();
        }

        private void Dispose()
        {
            foreach (var knife in _activeKnives)
            {
               _knifePool.Despawn(knife);
            }
            _activeKnives.Clear();
        }
    }
}
