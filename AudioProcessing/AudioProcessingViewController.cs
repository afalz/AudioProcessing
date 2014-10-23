using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.MediaToolbox;
using MonoTouch.AudioToolbox;
using MonoTouch.CoreMedia;

namespace AudioProcessing
{
	public partial class AudioProcessingViewController : UIViewController
	{
		public AudioProcessingViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			AVAsset asset = AVAsset.FromUrl (NSBundle.MainBundle.GetUrlForResource ("sample_mpeg4", "mp4"));
			AVPlayerItem playerItem = new AVPlayerItem (asset);
			AVPlayer player = new AVPlayer (playerItem); 

			AVAssetTrack track = asset.Tracks [0];
			AVMutableAudioMixInputParameters inputParams = AVMutableAudioMixInputParameters.FromTrack (track);

			MTAudioProcessingTapCallbacks tapCallbacks = new MTAudioProcessingTapCallbacks (
				delegate(MTAudioProcessingTap tap, long numberFrames, MTAudioProcessingTapFlags flags, 
					AudioBuffers bufferList, out long numberFramesOut, out MTAudioProcessingTapFlags flagsOut) {
					CMTimeRange timeRange;
					numberFramesOut = numberFrames;
					tap.GetSourceAudio (numberFrames, bufferList, out flagsOut, out timeRange, 0);
				}
			);
			MTAudioProcessingTap processingTap = new MTAudioProcessingTap (tapCallbacks, MTAudioProcessingTapCreationFlags.PostEffects);

			inputParams.AudioTapProcessor = processingTap;

			AVMutableAudioMix audioMix = new AVMutableAudioMix ();
			audioMix.InputParameters = new AVAudioMixInputParameters[] { inputParams };
			playerItem.AudioMix = audioMix;

			player.Play ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

