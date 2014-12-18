using System;
using Gtk;
using SuperMaps;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void getInfoButton_clicked (object sender, EventArgs e)
	{
		GeoLocation geo = new GeoLocation(float.Parse(lonEntry.Text), float.Parse(latEntry.Text));
		myIPLabel.Text = geo.IP;
		myLonLabel.Text = geo.lon.ToString();
		myLatLabel.Text = geo.lat.ToString();
		cityLabel.Text = geo.city;
	}
}
