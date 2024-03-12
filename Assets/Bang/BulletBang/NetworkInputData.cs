using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 direction;
        public Quaternion rotation;
    }
}
