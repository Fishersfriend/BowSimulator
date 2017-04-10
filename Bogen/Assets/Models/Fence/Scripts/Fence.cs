using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour {

    [Range(0, 15)]
    public float radius = 2.5f;
    [Range(3, 24)]
    public int wallCount = 8;
    [Range(0, 80)]
    public int plankCount = 12;
    [Range(-.3f, 0f)]
    public float postOffset = .06f;
    [Range(0, 1.2f)]
    public float barHeight = .8f;

    [SerializeField]
    private Transform postPrefab;
    [SerializeField]
    private Transform barPrefab;
    [SerializeField]
    private Transform[] plankPrefabs;

    private Transform[] posts;
    private Transform[] bars;
    private Transform[] plankGrps;
    private Transform[] walls;
    public Transform[] Walls { get { return walls; } }

    private int oldSegments;
    private int oldPlankCount;

    private void Awake() {
        Create();
        Generate();
    }

    private void Create() {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        posts = new Transform[wallCount];
        bars = new Transform[wallCount];
        walls = new Transform[wallCount];
        plankGrps = new Transform[wallCount];

        for (int i = 0; i < wallCount; i++) {
            posts[i] = Instantiate<Transform>(postPrefab, transform);
            RotateRandom(posts[i].GetChild(0));
            posts[i].GetChild(0).localPosition += Vector3.up * Random.Range(-.07f, .07f);

            walls[i] = (new GameObject()).transform;
            walls[i].name = "Wall_" + i;
            walls[i].parent = transform;
            walls[i].localPosition = Vector3.zero;
            walls[i].gameObject.AddComponent<FenceWall>();
            
            for (int j = 0; j < plankCount; j++) {
                Transform newPlank = Instantiate<Transform>(plankPrefabs[Random.Range(0, plankPrefabs.Length)], walls[i]);
                RotateRandom(newPlank.GetChild(0));
                newPlank.localRotation = Quaternion.Euler(Random.Range(-3f, 3f), 0, 0);
            }
            
            bars[i] = Instantiate<Transform>(barPrefab, walls[i]);
            RotateRandom(bars[i].GetChild(0));
        }

        oldSegments = wallCount;
        oldPlankCount = plankCount;

    }

    public void Generate() {
        if (wallCount != oldSegments || plankCount != oldPlankCount) Create();

        float step = (Mathf.PI * 2) / wallCount;
        float wallLength = ((new Vector3(Mathf.Sin(step), 0, Mathf.Cos(step))) * radius - (new Vector3(0, 0, radius))).magnitude;
        for (int i = 0; i < wallCount; i++) {
            float angle = i * step;
            Vector3 pos = (new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle))) * radius;

            Transform post = posts[i];
            post.localPosition = pos + Vector3.up * postOffset;
            post.localRotation = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0);

            

            Transform wall = walls[i];
            float plankStep = wallLength / plankCount;
            for(int j = 0; j < plankCount; j++) {
                Transform plank = wall.GetChild(j);
                plank.localPosition = Vector3.right * (plankStep * j - wallLength/2 + plankStep/2);
                plank.localScale = new Vector3(1, barHeight / .8f, 1);
            }
            wall.localPosition = pos + Vector3.up * barHeight;
            wall.localRotation = Quaternion.Euler(0, (angle + step / 2) * Mathf.Rad2Deg, 0);
            wall.position += wall.right * wallLength / 2;

            float plankOffset = .116f;
            wall.localPosition *= 1 + plankOffset / wall.localPosition.magnitude;

            
            Transform bar = bars[i];
            bar.localRotation = Quaternion.Euler(0, 0, -90);
            bar.localPosition = Vector3.back * .044f;
            bar.localScale = new Vector3(1, wallLength / 1.913417f, 1);

        }
    }

    private void RotateRandom(Transform tm) {
        tm.localRotation = Quaternion.Euler(0, Random.Range(0, 2) * 180, Random.Range(0, 2) * 180);
    }
}
