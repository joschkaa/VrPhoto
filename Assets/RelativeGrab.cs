using System;
using System.Collections.Generic;
using Grabbing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class RelativeGrab : MonoBehaviour
{
    private ActionBasedController _controller;

    private readonly List<IGrabbable> _colliders = new();

    private IGrabbable _activeGrabbed;
    
    
    private void OnTriggerEnter (Collider other)
    {
        var grabbable = other.gameObject.GetComponent<IGrabbable>();
        if (grabbable == null || _colliders.Contains(grabbable)) return;
        _colliders.Add(grabbable);
        grabbable.GrabberEnter(gameObject);
    }
 
    private void OnTriggerExit (Collider other) {
        var grabbable = other.gameObject.GetComponent<IGrabbable>();
        if (grabbable == null) return;
        _colliders.Remove(grabbable);
        grabbable.GrabberExit(gameObject);
    }
    
    private void Start()
    {
        _controller = GetComponent<ActionBasedController>();
        _controller.activateAction.action.started += TriggerStarted;
        _controller.activateAction.action.canceled += TriggerCanceled;
    }

    private void TriggerStarted(InputAction.CallbackContext ctx)
    {
        if (_colliders.Count == 0) return;
        _activeGrabbed = _colliders[0];
        _activeGrabbed.GrabberGrab(gameObject);
    }

    private void TriggerCanceled(InputAction.CallbackContext ctx)
    {
        if (_activeGrabbed == null) return;
        _activeGrabbed.GrabberRelease(gameObject);
        _activeGrabbed = null;
    }
}