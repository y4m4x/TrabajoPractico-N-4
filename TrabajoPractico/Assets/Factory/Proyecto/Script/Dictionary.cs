using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private Factory[] Object;
    private Dictionary<string, Factory> ObjectToSpawn;

    private void Awake()
    {
        ObjectToSpawn = new Dictionary<string, Factory>();

        foreach (var obj in Object)
        {
            ObjectToSpawn.Add(obj.ObjectToSpawn, obj);
        }
    }

    public Object CreateObject(string ObjectToSpawn, Transform ObjectToSpawnTransform)
    {
        if (ObjectToSpawn.TryGetValue(ObjectToSpawn, out Object objectPrefab))
        {
            Object objectInstance = Instantiate(objectPrefab, ObjectToSpawnTransform.position, Quaternion.identity);
        }
    }
}
