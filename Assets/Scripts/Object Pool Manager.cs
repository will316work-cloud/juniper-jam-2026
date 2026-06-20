using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

namespace Spawners
{
    /// <summary>
    /// Types of pools to reference.
    /// </summary>
    public enum PoolType
    {
        GameObjects,
        ParticleSystem,
        SoundFX,
        UI
    }

    /// <summary>
    /// Manages spawning pools for all objects in the scene.
    /// 
    /// Author: William Min
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private bool _addToDontDestroyOnLoad = false;  // True if this manager will persist between loading scenes


        #endregion

        #region Private Fields


        private GameObject _emptyHolder;    // GameObject reference to the main holder of pool holders

        private static GameObject _particleSystemEmpty; // GameObject reference to the Particle holder
        private static GameObject _gameObjectsEmpty;    // GameObject reference to the GameObject holder
        private static GameObject _soundFXEmpty;        // GameObject reference to the Sound Effects holder
        private static GameObject _UIEmpty;             // GameObject reference to the UI holder

        private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools; // Mapping between a prefab to its pool to spawn and return in
        private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;        // Mapping between a clone to its prefab origin


        #endregion

        #region MonoBehavior Callbacks


        private void Awake()
        {
            _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();    // Mapping between prefab and pool
            _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();           // Mapping between clone instance and its prefab origin

            SetUpEmpties();
        }


        #endregion

        #region Pool Setup


        // Sets up empty gameObjects to store clones
        private void SetUpEmpties()
        {
            _emptyHolder = new GameObject("ObjectPools");

            _particleSystemEmpty = new GameObject("Particle Effects");
            _particleSystemEmpty.transform.SetParent(_emptyHolder.transform);

            _gameObjectsEmpty = new GameObject("GameObjects");
            _gameObjectsEmpty.transform.SetParent(_emptyHolder.transform);

            _soundFXEmpty = new GameObject("Sound FX");
            _soundFXEmpty.transform.SetParent(_emptyHolder.transform);

            _UIEmpty = new GameObject("UI");
            _UIEmpty.transform.SetParent(_emptyHolder.transform);

            if (_addToDontDestroyOnLoad)
                DontDestroyOnLoad(_particleSystemEmpty.transform.root);
        }

