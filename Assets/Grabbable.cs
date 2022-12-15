using System;
using Grabbing;
using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
{
    private Transform _oldParent;
    private Rigidbody _rigidbody;
    private bool _kinematic;
    private GameObject _currentGrabber;
    
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void GrabberEnter(GameObject grabber)
    {
    }

    public void GrabberExit(GameObject grabber)
    {
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
        transform.SetParent(_oldParent);
        _oldParent = null;

        _rigidbody.isKinematic = _kinematic;
    }
}
