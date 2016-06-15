using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;
using SharedProject;
using System.IO;

namespace AndroidApp
{
    [Activity(Label = "AndroidApp", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        public static Bitmap _bitmap;
        private ImageView _imageView;
        private Button _pictureButton;
        private TextView _resultTextView;
       
       

       

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
			//TODO : Tarea 8 Trabajar con la interfaz gráfica en Android 
			_pictureButton = FindViewById<Button>(Resource.Id.GetPictureButton);
			_pictureButton.Click += pictureButtonClick;
			_resultTextView = FindViewById<TextView>(Resource.Id.resultText);
			_imageView = FindViewById<ImageView>(Resource.Id.imageView1);

//           
        }

		private async void pictureButtonClick(object sender, EventArgs eventArgs){
		    
			var photo =	await Core.TakePhoto (this);

			Bitmap bmpPhoto = BitmapFactory.DecodeByteArray (photo, 0, photo.Length);
			_imageView.SetImageBitmap (bmpPhoto);

			using (Stream stream = new MemoryStream (photo)) {
				var averages = await SharedProject.Core.GetAverageEmotionsScore (stream);
				_resultTextView.Text = SharedProject.Core.GetResultMessage (averages);
			}

		}


      
    }
}
