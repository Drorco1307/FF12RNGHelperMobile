using Android.Content;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace FF12RngHelper.Droid
{
    /// <summary>
    /// Rendering logic for custom toggle button
    /// </summary>
    public class ToggleSwitchRenderer : SwitchRenderer
    {
        private ToggleSwitch view;

        public ToggleSwitchRenderer(Context context) : base(context)
        {

        }

        #region Methods
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;
            view = (ToggleSwitch)Element;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                if (Control != null)
                {
                    if (Control.Checked)
                    {
                        Control.TrackDrawable.SetColorFilter(view.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                    }
                    else
                    {
                        Control.TrackDrawable.SetColorFilter(view.SwitchOffColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                    }
                    Control.CheckedChange += OnCheckedChange;
                    UpdateSwitchThumbImage(view);
                }
                //Control.TrackDrawable.SetColorFilter(view.SwitchBGColor.ToAndroid(), PorterDuff.Mode.Multiply);  
            }
        }

        private void UpdateSwitchThumbImage(ToggleSwitch view)
        {
            if (!string.IsNullOrEmpty(view.SwitchThumbImage))
            {
                view.SwitchThumbImage = view.SwitchThumbImage.Replace(".jpg", "").Replace(".png", "");
                int imgid = (int)typeof(Resource.Drawable).GetField(view.SwitchThumbImage).GetValue(null);
                Control.SetThumbResource(Resource.Drawable.notification_icon_background);
            }
            else
            {
                Control.ThumbDrawable.SetColorFilter(view.SwitchThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
                // Control.SetTrackResource(Resource.Drawable.track);  
            }
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (Control.Checked)
            {
                Control.TrackDrawable.SetColorFilter(view.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
            else
            {
                Control.TrackDrawable.SetColorFilter(view.SwitchOffColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Control.CheckedChange -= this.OnCheckedChange;
            base.Dispose(disposing);
        }
        #endregion
    }
}