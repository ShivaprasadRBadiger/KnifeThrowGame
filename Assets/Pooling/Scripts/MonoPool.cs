using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pooling
{
    [Serializable]
    public class MonoPoolSettings
    {
        public int minSize = 4;
        public int maxSize = 20;
    }
    public class MonoPool<T> where T : MonoBehaviour
    {
        private MonoPoolSettings _settings;
        private  List<T> _inactiveObjects= new List<T>(); 
        private  List<T> _activeObjects= new List<T>();
        private Transform _poolParent;
        private T _prefab;

        public  MonoPool(MonoPoolSettings settings, T prefab,Transform underTransform=null)
        {
            _settings = settings;
            _poolParent = underTransform;
            _prefab = prefab;
            ExpandPool(_settings.minSize);
        }

        public T Spawn()
        {
            if (_inactiveObjects.Count==0)
            {
                if (_activeObjects.Count >= _settings.maxSize)
                {
                    Debug.LogError("Trying to spawn more than max limit of the pool!");
                    return null;
                }
                ExpandPool(1);
            }
           
            int lastIndex = _inactiveObjects.Count - 1;
            var spawnedObject=  _inactiveObjects[lastIndex];
            spawnedObject.gameObject.SetActive(true);

            _inactiveObjects.RemoveAt(lastIndex);
            _activeObjects.Add(spawnedObject);
            
            return spawnedObject;
        }

        public void Despawn(T item)
        {
            if (!_activeObjects.Contains(item))
            {
                Debug.LogError($"{item.name} not found in active object list of the pool!");
                return;
            }
            item.gameObject.SetActive(false);
            var transform = item.transform;
            transform.SetParent(_poolParent);
            transform.localPosition = Vector3.zero;
            transform.localRotation= Quaternion.identity;
            _activeObjects.Remove(item);
            _inactiveObjects.Add(item);
        }

        private void ExpandPool(int size)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject go =  Object.Instantiate(_prefab.gameObject, _poolParent, false);
                T monoBehaviour = go.GetComponent<T>();
                var transform = monoBehaviour.transform;
                transform.localPosition=Vector3.zero;
                transform.localRotation=Quaternion.identity;
                transform.SetSiblingIndex(i);
                go.SetActive(false);
                _inactiveObjects.Add(monoBehaviour);
            }
        }
    }
}