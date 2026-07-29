using Fusion;
using UnityEngine;

namespace BulletBang
{
    /// <summary>
    /// Serializable set of player intentions submitted to a Fusion simulation tick.
    /// This type contains input only; authoritative gameplay state belongs to
    /// network behaviours.
    /// </summary>
    public struct NetworkInputData : INetworkInput
    {
        /// <summary>Requested planar movement in local X/Z space.</summary>
        public Vector2 MovementInput;

        /// <summary>Requested horizontal body rotation.</summary>
        public float RotationInput;

        /// <summary>Whether the jump control is currently held.</summary>
        public NetworkBool JumpHeld;

        /// <summary>Whether the crouch control is currently held.</summary>
        public NetworkBool CrouchHeld;
    }
}