        // Creates an ObjectPool for a given prefab
        private static void _createPool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: () => _createObject(prefab, pos, rot, poolType),
                actionOnGet: _onGetObject,
                actionOnRelease: _onReleaseObject,
                actionOnDestroy: _onDestroyObject
                );

            _objectPools.Add(prefab, pool);
        }

        // Creates a new GameObject and stores it into a pool
        private static GameObject _createObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
        {
            //prefab.SetActive(false);

            GameObject obj = Instantiate(prefab, pos, rot);

            //prefab.SetActive(true);

            GameObject parentObject = _setParentObject(poolType);
            obj.transform.SetParent(parentObject.transform);

            return obj;
        }

        // Pulls an object from the pool
        private static void _onGetObject(GameObject obj)
        {
            // optional logic
        }

        // Releases an object back into the pool
        private static void _onReleaseObject(GameObject obj)
        {
            obj.SetActive(false);
        }

        // Destroys an object
        private static void _onDestroyObject(GameObject obj)
        {
            if (_cloneToPrefabMap.ContainsKey(obj))
                _cloneToPrefabMap.Remove(obj);
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Spawns an object from the pool and 
        /// returns its component based on the requested component type.
        /// </summary>
        /// <typeparam name="T">Type of component</typeparam>
        /// <param name="typePrefab">Component of the prefab</param>
        /// <param name="spawnPos">Spawn position</param>
        /// <param name="spawnRotation">Spawn rotation</param>
        /// <param name="poolType">Pool type to spawn into</param>
        /// <param name="onSpawnObject">Additional actions for when spawning an object</param>
        /// <returns>Component attached to the spawned object</returns>
        public static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
        {
            GameObject objectToSpawn = typePrefab.gameObject;
            GameObject spawnedObject = _spawnBaseObject(objectToSpawn, spawnPos, spawnRotation, poolType);
            T component = _getComponentFromSpawned<T>(spawnedObject, objectToSpawn.name);

            return component;
        }

        /// <summary>
        /// Spawns and returns an object from the pool.
        /// </summary>
        /// <typeparam name="T">Type of component</typeparam>
        /// <param name="objectToSpawn">Prefab GameObject</param>
        /// <param name="spawnPos">Spawn position</param>
        /// <param name="spawnRotation">Spawn rotation</param>
        /// <param name="poolType">Pool type to spawn into</param>
        /// <param name="onSpawnObject">Additional actions for when spawning an object</param>
        /// <returns>GameObject reference to spawned object</returns>
        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
        {
            return _spawnBaseObject(objectToSpawn, spawnPos, spawnRotation, poolType);
        }

        /// <summary>
        /// Returns a spawned object into its pool.
        /// </summary>
        /// <param name="obj">GameObject reference to the spawned object</param>
        /// <param name="poolType">Pool type to spawn into</param>
        /// <param name="onDespawnObject">Additional actions for when returning an object</param>
        public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
        {
            if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
            {
                GameObject parentObject = _setParentObject(poolType);

                // Sets parent to cooresponding pool type
                if (obj.transform.parent != parentObject.transform)
                {
                    obj.transform.SetParent(parentObject.transform);
                }

                // Releases object if its prefab has a pool
                if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
                {
                    pool.Release(obj);
                }
            }
            else
            {
                Debug.LogWarning($"Trying to return on object that is not pooled: {obj.name}");
                Destroy(obj);
            }
        }

        /// <summary>
        /// Returns the prefab that spawned the spawned instance.
        /// </summary>
        /// <param name="spawnedObject">GameObject reference of spawned instance</param>
        /// <returns>GameObject reference of the prefab that spawned the spawned instance</returns>
        public static GameObject GetPrefabFromInstance(GameObject spawnedObject)
        {
            if (_cloneToPrefabMap.ContainsKey(spawnedObject))
            {
                return _cloneToPrefabMap[spawnedObject];
            }
            else
            {
                return null;
            }
        }


        #endregion

        #region Private Methods


        // Spawns and gives a base GameObject
        private static GameObject _spawnBaseObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
        {
            // Create new pool if prefab doesn't have a pool
            if (!_objectPools.ContainsKey(objectToSpawn))
            {
                _createPool(objectToSpawn, spawnPos, spawnRotation, poolType);
            }

            GameObject obj = _objectPools[objectToSpawn].Get();

            if (obj != null)
            {
                //Add spawned object to map with its prefab if it's not in the map
                if (!_cloneToPrefabMap.ContainsKey(obj))
                {
                    _cloneToPrefabMap.Add(obj, objectToSpawn);
                }

                obj.transform.position = spawnPos;
                obj.transform.rotation = spawnRotation;
                obj.SetActive(true);

                return obj;
            }

            return null;
        }

        // Returns the parent GameObject corresponding to the pool type
        private static GameObject _setParentObject(PoolType poolType)
        {
            switch (poolType)
            {
                case PoolType.ParticleSystem:

                    return _particleSystemEmpty;

                case PoolType.GameObjects:

                    return _gameObjectsEmpty;

                case PoolType.SoundFX:

                    return _soundFXEmpty;

                case PoolType.UI:

                    return _UIEmpty;

                default:
                    return null;
            }
        }

        // Returns a component from a spawned object if it has one
        private static T _getComponentFromSpawned<T>(GameObject spawnedObject, string prefabName) where T : Component
        {
            if (spawnedObject != null)
            {
                if (spawnedObject.TryGetComponent(out T component))
                {
                    return component;
                }
                else
                {
                    Debug.LogError($"Object {prefabName} doesn't have component of type {typeof(T)}");
                    return null;
                }
            }

            return null;
        }


        #endregion
    }
}
