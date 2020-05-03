using System;
using System.Collections.Generic;
using Assets.KnifeHit.GameSettings;
using Pooling;
using UnityEngine;

namespace Assets.KnifeHit.InGameScreens
{
    public class RemainingKnifeView : MonoBehaviour
    {
        [SerializeField] private GameWideSettings gameWideSettings;
      
        private MonoPool<KnifeItemWidget> _knifeItemWidgetPool;
        private List<KnifeItemWidget> _activeItems;
        //Index of _activeItems pointing to current knife that will be turned off.
        private int _currentKnifeIndex;
        private Settings _settings;

        private void Awake()
        {
            _settings = gameWideSettings.RemainingKnifeViewSettings;
        }

        private void OnEnable()
        {
            InitView(gameWideSettings.KnifeThrowSettings.availableKnives);
        }

        public void InitView(int totalKnives)
        {
            
            if (totalKnives> _settings.WidgetPoolSettings.maxSize)
            {
                Debug.LogError("Requesting more items than pooled, please change pooling settings of the this game object.");
                return;
            }
            _knifeItemWidgetPool = new MonoPool<KnifeItemWidget>(_settings.WidgetPoolSettings,_settings.KnifeItemPrefab,transform);
            _activeItems= new List<KnifeItemWidget>();
            for (int i = 0; i < totalKnives; i++)
            {
                var item = _knifeItemWidgetPool.Spawn();
                item.IsSpent = false;
                 _activeItems.Add(item);
            }
            _currentKnifeIndex = _activeItems.Count-1;
        }

        public void OnUserSpentKnife()
        {
            _activeItems[_currentKnifeIndex].IsSpent = true;
            if(_currentKnifeIndex>0)
                _currentKnifeIndex--;
        }

        void Dispose()
        {
            for (int i = 0; i < _activeItems.Count; i++)
            {
                _knifeItemWidgetPool.Despawn(_activeItems[i]);
            }
            _activeItems.Clear();
        }
        
        [Serializable]
        public class Settings
        {
            public KnifeItemWidget KnifeItemPrefab;
            public MonoPoolSettings WidgetPoolSettings;
        }
    }
}
