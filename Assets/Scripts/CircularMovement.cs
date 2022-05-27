using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class CircularMovement : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private bool _rotate = false;
        [SerializeField] private float _rotationCorrection = 0;
        [SerializeField] private bool _enableStartAnimation = false;
        [SerializeField] private float _waitingTime = 5;
        [SerializeField] private float _animationSpeed = 1f;

        private Transform[] bodies;
        private float time;

        private void Awake()
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
            bodies = GetTransformsInFirstDepthChildren();
        }

        private Transform[] GetTransformsInFirstDepthChildren()
        {
            var gos = GetComponentsInChildren<Transform>();
            var result = new List<Transform>();
            foreach (var go in gos)
                if (go.parent.gameObject == gameObject)
                    result.Add(go.transform);
            return result.ToArray();
        }

        private void Update()
        {
            if (_enableStartAnimation && _waitingTime > 0)
                _waitingTime -= Time.deltaTime;
            CalculatePositions();
            if(_rotate)
                CalculateRotations();
        }
        
        private void CalculatePositions()
        {
            var step = 2 * Mathf.PI / bodies.Length;

            var containerPosition = transform.position;
            for (var i = 0; i < bodies.Length; i++)
            {
                var angle = step * i;
                var pos = new Vector3(
                    Mathf.Cos(angle) * _radius,
                    Mathf.Sin(angle) * _radius);
                bodies[i].position = containerPosition + pos;
            }
        }
        
        private void CalculateRotations()
        {
            var step = 360 / bodies.Length;
            
            var containerRotation = transform.eulerAngles;
            for (var i = 0; i < bodies.Length; i++)
            {
                var angle = step * i;

                var rot = new Vector3(0, 0, angle + _rotationCorrection);
                bodies[i].eulerAngles = containerRotation + rot;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateContent();
            CalculatePositions();
            if(_rotate)
                CalculateRotations();
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }
#endif
    }
}