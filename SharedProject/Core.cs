using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Media;
using System.Collections.Generic;


#if __ANDROID__
using Android.Content;
#endif

namespace SharedProject
{
	public partial class Core
	{
		public enum EmotionType
		{
			Happiness,
			Sadness,
			Contempt,
			Fear,
			Disgust,
			Neutral,
			Anger
		}

		const string emotionKey = "88f748eefd944a5d8d337a1765414bba";

		//TODO : Tarea 3 Función encargada de consumir la API de Microsoft
		private static async Task<Emotion[]> GetScores (Stream stream)
		{



			EmotionServiceClient emotionClient = new EmotionServiceClient (emotionKey);

			//TODO : Tarea 4 Función asíncrona
			var emotionResults = await emotionClient.RecognizeAsync (stream);

			if (emotionResults == null || emotionResults.Count () == 0) {
				throw new Exception ("Can't detect face");
			}

			return emotionResults;
		}

		public static int peopleInPhoto = 0;
		//TODO : Tarea 5 Función encargada de obtener el promedio de las emociones
		public static async Task<Dictionary<EmotionType, float>> GetAverageEmotionsScore (Stream stream)
		{
			Dictionary<EmotionType, float> result = null;
			float scoreAnger = 0, scoreNeutral = 0, scoreHappiness = 0, scoreSadness = 0, scoreDisgust = 0, scoreFear = 0, scoreContempt = 0;
			try {
				Emotion[] emotionResults = await GetScores (stream); 
				peopleInPhoto = emotionResults.Count();
				foreach (var emotionResult in emotionResults) {
					scoreHappiness = scoreHappiness + emotionResult.Scores.Happiness;
					scoreSadness = scoreSadness + emotionResult.Scores.Sadness;
					scoreDisgust = scoreDisgust + emotionResult.Scores.Disgust;
					scoreFear = scoreFear + emotionResult.Scores.Fear;
					scoreContempt = scoreContempt + emotionResult.Scores.Contempt;
					scoreNeutral = scoreNeutral + emotionResult.Scores.Neutral;
					scoreAnger = scoreAnger + emotionResult.Scores.Anger;
				}

				result = new Dictionary<EmotionType, float> ();
				result.Add (EmotionType.Happiness, scoreHappiness / emotionResults.Count ());
				result.Add (EmotionType.Sadness, scoreSadness / emotionResults.Count ());
				result.Add (EmotionType.Disgust, scoreDisgust / emotionResults.Count ());
				result.Add (EmotionType.Contempt, scoreContempt / emotionResults.Count ());
				result.Add (EmotionType.Fear, scoreFear / emotionResults.Count ());
				result.Add (EmotionType.Neutral, scoreNeutral / emotionResults.Count ());
				result.Add (EmotionType.Anger, scoreAnger / emotionResults.Count ());

			} catch (Exception e) {
				result = null;
			}
			return result;
		}

		public static string GetResultMessage (Dictionary<EmotionType, float>  scores)
		{
			//TODO : Tarea 6 Linq
			var orderedScores = scores.OrderByDescending (score => score.Value);

			var higestScore = orderedScores.ElementAt (0).Value * 100;
			double result = Math.Round (higestScore, 2);
			return string.Format("{0} personas {1} al {2}%",peopleInPhoto, orderedScores.ElementAt (0).Key, higestScore);

		}


		//TODO : Tarea 1 Función encargada de tomar la foto con un componente
#if __ANDROID__
		public async static Task<Byte[]> TakePhoto (Context context)
		{
			MediaPicker picker = new MediaPicker (context);
#elif __IOS__
		public async static Task<Byte[]> TakePhoto ()
		{
			MediaPicker picker = new MediaPicker ();
#endif
			byte[] imageData;
			StoreCameraMediaOptions options = new StoreCameraMediaOptions ();
			options.DefaultCamera = CameraDevice.Rear;
			options.Name = "photo";
			var photo = await picker.TakePhotoAsync (options);
			Stream stream = photo.GetStream ();

#if __IOS__
		    //TODO : Tarea 2 Reducir el tamaño de la foto para evitar que sea demasiado pesada
		    stream = ResizeImage (stream,50);
#endif
				using (MemoryStream ms = new MemoryStream ()) {
					stream.CopyTo (ms);
					imageData = ms.ToArray ();
				}
			stream.Dispose();
			return imageData;
		}



	}
}