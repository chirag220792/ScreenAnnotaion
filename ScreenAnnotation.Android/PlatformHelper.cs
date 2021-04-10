using ScreenAnnotation.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformHelper))]

namespace ScreenAnnotation.Droid
{
    internal class PlatformHelper : IPlatformHelper
    {
        public bool IsInitialized => Platform.IsInitialized;
    }
}