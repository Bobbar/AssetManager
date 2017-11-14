using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
namespace AssetManager
{
    /// <summary>
    ///  These methods allow me to add a single line after the
    ///  InitializeComponent call in a Forms constructor that will efficiently reassign
    ///  all control images set from the recklessly leaky 'spawn-a-new-instance-of-every-object' - ResourceManager.GetObject method.
    ///  By using a global HashTable to contain and pass out single instances of the images, I can
    ///  reduce memory usage by comical orders of magnitude.
    ///  ResourceManager.GetObject can "Die a prolonged and relentlessly agonizing death."
    /// </summary>
    static class ImageCaching
    {

        private static Hashtable ImageCacheHashTable = new Hashtable();
        /// <summary>
        /// Controls passed to this method will have all their child Control images replaced with a single centrally cached instance of that image.
        /// </summary>
        /// <param name="control">Parent container object containing the controls you wish to have cached images applied to. Typically a Form.</param>
        public static void CacheControlImages(object control)
        {
            //Loop through child controls.
            foreach (Control ctl in ((Control)control).Controls)
            {
                if (ctl.GetType() == typeof(OneClickToolStrip) | ctl.GetType() == typeof(ToolStrip))
                {
                    //If control is a toolstrip, pass it to another recursive method to loop through all toolstrip items.
                    SetToolStripImages(((ToolStrip)ctl).Items);
                }
                else
                {
                    //If control is a control, replace images with cached versions.
                    SetControlImage(ctl);
                }
                //If the control has children, recurse.
                if (ctl.HasChildren)
                    CacheControlImages(ctl);
            }
        }

        /// <summary>
        /// This method manages the image cache. Images not found with the corresponding key are added to the cache. Returns a cached image.
        /// </summary>
        /// <param name="key">The identifying key for cached image. Can be anything, but a control.name of a frequently duplicated control is a good value.</param>
        /// <param name="image">The image to be cached.</param>
        /// <returns></returns>
        public static Image ImageCache(object key, Image image)
        {
            //Try to pull an image from the cache with a matching key.
            Image img = (Image)ImageCacheHashTable[key];
            //If no matching image was found, add it to the cache.
            if (img == null)
            {
                //Add a clone of the original image so the original can be safely disposed.
                ImageCacheHashTable[key] = image.Clone();
                //Toss that memory hogging lump in the garbage.
                image.Dispose();
                //Grab the new image from the cache.
                img = (Image)ImageCacheHashTable[key];
            }
            //Will always return a cached image.
            return img;
        }

        /// <summary>
        /// Recursively loops through a toolstrip and replaces any images found with a cached instance.
        /// </summary>
        /// <param name="tools"></param>
        private static void SetToolStripImages(ToolStripItemCollection tools)
        {
            foreach (ToolStripItem tool in tools)
            {
                if (tool.Image != null)
                {
                    var img = ImageCache(tool.Name, tool.Image);
                    tool.Image = img;
                }
                if (tool as ToolStripDropDownButton != null)
                    SetToolStripImages(((ToolStripDropDownButton)tool).DropDownItems);
            }
        }

        /// <summary>
        /// Sets a controls image to a cached instance. Checks the control type and sets the correct image property accordingly.
        /// </summary>
        /// <param name="ctl"></param>
        private static void SetControlImage(object ctl)
        {
            
            if (ctl is Button)
            {
                var but = (Button)ctl;
                if (but.BackgroundImage != null)
                {
                    var img = ImageCache(but.Name, but.BackgroundImage);
                    but.BackgroundImage = img;
                }
            }
            else if (ctl is PictureBox)
            {
                var pbox = (PictureBox)ctl;
                if (pbox.Image != null)
                {
                    var img = ImageCache(pbox.Name, pbox.Image);
                    pbox.Image = img;
                }
            }

            //switch (true)
            //{
            //    case object.ReferenceEquals(ctl.GetType(), typeof(Button)):
            //        var but = (Button)ctl;
            //        if (but.BackgroundImage != null)
            //        {
            //            var img = ImageCache(but.Name, but.BackgroundImage);
            //            but.BackgroundImage = img;
            //        }
            //        break;
            //    case object.ReferenceEquals(ctl.GetType(), typeof(PictureBox)):
            //        var pbox = (PictureBox)ctl;
            //        if (pbox.Image != null)
            //        {
            //            var img = ImageCache(pbox.Name, pbox.Image);
            //            pbox.Image = img;
            //        }
            //        break;
            //}
        }

    }
}
