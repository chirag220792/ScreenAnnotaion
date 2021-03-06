using ScreenAnnotation.EditorPage;
using ScreenAnnotation.Helper;
using SkiaSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScreenAnnotation
{
    
    public class ImageEditor
    {
        private static readonly Lazy<ImageEditor> lazy = new Lazy<ImageEditor>(() => new ImageEditor());
        private ImageEditor()
        {
            IPlatformHelper platform = DependencyService.Get<IPlatformHelper>();
            if (!(platform?.IsInitialized ?? false))
                throw new Exception("ScreenAnnotation must be initialized on the platform");
        }

    
        public static ImageEditor Instance { get => lazy.Value; }
        internal IImageHelper ImageHelper => DependencyService.Get<IImageHelper>();
    
        public static bool IsOpened { get; private set; } = false;

        private const string defaultFolderName = "ScreenAnnotation";
        private string folderName;
        private bool mainPageIsChanged = false;
        private Page mainPage;
        private TaskCompletionSource<byte[]> taskCompletionEditImage;
        private bool imageEditLock;
        private bool imageSetLock;
        private ImageEditorPage page;

        /// <summary>name of the folder for saving images </summary>
        public string FolderName
        {
            get => string.IsNullOrWhiteSpace(folderName) ? defaultFolderName : defaultFolderName;
            set => folderName = value;
        }
        private bool ImageEditLock
        {
            get => imageEditLock;
            set => imageEditLock = IsOpened = value;
        }



        /// <summary>method for saving images</summary>
        /// <param name="data">image</param>
        /// <param name="imageName">file name of the image</param>
        /// <returns>returns "true" if the image was saved</returns>
        public async Task<bool> SaveImage(byte[] data, string imageName) => await ImageHelper.SaveImageAsync(data, imageName);

        /// <summary>Returns the edited image
        /// <para>
        /// if <paramref name="bitmap"/> is null, the user can select an image from the gallery
        /// </para></summary>
        /// <param name="bitmap">original image</param>
        /// <param name="config">сonfigurator image editor</param>
        /// <returns>edited image</returns>
        public async Task<byte[]> GetEditedImage(SKBitmap bitmap = null, ImageEditorConfig config = null)
        {
            if (!ImageEditLock)
            {
                ImageEditLock = true;
                if (bitmap == null)
                {
                    using (Stream stream = await ImageHelper.GetImageAsync())
                    {
                        bitmap = stream != null ? SKBitmap.Decode(stream) : null;
                    }
                }
                if (config == null)
                    config = new ImageEditorConfig();

                //await Task.Delay(100);
                var data = bitmap != null ? await PushImageEditorPage(bitmap, config) : null;
                ImageEditLock = false;
                return data;
            }
            else
                return null;
        }

        internal void SetImage(SKBitmap bitmap = null)
        {
            if (!imageSetLock)
            {
                imageSetLock = true;
                if (bitmap != null)
                {
                    taskCompletionEditImage.SetResult(SkiaHelper.SKBitmapToBytes(bitmap));
                }
                else
                    taskCompletionEditImage.SetResult(null);

                if (page != null)
                {
                    page.Dispose();
                    page = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }
      
        private async Task<byte[]> PushImageEditorPage(SKBitmap bitmap, ImageEditorConfig config)
        {
            try
            {
                taskCompletionEditImage = new TaskCompletionSource<byte[]>();

                if (bitmap != null)
                {
                    page = new ImageEditorPage(bitmap, config);

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Application.Current.MainPage.Navigation.PushModalAsync(page);
                    }
                    else
                    {
                        mainPage = Application.Current.MainPage;
                        Application.Current.MainPage = page;
                        mainPageIsChanged = true;
                    }
                }
                else
                    taskCompletionEditImage.SetResult(null);

                byte[] data = await taskCompletionEditImage.Task;

                if (mainPageIsChanged)
                    Application.Current.MainPage = mainPage;
                else
                    await Application.Current.MainPage.Navigation.PopModalAsync();

                mainPage = null;
                ImageEditLock = false;
                imageSetLock = false;
                return data;
            }
            catch (Exception ex)
            {
                SetImage(null);
                return null;
            }
        }
    }
}
