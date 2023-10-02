using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Levels", order = 1)]
public class Level : ScriptableObject
{
    [Serializable]
    public class FieldObjectPosition
    {
        public Vector2Int Position;
        public GameObject Object;
    }

    [Serializable]
    public class SpawnWave
    {
        public List<Enemy> _enemies;
    }

    [Header("Field Settings")]
    public int FieldWidth;
    public int FieldHeight;
    public int CellSize;

    public List<FieldObjectPosition> FieldObjects;
    public List<Vector2Int> PositionOfInvicibleWalls;
    public Sprite CellSprite;

    public Vector3 OriginPosition;

    public Vector2Int SpawnerPosition;
    public Vector2Int MainBuildingPosition;

    public int WallsCount;

    [Header("References")]
    public Spavner Spavner;

    public GameObject MainBuilding;
    public Sprite WallSelf;
    public GameObject InvincibleWall;

    [Header("References")]

    public List<SpawnWave> _spavnQueue;

}
