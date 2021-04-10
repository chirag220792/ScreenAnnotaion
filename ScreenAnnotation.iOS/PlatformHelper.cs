using ScreenAnnotation.IOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformHelper))]

namespace ScreenAnnotation.IOS
{
    internal class PlatformHelper : IPlatformHelper
    {
        public bool IsInitialized => Platform.IsInitialized;
    }
}