using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    #region PoolClass
    [System.Serializable]
    public class Pool
    {
        public GameObject objectToPool;
        public int amountToPool;
        public bool canExpand;
    }
    #endregion
    
    #region Singleton
    public static ObjectPoolManager Instance;
    void Awake() 
    {
        if(Instance==null) Instance=this;
	}
    #endregion

    #region Variables
    public List<Pool> itemsToPool;
    private List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private Transform bulletParent;
    [SerializeField] private Transform bulletSpawnPosition;
    #endregion

    void Start() 
    { 
        foreach(Pool item in itemsToPool) 
        {
            for(int i = 0; i < item.amountToPool; i++) 
            {
                GameObject obj = Instantiate(item.objectToPool, bulletParent);
                
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
	
    public GameObject GetPooledObject(string tag) 
    {
        for(int i = 0 ; i < pooledObjects.Count; i++) 
        {
            if(!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(tag)) 
            {
                return pooledObjects[i];
            }
        }
        
        foreach(Pool item in itemsToPool) 
        {
            if (item.canExpand) 
            {
                GameObject obj = Instantiate(item.objectToPool, bulletParent);

                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }

    public Transform GetBulletSpawnPosition()
    {
        if(bulletSpawnPosition == null) bulletSpawnPosition = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CapsuleCollider>().transform;
        return bulletSpawnPosition;
    }
}
