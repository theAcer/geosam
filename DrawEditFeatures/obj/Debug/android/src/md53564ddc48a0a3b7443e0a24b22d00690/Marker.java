package md53564ddc48a0a3b7443e0a24b22d00690;


public class Marker
	extends android.widget.ImageView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMeasure:(II)V:GetOnMeasure_IIHandler\n" +
			"";
		mono.android.Runtime.register ("ThinkGeo.MapSuite.AndroidEdition.Marker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", Marker.class, __md_methods);
	}


	public Marker (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == Marker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.Marker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public Marker (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == Marker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.Marker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public Marker (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == Marker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.Marker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public Marker (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == Marker.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.Marker, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onMeasure (int p0, int p1)
	{
		n_onMeasure (p0, p1);
	}

	private native void n_onMeasure (int p0, int p1);

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
