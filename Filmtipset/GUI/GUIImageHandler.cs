using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using MediaPortal.GUI.Library;
using System.Drawing;
using MediaPortal.Util;
using System.Drawing.Imaging;

namespace Filmtipset.GUI
{
    [Flags]
    public enum MainOverlayImage
    {
        None = 0,
        Seen = 1,
    }

    public enum RatingOverlayImage
    {
        None = 100,
        Grade0 = 0,
        Grade1 = 1,
        Grade2 = 2,
        Grade3 = 3,
        Grade4 = 4,
        Grade5 = 5
    }

    public static class GUIImageHandler
    {
        internal static string GetDefaultPoster(bool largePoster = true)
        {
            return largePoster ? "defaultVideoBig.png" : "defaultVideo.png";
        }


        internal static string GetWatchedIcon(bool seen)
        {
            if (seen)
                return "FilmtipsetWatched.png";
            return "";
        }

        internal static string GetGradeIcon(int grade)
        {
            return string.Format("FilmtipsetBadgeGrade{0}.png",grade);
        }


        public static bool DoLocalFileExist(string localFile)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localFile));
                return File.Exists(localFile);
            }
            catch { return false; }
        }

        /// <summary>
        /// Download an image if it does not exist locally
        /// </summary>
        /// <param name="url">Online URL of image to download</param>
        /// <param name="localFile">Local filename to save image</param>
        /// <returns>true if image downloads successfully or loads from disk successfully</returns>
        public static bool DownloadImage(string url, string localFile)
        {
            WebClient webClient = new WebClient();

            // Ignore Image placeholders (series/movies with no artwork)
            // use skins default images instead
            try
            {
                if (!DoLocalFileExist(localFile))
                {
                    Log.Debug("[Filmtipset] Downloading new image from: {0} to {1}", url, localFile);
                    if (url.ToLower().Contains("fanart.tv")) //Scale down some
                    {
                        try
                        {
                            byte[] data = webClient.DownloadData(url);
                            System.IO.MemoryStream memStream = new System.IO.MemoryStream(data);
                            //resize the image to the specified height and width
                            Image image = Image.FromStream(memStream);
                            if (image.Width == 1000)
                            {
                                using (var resized = ImageUtilities.ResizeImage(image, FilmtipsetSettings.ThumbWidth, FilmtipsetSettings.ThumbHeight))
                                {
                                    //save the resized image as a jpeg with a quality of 90
                                    ImageUtilities.SaveJpeg(localFile, resized, 90);
                                }
                            }
                            else
                            {
                                image.Save(localFile, ImageFormat.Jpeg);
                            }
                            memStream.Dispose();
                        }
                        catch
                        {
                            webClient.DownloadFile(url, localFile);

                        }
                    }
                    else
                    {
                        webClient.DownloadFile(url, localFile);
                    }
                    webClient.Dispose();

                }
                return true;
            }
            catch (Exception e)
            {
                Log.Warn("[Filmtipset] Image download failed from '{0}' to '{1}' error {2}", url, localFile, e.Message);
                try { if (File.Exists(localFile)) File.Delete(localFile); }
                catch { }
                return false;
            }
        }

        public static string GetTextureIdentFromFile(string filename, string suffix)
        {
            return "[Filmtipset:" + (filename + suffix).GetHashCode() + "]";
        }

        public static Bitmap DrawOverlayOnPoster(string origPoster, MainOverlayImage mainType, RatingOverlayImage ratingType)
        {
            return DrawOverlayOnPoster(origPoster, mainType, ratingType, new Size());
        }

        /// <summary>
        /// Draws a filmtipset overlay, library/seen/watchlist icon on a poster
        /// This is done in memory and wont touch the existing file
        /// </summary>
        /// <param name="origPoster">Filename of the untouched poster</param>
        /// <param name="type">Overlay type enum</param>
        /// <param name="size">Size of returned image</param>
        /// <returns>An image with overlay added to poster</returns>
        public static Bitmap DrawOverlayOnPoster(string origPoster, MainOverlayImage mainType, RatingOverlayImage ratingType, Size size)
        {
            Image image = GUIImageHandler.LoadImage(origPoster);
            if (image == null) return null;
            Bitmap poster = new Bitmap(image, size);
            Graphics gph = Graphics.FromImage(poster);

            string mainOverlayImage = GUIGraphicsContext.Skin + string.Format(@"\Media\Filmtipset{0}.png", mainType.ToString().Replace(", ", string.Empty));
            if (mainType != MainOverlayImage.None && File.Exists(mainOverlayImage))
            {
                Bitmap newPoster = new Bitmap(GUIImageHandler.LoadImage(mainOverlayImage));
                gph.DrawImage(newPoster, poster.Width -122 /*58*/, 0);
            }

            string ratingOverlayImage = GUIGraphicsContext.Skin + string.Format(@"\Media\Filmtipset{0}.png", Enum.GetName(typeof(RatingOverlayImage), ratingType));
            if (ratingType != RatingOverlayImage.None && File.Exists(ratingOverlayImage))
            {
                Bitmap newPoster = new Bitmap(GUIImageHandler.LoadImage(ratingOverlayImage));
                gph.DrawImage(newPoster, poster.Width - 122 /*58*/, 0);
            }
            gph.Dispose();
            return poster;
        }

        /// <summary>
        /// Loads an image FAST from file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Image LoadImage(string file)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file)) return null;

            Image img = null;

            try
            {
                img = ImageFast.FromFile(file);
            }
            catch
            {
                // Most likely a Zero Byte file but not always
                Log.Warn("[Filmtipset] Fast loading of texture {0} failed - trying safe fallback now", file);
                try { img = Image.FromFile(file); }
                catch { }
            }

            return img;
        }

        public static void LoadFanart(ImageSwapper backdrop, string filename)
        {
            // Dont activate and load if user does not want to download fanart
            if (!FilmtipsetSettings.EnableFanart)
            {
                if (backdrop.Active) backdrop.Active = false;
                return;
            }

            // Activate Backdrop in Image Swapper
            if (!backdrop.Active) backdrop.Active = true;

            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
                filename = string.Empty;

            // Assign Fanart filename to Image Loader
            // Will display fanart in backdrop or reset to default background
            backdrop.Filename = filename;
        }


    }

    /// <summary>
    /// Provides various image untilities, such as high quality resizing and the ability to save a JPEG.
    /// Stolen from http://stackoverflow.com/questions/249587/high-quality-image-scaling-c-sharp
    /// </summary>
    public static class ImageUtilities
    {
        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> encoders = null;

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return encoders;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);
            //set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        /// <param name="quality">An integer from 0 to 100, with 100 being the 
        /// highest quality</param> 
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(path, jpegCodec, encoderParams);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }
    }

}
