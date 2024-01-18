using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReader;

	private void Awake()
    {
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();

        inputReader.Reset();
    }
}
