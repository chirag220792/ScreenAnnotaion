﻿namespace ScreenAnnotation.IOS
{
    public static class Platform
    {
        internal static bool IsInitialized { get; set; }

        public static void Init()
        {
            IsInitialized = true;
            LinkAssemblies();
        }

        private static void LinkAssemblies()
        {

        }
    }
}
