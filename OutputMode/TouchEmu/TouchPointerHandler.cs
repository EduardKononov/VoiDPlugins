using System.Numerics;
using OpenTabletDriver.Plugin.Platform.Pointer;

namespace VoiDPlugins.OutputMode
{
    public class TouchPointerHandler : IAbsolutePointer, IVirtualTablet
    {
        private bool _inContact;
        private bool _lastContact;

        public TouchPointerHandler()
        {
            Touch.Init();
            _inContact = false;
            _lastContact = false;
        }

        public void SetButtonState(uint button, bool active)
        {
            return;
        }

        public void SetEraser(bool isEraser)
        {
            return;
        }

        public void SetPosition(Vector2 pos)
        {
            Touch.SetPosition(new POINT((int)pos.X, (int)pos.Y));
            if (_inContact != _lastContact)
            {
                if (_inContact)
                {
                    Touch.UnsetPointerFlags(POINTER_FLAGS.UP | POINTER_FLAGS.UPDATE);
                    Touch.SetPointerFlags(POINTER_FLAGS.DOWN);
                    _lastContact = _inContact;
                }
                else
                {
                    Touch.UnsetPointerFlags(POINTER_FLAGS.DOWN | POINTER_FLAGS.UPDATE);
                    Touch.SetPointerFlags(POINTER_FLAGS.UP);
                    _lastContact = _inContact;
                }
            }
            else
            {
                Touch.SetPointerFlags(POINTER_FLAGS.UPDATE);
            }
            Touch.SetTarget();
            Touch.Inject();
        }

        public void SetPressure(float percentage)
        {
            var pressure = (uint)(percentage * 1024);
            if (pressure > 0)
            {
                Touch.SetPressure(pressure);
                Touch.SetPointerFlags(POINTER_FLAGS.INCONTACT | POINTER_FLAGS.FIRSTBUTTON);
                _inContact = true;
            }
            else
            {
                Touch.SetPressure(1);
                Touch.UnsetPointerFlags(POINTER_FLAGS.INCONTACT | POINTER_FLAGS.FIRSTBUTTON);
                _inContact = false;
            }
        }

        public void SetProximity(bool proximity, uint distance)
        {
            return;
        }

        public void SetTilt(Vector2 tilt)
        {
            return;
        }
    }
}