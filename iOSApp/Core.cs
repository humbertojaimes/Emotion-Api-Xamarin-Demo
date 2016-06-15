using System;
using System.IO;

namespace SharedProject
{

	public static class ImageExtensions
	{
		public static UIKit.UIImage ToImage (this byte[] data)
		{
			if (data == null) {
				return null;
			}

			UIKit.UIImage image;
			try {
				image = new UIKit.UIImage (Foundation.NSData.FromArray (data));
			} catch (Exception e) {
				return null;
			}
				
			return image;
		}

	}


	public partial class Core
	{
		//TODO: Tarea 2.1 Implementación iOS
		public static Stream ResizeImage (Stream inputImage, float percetangeToReduce)
		{

			byte[] imageData;

			using (MemoryStream ms = new MemoryStream())
			{
				inputImage.CopyTo(ms);
				imageData = ms.ToArray();
			}


			UIKit.UIImage originalImage = imageData.ToImage();
			var percentage = percetangeToReduce / 100;
			float width = (float)originalImage.Size.Width * percentage ;
			float height = (float)originalImage.Size.Width * percentage;
			UIKit.UIImageOrientation orientation = originalImage.Orientation;

			//create a 24bit RGB image
			using (CoreGraphics.CGBitmapContext context = new CoreGraphics.CGBitmapContext (IntPtr.Zero,
				(int)width, (int)height, 8,
				(int)(4 * width), CoreGraphics.CGColorSpace.CreateDeviceRGB (),
				CoreGraphics.CGImageAlphaInfo.PremultipliedFirst)) {

				System.Drawing.RectangleF imageRect = new System.Drawing.RectangleF (0, 0, width, height);

				// draw the image
				context.DrawImage (imageRect, originalImage.CGImage);

				UIKit.UIImage resizedImage = UIKit.UIImage.FromImage (context.ToImage (), 0, orientation);

				// save the image as a jpeg
				return new MemoryStream(resizedImage.AsJPEG ().ToArray ());
			}
		}




	}
}

