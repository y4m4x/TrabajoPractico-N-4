using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private Factory[] Object;
    private Dictionary<string, Factory> ObjectDictionary;

    private void Awake()
    {
        ObjectDictionary = new Dictionary<string, Factory>();

        foreach (var obj in Object)
        {
            ObjectDictionary.Add(obj.ObjectToSpawn, obj);
        }
    }

    public Factory CreateObject(string name, Transform ObjectToSpawnTransform)
    {
        if (ObjectDictionary.TryGetValue(name, out Factory objectPrefab))
        {
            Factory objectInstance = Instantiate(objectPrefab, ObjectToSpawnTransform.position, Quaternion.identity);
            return objectInstance;
        }
        else
        {
            Debug.LogWarning($"El objeto '{ObjectDictionary}' no existe en la base de datos de objetos.");
            return null;
        }
    }
}

