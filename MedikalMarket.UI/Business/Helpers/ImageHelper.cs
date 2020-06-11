using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace MedikalMarket.UI.Business.Helpers
{
    public static class ImageHelper
    {
        private const int OrientationKey = 0x0112;
        private const int NotSpecified = 0;
        private const int NormalOrientation = 1;
        private const int MirrorHorizontal = 2;
        private const int UpsideDown = 3;
        private const int MirrorVertical = 4;
        private const int MirrorHorizontalAndRotateRight = 5;
        private const int RotateLeft = 6;
        private const int MirorHorizontalAndRotateLeft = 7;
        private const int RotateRight = 8;

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


        public static Bitmap RezImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static bool ResizeAndCompressImage(string orgFile, string resizedFile, ImageFormat format, int width, int height)
        {
            try
            {
                using (Image img = Image.FromFile(orgFile))
                {
                    CheckOrientation(img);
                    Image thumbNail = new Bitmap(width, height, img.PixelFormat);
                    Graphics g = Graphics.FromImage(thumbNail);
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingMode = CompositingMode.SourceCopy; //sonradadn ekledim.
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    g.DrawImage(img, rect);
                    thumbNail.Save(resizedFile, format);

                    #region MyRegion
                    //90 derece sağa(bir tık sağa)
                    //thumbNail.RotateFlip(RotateFlipType.Rotate90FlipNone);

                    //180 derece sağa(tepetaklak)
                    //thumbNail.RotateFlip(RotateFlipType.Rotate180FlipNone);

                    //270 derece sağa (bir tık sola)
                    //thumbNail.RotateFlip(RotateFlipType.Rotate270FlipNone);


                    //thumbNail.RotateFlip(RotateFlipType.RotateNoneFlipNone);

                    //thumbNail.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    //thumbNail.Save(resizedFile, format);


                    //thumbNail.RotateFlip(RotateFlipType.Rotate90FlipX);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.Rotate90FlipXY);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.Rotate90FlipY);
                    //thumbNail.Save(resizedFile, format);


                    //thumbNail.RotateFlip(RotateFlipType.Rotate180FlipX);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.Rotate180FlipXY);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.Rotate180FlipY);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.Rotate270FlipX);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.Rotate270FlipXY);
                    //thumbNail.Save(resizedFile, format);

                    //thumbNail.RotateFlip(RotateFlipType.Rotate270FlipY);
                    //thumbNail.Save(resizedFile, format);
                    #endregion
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Image GetCompressedBitmap(Bitmap bmp, long quality)
        {
            using (var mss = new MemoryStream())
            {
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = qualityParam;
                bmp.Save(mss, imageCodec, parameters);
                return Image.FromStream(mss);
            }
        }

        public static Bitmap CreateBitmapImageFromText(string imageText)
        {
            Bitmap objBmpImage = new Bitmap(1, 1);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth = (int)objGraphics.MeasureString(imageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(imageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.White);
            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            objGraphics.DrawString(imageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);
            objGraphics.Flush();
            return (objBmpImage);
        }

        public static Bitmap WatermarkImage(Bitmap image, Bitmap watermark)
        {
            using (Graphics imageGraphics = Graphics.FromImage(image))
            {
                watermark.SetResolution(imageGraphics.DpiX, imageGraphics.DpiY);

                int x = (image.Width - watermark.Width) / 2;
                int y = (image.Height - watermark.Height) / 2;

                imageGraphics.DrawImage(watermark, x, y, watermark.Width, watermark.Height);
            }

            return image;
        }

        public static Stream ConvertImage(this Stream originalStream, ImageFormat format)
        {
            var image = Image.FromStream(originalStream);

            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public static void CheckOrientation(Image image)
        {
            // Fix orientation if needed.
            if (image.PropertyIdList.Contains(OrientationKey))
            {
                var orientation = (int)image.GetPropertyItem(OrientationKey).Value[0];
                switch (orientation)
                {
                    case NotSpecified: // Assume it is good.
                    case NormalOrientation:
                        // No rotation required.
                        break;
                    case MirrorHorizontal:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case UpsideDown:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case MirrorVertical:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case MirrorHorizontalAndRotateRight:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case RotateLeft:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case MirorHorizontalAndRotateLeft:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case RotateRight:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
                }
            }
        }
    }





    //public class Size
    //{
    //    public Size(int width, int height)
    //    {
    //        Width = width;
    //        Height = height;
    //    }
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //}
}
