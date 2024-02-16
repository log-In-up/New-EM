using System;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [Serializable]
    public struct EnemySpawnData
    {
        public string Id;
        public EnemyTypeId Type;
        public Vector3 Position;
        public Quaternion Rotation;

        public EnemySpawnData(string id, EnemyTypeId type, Vector3 position, Quaternion rotation) : this()
        {
            Id = id;
            Type = type;
            Position = position;
            Rotation = rotation;
        }
    }
}