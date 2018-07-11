using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework {

	[Serializable]
	public class PoolInfo {
		public string poolName;
		public GameObject prefab;
		public int poolSize;
		public bool fixedSize;
	}

	public class GameObjectPool {
        private int maxSize;
		private int poolSize;
		private string poolName;
        private Transform poolRoot;
        private GameObject poolObjectPrefab;
        private Stack<GameObject> availableObjStack = new Stack<GameObject>();

        public GameObjectPool(string poolName, GameObject poolObjectPrefab, int initCount, int maxSize, Transform pool) {
			this.poolName = poolName;
			this.poolSize = initCount;
            this.maxSize = maxSize;
            this.poolRoot = pool;
            this.poolObjectPrefab = poolObjectPrefab;

			//populate the pool
			for(int index = 0; index < initCount; index++) {
				AddObjectToPool(NewObjectInstance());
			}
		}

		//o(1)
        private void AddObjectToPool(GameObject go) {
			//add to pool
            go.SetActive(false);
            availableObjStack.Push(go);
            go.transform.SetParent(poolRoot, false);
		}

        private GameObject NewObjectInstance() {
            return GameObject.Instantiate(poolObjectPrefab) as GameObject;
		}

		public GameObject NextAvailableObject() {
            GameObject go = null;
			if(availableObjStack.Count > 0) {
				go = availableObjStack.Pop();
			} else {
				Debug.LogWarning("No object available & cannot grow pool: " + poolName);
			}
            go.SetActive(true);
            return go;
		} 
		
		//o(1)
        public void ReturnObjectToPool(string pool, GameObject po) {
            if (poolName.Equals(pool)) {
                AddObjectToPool(po);
			} else {
				Debug.LogError(string.Format("Trying to add object to incorrect pool {0} ", poolName));
			}
		}
	}
}
