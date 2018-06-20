package md53564ddc48a0a3b7443e0a24b22d00690;


public class QuartzDrawable
	extends android.graphics.drawable.Drawable
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_draw:(Landroid/graphics/Canvas;)V:GetDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_getOpacity:()I:GetGetOpacityHandler\n" +
			"n_setAlpha:(I)V:GetSetAlpha_IHandler\n" +
			"n_setColorFilter:(Landroid/graphics/ColorFilter;)V:GetSetColorFilter_Landroid_graphics_ColorFilter_Handler\n" +
			"";
		mono.android.Runtime.register ("ThinkGeo.MapSuite.AndroidEdition.QuartzDrawable, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", QuartzDrawable.class, __md_methods);
	}


	public QuartzDrawable () throws java.lang.Throwable
	{
		super ();
		if (getClass () == QuartzDrawable.class)
			mono.android.TypeManager.Activate ("ThinkGeo.MapSuite.AndroidEdition.QuartzDrawable, AndroidEdition, Version=8.0.0.0, Culture=neutral, PublicKeyToken=0828af5241fb4207", "", this, new java.lang.Object[] {  });
	}


	public void draw (android.graphics.Canvas p0)
	{
		n_draw (p0);
	}

	private native void n_draw (android.graphics.Canvas p0);


	public int getOpacity ()
	{
		return n_getOpacity ();
	}

	private native int n_getOpacity ();


	public void setAlpha (int p0)
	{
		n_setAlpha (p0);
	}

	private native void n_setAlpha (int p0);


	public void setColorFilter (android.graphics.ColorFilter p0)
	{
		n_setColorFilter (p0);
	}

	private native void n_setColorFilter (android.graphics.ColorFilter p0);

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
