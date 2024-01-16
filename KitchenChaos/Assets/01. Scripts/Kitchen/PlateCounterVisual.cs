using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
	[SerializeField] Transform topPoint;
    [SerializeField] Transform platePrefab;

    private PlateCounter counter;

    private Stack<GameObject> plates;

    private void Awake()
    {
        counter = transform.parent.GetComponent<PlateCounter>();
        plates = new Stack<GameObject>();
    }

    private void Start()
    {
        counter.OnPlateSpawnedEvent += HandleSpawn;
        counter.OnPlateRemovedEvent += HandleRemove;
    }

    private void HandleSpawn()
    {
        Transform instance = Instantiate(platePrefab, topPoint);

        float offset = 0.1f;
        instance.localPosition = new Vector3(0, offset * plates.Count, 0);

        plates.Push(instance.gameObject);
    }

    private void HandleRemove()
    {
        GameObject plate = plates.Pop();
        Destroy(plate);
    }
}
