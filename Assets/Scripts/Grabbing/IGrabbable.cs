using UnityEngine;

namespace Grabbing
{
    public interface IGrabbable
    {
        void GrabberEnter(GameObject grabber);
        void GrabberExit(GameObject grabber);
        void GrabberGrab(GameObject grabber);
        void GrabberRelease(GameObject grabber);
    }
}