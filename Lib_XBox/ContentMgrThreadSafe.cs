using System;
using Microsoft.Xna.Framework.Content;

/*
        /// <summary>
        /// Creates a Texture2D from the given stream, synchronized with the content loading to avoid thread conflicts.
        /// </summary>
        /// <param name="stream">Stream containing the .png or .jpg</param>
        /// <returns>Texture created from the stream</returns>
        public Texture2D FromStream(Stream stream)
        {
            lock (loadLock)
            {
                IGraphicsDeviceService graphicsDeviceService = (IGraphicsDeviceService)ServiceProvider.GetService(typeof(IGraphicsDeviceService));
                return Texture2D.FromStream(graphicsDeviceService.GraphicsDevice, stream);
            }
        }
*/

namespace XNALib
{
    /// <summary>
    /// Provides a content manager that can be used in multiple threads,
    /// but only tries to load one asset at a time. It will block the
    /// other threads trying to load assets until the current asset
    /// load is completed.
    /// </summary>
    public class ContentMgrThreadSafe : ContentManager
    {
        static object loadLock = new object();

        public ContentMgrThreadSafe(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public ContentMgrThreadSafe(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }

        public override T Load<T>(string assetName)
        {
            lock (loadLock)
            {
                return base.Load<T>(assetName);
            }
        }
    }
}
