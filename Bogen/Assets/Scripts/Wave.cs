using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave {
    [Range(1,50)]
    public int enemyCount;
    [Range(0, 30)]
    public float spawnInterval;

    public Wave(int enemyCount, float spawnInterval) {
        this.enemyCount = enemyCount;
        this.spawnInterval = spawnInterval;
    }
}
