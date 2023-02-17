using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private Transform counterTopPoint;
        [SerializeField] private Transform plateVisualPrefab;
        [SerializeField] PlatesCounter platesCounter;
        private Stack<Transform> plates;
        private void Awake()
        {
            plates = new Stack<Transform>();
        }
        private void Start()
        {
            platesCounter.OnPlateSpawn += PlatesCounter_OnPlateSpawn;
            platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
        }

        private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
        {
            Transform plateRemoved = plates.Pop();
            Destroy(plateRemoved.gameObject);
        }

        private void PlatesCounter_OnPlateSpawn(object sender, System.EventArgs e)
        {
            Transform plate = Instantiate(plateVisualPrefab, counterTopPoint);
            float height = .1f;
            plate.localPosition = new Vector3(0, height * plates.Count, 0);
            plates.Push(plate);
        }
    }
}
