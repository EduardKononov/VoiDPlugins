using System.Numerics;
using OpenTabletDriver.Plugin.Platform.Pointer;
using OpenTabletDriver.Plugin.Tablet;
using VoiDPlugins.Library.VMulti;
using VoiDPlugins.Library.VMulti.Device;
using VoiDPlugins.Library.VoiD;

namespace VoiDPlugins.OutputMode
{
    public unsafe class VMultiRelativePointer : IRelativePointer, ISynchronousPointer
    {
        private readonly RelativeInputReport* _rawPointer;
        private readonly VMultiInstance<RelativeInputReport>? _instance;
        private Vector2 _error;

        public VMultiRelativePointer(TabletReference tabletReference)
        {
            _instance = GlobalStore<VMultiInstance>.Set(tabletReference, () => new VMultiInstance<RelativeInputReport>("VMultiAbs", new RelativeInputReport()));
            _rawPointer = _instance.Pointer;
        }

        public void SetPosition(Vector2 delta)
        {
            delta += _error;
            _error = new Vector2(delta.X % 1, delta.Y % 1);
            _rawPointer->X = (byte)delta.X;
            _rawPointer->Y = (byte)delta.Y;
        }

        public void Reset()
        {
        }

        public void Flush()
        {
            _instance!.Write();
        }
    }
}