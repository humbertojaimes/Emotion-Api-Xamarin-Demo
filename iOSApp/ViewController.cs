using System;
using System.IO;
using Foundation;
using UIKit;
using SharedProject;


namespace iOSApp
{
	public partial class ViewController : UIViewController
	{
		protected ViewController (IntPtr handle) : base (handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		
		public override void ViewDidAppear (bool animated)
		{
			
		}

		public override void ViewDidLoad ()
		{
			//TODO : Tarea 7 Trabajar con la interfaz gráfica en iOS 
			TakePhotoButton.TouchDown += OnTakePhotoButtonPressed;
		}

		async void OnTakePhotoButtonPressed (object sender, EventArgs eventArgs){
			var photo = await Core.TakePhoto ();
			ThePhoto.Image = photo.ToImage ();

			using (Stream stream = new MemoryStream (photo)) {
				var averages = await SharedProject.Core.GetAverageEmotionsScore (stream);
				DetailsText.Text = SharedProject.Core.GetResultMessage (averages);
			}
		}


	}
}

