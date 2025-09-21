using System.Collections.Generic;
using UnityEngine;

namespace Managers.CubesManager
{
    [CreateAssetMenu(fileName = "CubesConfig", menuName = "Configs/CubesConfig")]
    public class CubesConfig : ScriptableObject
    {
        [SerializeField] private List<CubeData> _cubeDatas = new();

        public List<CubeData> CubeDatas => _cubeDatas;
    }
}