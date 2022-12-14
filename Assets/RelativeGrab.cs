using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class RelativeGrab : MonoBehaviour
{
    private ActionBasedController _controller;

    private readonly List<GameObject> _colliders = new();

    private GameObject _activeGrabbed;
    private Transform _parentTransform;
    private bool _isKinematic;
    
    
    private void OnTriggerEnter (Collider other) {
        Debug.Log("Add");
        if (other.gameObject.GetComponent<Grabbable>() != null && !_colliders.Contains(other.gameObject))
        {
            _colliders.Add(other.gameObject);
            Debug.Log("Add to colliders");
        }
    }
 
    private void OnTriggerExit (Collider other) {
        _colliders.Remove(other.gameObject);
    }
    
    private void Start()
    {
        _controller = GetComponent<ActionBasedController>();
        _controller.activateAction.action.started += TriggerStarted;
        _controller.activateAction.action.canceled += TriggerCanceled;
    }

    private void TriggerStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("Trigger Started");
        if (_colliders.Count == 0) return;
        _activeGrabbed = _colliders[0];
        _parentTransform = _activeGrabbed.transform.parent;
        
        _activeGrabbed.transform.SetParent(transform);
        var rb = _activeGrabbed.GetComponent<Rigidbody>();
        if (rb != null)
        {
            _isKinematic = rb.isKinematic;
            rb.isKinematic = true;
        }
    }

    private void TriggerCanceled(InputAction.CallbackContext ctx)
    {
        if (_activeGrabbed == null) return;
        _activeGrabbed.transform.SetParent(_parentTransform);
        _parentTransform = null;
        
        var rb = _activeGrabbed.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = _isKinematic;
        }
    }
}