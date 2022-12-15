using System;
using System.Collections.Generic;
using Grabbing;
using UnityEngine;

public class GrabbableAndSock : MonoBehaviour, IGrabbable
{
    private Transform _oldParent;
    private Rigidbody _rigidbody;
    private bool _kinematic;
    private GameObject _currentGrabber;
    private List<Selectable> _colliders = new();

    [SerializeField] private LightController lightController;

    [SerializeField] private Transform resetPoint;
    
    private Renderer _renderer;
    
    [SerializeField] private Material selected;
    [SerializeField] private Material unselected;
    
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    public void GrabberEnter(GameObject grabber)
    {
    }

    public void GrabberExit(GameObject grabber)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.gameObject.GetComponent<Selectable>();
        if (grabbable == null || _colliders.Contains(grabbable)) return;
        _colliders.Add(grabbable);
        _renderer.material = selected;
    }

    private void OnTriggerExit(Collider other)
    {
        var grabbable = other.gameObject.GetComponent<Selectable>();
        if (grabbable == null) return;
        _colliders.Remove(grabbable);
        if (_colliders.Count == 0)
        {
            _renderer.material = unselected;
        }
    }


    public void GrabberGrab(GameObject grabber)
    {
        if (_currentGrabber != null) GrabberRelease(_currentGrabber);
        _currentGrabber = grabber;
        _oldParent = transform.parent;
        transform.SetParent(grabber.transform);
        

        _kinematic = _rigidbody.isKinematic;
        _rigidbody.isKinematic = true;
    }

    public void GrabberRelease(GameObject grabber)
    {
        if (_currentGrabber != grabber) return;
        lightController.Light = _colliders.Count != 0 ? _colliders[0].lalalight : null;
        transform.SetParent(_colliders.Count != 0 ? _colliders[0].anchor : resetPoint);
        transform.localPosition = Vector3.zero;
        
        
        _rigidbody.isKinematic = _kinematic;
    }
}
