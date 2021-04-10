using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace ScreenAnnotation.Droid
{
    
    public static class Platform
    {
        internal static bool IsInitialized { get; set; }

        internal static FormsAppCompatActivity CurrentActivity { get; private set; }
        internal static Bundle CurrentBundle { get; private set; }
        internal const int PickImageId = 1000;

        public static void Init(FormsAppCompatActivity activity, Bundle bundle)
        {
            CurrentActivity = activity;
            CurrentBundle = bundle;
            IsInitialized = true;
            LinkAssemblies();
        }

        /// <summary>required to get an image from the gallery</summary>
        public static void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            if (requestCode == PickImageId)
                ImageHelper.OnActivityResult(resultCode, intent);
        }

        public static void OnBackPressed()
        {
            if(ImageEditor.IsOpened)
                MessagingCenter.Send(Xamarin.Forms.Application.Current, "SADroidBackButton");
        }

        private static void LinkAssemblies()
        {

        }
    }
}
