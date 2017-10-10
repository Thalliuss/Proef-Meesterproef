using System;
using System.Collections.Generic;
using UnityEngine;

public class MassPointHandler : MonoBehaviour
{
    [Serializable]
    public class PointMassInfo
    {
        public MassPoint massPoint;
        public float gravitationalForce;
        public float gravitationalField;
    }
    public List<PointMassInfo> massPoints = new List<PointMassInfo>();
}