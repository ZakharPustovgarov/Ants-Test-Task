using UnityEngine;

public class SpawnResourceVein : MonoBehaviour
{
    [SerializeField] private GameObject resourceVeinPrefab;
    [SerializeField] private Vector3 mapCenter = Vector3.zero;
    [SerializeField] private float minDIstanceFromCenter = 2f;
    [SerializeField] private float maxDIstanceFromCenter = 5f;

    public void SpawnNewVein()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float spawnDistance = Random.Range(minDIstanceFromCenter, maxDIstanceFromCenter);

        float offsetX = Mathf.Cos(randomAngle) * spawnDistance;
        float offsetY = Mathf.Sin(randomAngle) * spawnDistance;

        Vector3 spawnPosition = new Vector3(
            mapCenter.x + offsetX,
            mapCenter.y + offsetY,
            mapCenter.z 
        );

        GameObject spawnedObject = Instantiate(resourceVeinPrefab, spawnPosition, Quaternion.identity);
    }

}
