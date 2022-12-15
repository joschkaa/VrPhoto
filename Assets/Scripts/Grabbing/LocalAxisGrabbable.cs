using System;
using UnityEngine;
using UnityEngine.Events;

namespace Grabbing
{
    public class LocalAxisGrabbable: MonoBehaviour, IGrabbable
    {
        [SerializeField] private bool lockXAxis;
        [SerializeField] private bool lockYAxis;
        [SerializeField] private bool lockZAxis;

        [SerializeField] private Vector3 maxAllowedDistance;

        [SerializeField] private Vector3 values;

        [SerializeField] private UnityEvent<Vector3> valuesChanged;

        [SerializeField] private Material selected;
        [SerializeField] private Material unselected;
        
        private Renderer _material;
        
        private bool _active;
        private GameObject _grabber;
        private Vector3 _relation;

        private float _lastDispatch;
        private bool _hasChanged;

        private void Start()
        {
            _material = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (!_hasChanged) return;
            if (Time.fixedTime < _lastDispatch + 0.01f) return;
            _lastDispatch = Time.fixedTime;
            _hasChanged = false;
            valuesChanged.Invoke(values);
        }

        private void FixedUpdate()
        {
            
            if (!_active) return;
            var t = transform;
            
            var relativePositionLocalRotation = Quaternion.Inverse(t.rotation) *(_grabber.transform.position - _relation - t.position);
            var newPos = t.localPosition + new Vector3(
                lockXAxis ? relativePositionLocalRotation.x : 0f, 
                lockYAxis ? relativePositionLocalRotation.y : 0f, 
                lockZAxis ? relativePositionLocalRotation.z : 0f
            );
            var newMaxPos = new Vector3(
                Math.Max(0, Math.Min(newPos.x, maxAllowedDistance.x)),
                Math.Max(0, Math.Min(newPos.y, maxAllowedDistance.y)),
                Math.Max(0, Math.Min(newPos.z, maxAllowedDistance.z))
            );

            values = new Vector3(newMaxPos.x / maxAllowedDistance.x, newMaxPos.y / maxAllowedDistance.y, newMaxPos.z / maxAllowedDistance.z);
            _hasChanged = true;
            
            t.localPosition = newMaxPos;
        }

        public void SetValue(Vector3 value)
        {
            values = value;
            transform.localPosition = new Vector3(
                Math.Max(0, Math.Min(value.x * maxAllowedDistance.x, maxAllowedDistance.x)),
                Math.Max(0, Math.Min(value.y * maxAllowedDistance.y, maxAllowedDistance.y)),
                Math.Max(0, Math.Min(value.z * maxAllowedDistance.z, maxAllowedDistance.z))
            );
            valuesChanged.Invoke(values);
        }

        public void GrabberEnter(GameObject grabber)
        {
            _material.material = selected;
        }

        public void GrabberExit(GameObject grabber)
        {
            _material.material = unselected;
        }

        public void GrabberGrab(GameObject grabber)
        {
            if(_active) GrabberRelease(null);
            _active = true;
            _grabber = grabber;
            _relation = grabber.transform.position- transform.position;
        }

        public void GrabberRelease(GameObject grabber)
        {
            _active = false;
            _grabber = null;
        }
    }
}