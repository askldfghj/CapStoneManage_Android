package md506f5d8d470dc445d621c844165e4e6c3;


public class mmsns_comment
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("App2.mmsns_comment, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", mmsns_comment.class, __md_methods);
	}


	public mmsns_comment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == mmsns_comment.class)
			mono.android.TypeManager.Activate ("App2.mmsns_comment, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public mmsns_comment (int p0, int p1, int p2, java.lang.String p3, java.lang.String p4) throws java.lang.Throwable
	{
		super ();
		if (getClass () == mmsns_comment.class)
			mono.android.TypeManager.Activate ("App2.mmsns_comment, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3, p4 });
	}

	java.util.ArrayList refList;
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
