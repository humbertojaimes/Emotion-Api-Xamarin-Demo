using System;
using Android.Graphics;
using System.IO;

namespace SharedProject
{
	public partial class Core
	{
		//TODO: Tarea 2.2 Implementación Android
		public static Stream ResizeImage (Stream imageData, float percentageToReduce)
		{
			MemoryStream imageS = new MemoryStream ();
			imageData.CopyTo (imageS);
			imageS.Position = 0;
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeStream (imageS);
			float percentage = percentageToReduce / 100;
			float width = originalImage.Width * percentage;
			float height = originalImage.Height * percentage;
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);


			resizedImage.Compress (Bitmap.CompressFormat.Jpeg, 70, imageS);
			return imageS;
			
		}
	}
}

