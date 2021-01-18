using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningRow : MonoBehaviour
{
    public bool isRoad;
    public bool slowed = false;
    public float objectSpeed;
    public ScrollingTexture scrollingTexture = null;
    [SerializeField]
    private Vector2 spawnTimeBounds;
    [SerializeField]
    private Vector2 objectSpeedBounds;
    [SerializeField]
    private Transform spawnPoint; 
    [SerializeField]
    private GameObject[] spawnableObjects;
    private int spawnableObjectsLength;
    private bool flipped = false;
    private ObjectPooler objectPooler;
    private List<int> objectPoolerIndexes = new List<int>();
    

    private void Start()
    {
        if (isRoad)
        {
            GameManager.Instance.roadsSpawn.Add(this);
        }
        objectPooler = ObjectPooler.SharedInstance;
        foreach(GameObject o in spawnableObjects)
        {
            objectPoolerIndexes.Add(objectPooler.AddObject(o, 5));
        }
        spawnableObjectsLength = spawnableObjects.Length;
        StartCoroutine(SpawnObject(Random.Range(0f, 1f)));
        objectSpeed = Random.Range(objectSpeedBounds.x, objectSpeedBounds.y);
        if (GameManager.Instance.playTimeMan)
        {
            objectSpeed *= 1.3f;
        }
        foreach (GameObject o in spawnableObjects)
        {
            o.GetComponent<LinearMovementObject>().speed = objectSpeed;
        }
        if(scrollingTexture != null)
        {
            scrollingTexture.speedX = objectSpeed / 12;
        }
    }

    private IEnumerator SpawnObject(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject newObject = objectPooler.GetPooledObject(objectPoolerIndexes[Random.Range(0, spawnableObjectsLength)], spawnPoint);//Instantiate(spawnableObjects[Random.Range(0, spawnableObjectsLength)], spawnPoint.position, spawnPoint.rotation);
        LinearMovementObject linearMovementObject = newObject.GetComponent<LinearMovementObject>();
        linearMovementObject.speed = objectSpeed;
        linearMovementObject.slowed = slowed;
        if (flipped)
        {
            linearMovementObject.speed *= -1;
        }
        StartCoroutine(SpawnObject(Random.Range(spawnTimeBounds.x, spawnTimeBounds.y)));
    }

    public void Flip()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        flipped = true;
    }
}
