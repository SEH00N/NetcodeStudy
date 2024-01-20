using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReader;

	private void Awake()
    {
        Player.ResetStaticData();

        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();

        inputReader.Reset();
    }
}
