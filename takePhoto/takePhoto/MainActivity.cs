	using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Content.PM;

namespace takePhoto
{
	[Activity (Label = "takePhoto", MainLauncher = true)]
	public class Activity1 : Activity
	{
		Java.IO.File _file; //kaj je ovo

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it

			//buttons declaration
			var button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += delegate {
				var intent = new Intent (MediaStore.ActionImageCapture);
				
				var availableActivities = this.PackageManager.QueryIntentActivities (intent, PackageInfoFlags.MatchDefaultOnly);
				
				if (availableActivities != null && availableActivities.Count > 0) {              
					var dir = new Java.IO.File (
						Android.OS.Environment.GetExternalStoragePublicDirectory (
						Android.OS.Environment.DirectoryPictures), "CameraAppDemo");     
					
					if (!dir.Exists ()) {
						dir.Mkdirs ();  
					}   
					
					_file = new Java.IO.File (dir, String.Format ("myPhoto{0}.jpg", Guid.NewGuid ()));     
					
					intent.PutExtra (MediaStore.ExtraOutput,
					                 Android.Net.Uri.FromFile (_file));                    
					StartActivityForResult (intent, 0);
				}
		};


		}
		protected override void OnActivityResult (int requestCode, Result resultCode,
		                                          Android.Content.Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			var imageView = FindViewById<ImageView> (Resource.Id.imageView1);
			
			// make it available in the gallery
			var mediaScanIntent =
				new Intent (Intent.ActionMediaScannerScanFile);
			var contentUri = Android.Net.Uri.FromFile (_file);
			mediaScanIntent.SetData (contentUri);
			this.SendBroadcast (mediaScanIntent);             
			
			// display in ImageView
			using(var bitmap = MediaStore.Images.Media.GetBitmap (ContentResolver, contentUri)){
				imageView.SetImageBitmap (bitmap);   
			}
		}
	}

}


