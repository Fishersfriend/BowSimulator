using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGenerator : MonoBehaviour {

    public Transform plantPrefab;
    public int plantCount = 30;
    public float maxRadius = 12;
    public float minRadius = 2.5f;
    public float minScale = 1f;
    public float maxScale = 1.8f;

    private void Start() {
        for(int i = 0; i < plantCount; i++) {
            Transform grass = Instantiate<Transform>(plantPrefab, transform);
            float angle = Random.value * 2 * Mathf.PI;
            float radius = (maxRadius - minRadius) * Mathf.Pow(Random.value, 2f) + minRadius;
            grass.localPosition = (new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle))) * radius;
            grass.localEulerAngles = Vector3.up * Random.Range(0, 360f);
            grass.localScale = Vector3.one * Random.Range(minScale, maxScale);
        }
    }
}
