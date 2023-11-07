using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private Dictionary Dictionary;
    [SerializeField] private Button Tree;
    [SerializeField] private Button Rock;
    [SerializeField] private Button Shrub;

    public Transform Spawn;

    private void Awake()
    {
        Tree.onClick.AddListener(() =>
        {
            Dictionary.CreateObject("Tree", Spawn);
        });
        Rock.onClick.AddListener(() =>
        {
            Dictionary.CreateObject("Rock4", Spawn);
        });
        Shrub.onClick.AddListener(() =>
        {
            Dictionary.CreateObject("Shrub", Spawn);
        });
    }
}
