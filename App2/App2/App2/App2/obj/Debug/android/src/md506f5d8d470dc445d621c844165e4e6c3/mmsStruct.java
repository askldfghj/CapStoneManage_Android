package md506f5d8d470dc445d621c844165e4e6c3;


public class mmsStruct
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("App2.mmsStruct, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", mmsStruct.class, __md_methods);
	}


	public mmsStruct () throws java.lang.Throwable
	{
		super ();
		if (getClass () == mmsStruct.class)
			mono.android.TypeManager.Activate ("App2.mmsStruct, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public mmsStruct (int p0, int p1, java.lang.String p2, java.lang.String p3) throws java.lang.Throwable
	{
		super ();
		if (getClass () == mmsStruct.class)
			mono.android.TypeManager.Activate ("App2.mmsStruct, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
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
