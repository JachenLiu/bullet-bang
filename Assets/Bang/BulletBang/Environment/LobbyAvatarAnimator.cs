using UnityEngine;

namespace BulletBang
{
    /// <summary>
    /// Presents replicated lobby movement using the imported humanoid rig.
    /// The current asset contains no clips, so this procedural adapter is an
    /// intentionally replaceable presentation component.
    /// </summary>
    public sealed class LobbyAvatarAnimator : MonoBehaviour
    {
        /// <summary>The replicated player whose public movement state is rendered.</summary>
        public NetworkPlayer Player { get; set; }

        private Animator _animator;
        private Transform _hips;
        private Transform _leftArm;
        private Transform _rightArm;
        private Transform _leftLeg;
        private Transform _rightLeg;
        private Vector3 _hipsPosition;
        private Quaternion _leftArmRotation;
        private Quaternion _rightArmRotation;
        private Quaternion _leftLegRotation;
        private Quaternion _rightLegRotation;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator == null || !_animator.isHuman) return;

            _hips = _animator.GetBoneTransform(HumanBodyBones.Hips);
            _leftArm = _animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            _rightArm = _animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            _leftLeg = _animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            _rightLeg = _animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            CacheRestPose();

            // Disabling the clip evaluator lets this adapter own bone transforms.
            _animator.enabled = false;
        }

        private void LateUpdate()
        {
            if (Player == null || _hips == null) return;

            var speed = Mathf.Clamp01(Player.VisualSpeed);
            var swing = Mathf.Sin(Time.time * Mathf.Lerp(2f, 9f, speed)) * 28f * speed;
            var airborne = !Player.IsGrounded;
            var crouch = Player.IsCrouching ? 1f : 0f;
            _hips.localPosition = Vector3.Lerp(
                _hips.localPosition,
                _hipsPosition + Vector3.down * (0.38f * crouch) +
                Vector3.up * (Mathf.Sin(Time.time * 1.8f) * 0.012f),
                Time.deltaTime * 12f);

            Pose(_leftArm, _leftArmRotation, airborne ? -25f : swing);
            Pose(_rightArm, _rightArmRotation, airborne ? -25f : -swing);
            Pose(_leftLeg, _leftLegRotation, airborne ? -22f : -swing);
            Pose(_rightLeg, _rightLegRotation, airborne ? -22f : swing);
        }

        private void CacheRestPose()
        {
            if (_hips != null) _hipsPosition = _hips.localPosition;
            if (_leftArm != null) _leftArmRotation = _leftArm.localRotation;
            if (_rightArm != null) _rightArmRotation = _rightArm.localRotation;
            if (_leftLeg != null) _leftLegRotation = _leftLeg.localRotation;
            if (_rightLeg != null) _rightLegRotation = _rightLeg.localRotation;
        }

        private static void Pose(Transform bone, Quaternion rest, float angle)
        {
            if (bone == null) return;
            bone.localRotation = Quaternion.Slerp(
                bone.localRotation,
                rest * Quaternion.Euler(angle, 0f, 0f),
                Time.deltaTime * 14f);
        }
    }
}
