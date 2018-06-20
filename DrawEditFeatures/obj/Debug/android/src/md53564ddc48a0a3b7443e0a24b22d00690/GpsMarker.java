package md53564ddc48a0a3b7443e0a24b22d00690;


public class GpsMarker
	extends md53564ddc48a0a3b7443e0a24b22d00690.Marker
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_dispatchDraw:(Landroid/graphics/Canvas;)V:GetDispatchDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("ThinkGeo.MapSuite.AndroidEdition.GpsMarker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", GpsMarker.class, __md_methods);
	}


	public GpsMarker (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == GpsMarker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.GpsMarker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public GpsMarker (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == GpsMarker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.GpsMarker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public GpsMarker (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == GpsMarker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.GpsMarker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public GpsMarker (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == GpsMarker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.GpsMarker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void dispatchDraw (android.graphics.Canvas p0)
	{
		n_dispatchDraw (p0);
	}

	private native void n_dispatchDraw (android.graphics.Canvas p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
