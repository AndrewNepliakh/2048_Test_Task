using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Managers.CubesManager
{
    public class CubeData
    {
        public CubeState CubeState => _cubeState;
        
        private static readonly List<Vector3> AvailablePositions = new();

        private int _rate;
        private Vector3 _position;
        
        private CubeState _cubeState;

        public int Rate => _rate;
        public Vector3 Position => _position;

        static CubeData()
        {
            var y = 0.6f;
            int[] xValues = { -2, -1, 0, 1, 2 };
            float[] zValues = { 8.5f, 9.5f, 10.5f };

            foreach (var x in xValues)
            foreach (var z in zValues)
                AvailablePositions.Add(new Vector3(x, y, z));
        }

        public CubeData(CubeState cubeState)
        {
            _rate = Random.value < 0.75f ? 2 : 4;
            _cubeState = cubeState;
            _position = GetRandomPosition();
        }

        public CubeData(Vector3 position, CubeState cubeState)
        {
            _rate = Random.value < 0.75f ? 2 : 4;
            _position = position;
            _cubeState = cubeState;
            AvailablePositions.Remove(position); 
        }
        
        public CubeData(int rate, Vector3 position, CubeState cubeState)
        {
            _rate = rate;
            _cubeState = cubeState;
            _position = position;
        }

        private Vector3 GetRandomPosition()
        {
            if (AvailablePositions.Count == 0)
                throw new Exception("No positions available!");

            var index = Random.Range(0, AvailablePositions.Count);
            var position = AvailablePositions[index];
            AvailablePositions.RemoveAt(index);
            return position;
        }
    }
}