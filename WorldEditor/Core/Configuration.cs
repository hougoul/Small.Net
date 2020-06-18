namespace WorldEditor.Core
{
    /// <summary>
    /// Global configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Enables or disables object tracking. Default is disabled (false).
        /// </summary>
        /// <remarks>
        /// Object Tracking is used to track COM object lifecycle creation/dispose. When this option is enabled
        /// objects can be tracked using <see cref="ObjectTracker"/>. Using Object tracking has a significant
        /// impact on performance and should be used only while debugging.
        /// </remarks>
        public static bool EnableObjectTracking = false;

        /// <summary>
        /// Enables or disables release of <see cref="ComObject"/> on finalizer. Default is disabled (false).
        /// </summary>
        public static bool EnableReleaseOnFinalizer = false;

        /// <summary>
        /// Enables or disables writing a warning via <see cref="System.Diagnostics.Trace"/> if a <see cref="ComObject"/> was disposed in the finalizer. Default is enabled (true).
        /// </summary>
        public static bool EnableTrackingReleaseOnFinalizer = true;

        /// <summary>
        /// Throws a <see cref="CompilationException"/> when a shader or effect compilation error occurred. Default is enabled (true).
        /// </summary>
        public static bool ThrowOnShaderCompileError = true;

        /// <summary>
        /// By default all objects in the process are tracked.
        /// Use this property to track objects per thread.
        /// </summary>
        public static bool UseThreadStaticObjectTracking = false;
    }
}