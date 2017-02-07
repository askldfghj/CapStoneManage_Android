package md506f5d8d470dc445d621c844165e4e6c3;


public class Account_Contents
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("App2.Account_Contents, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Account_Contents.class, __md_methods);
	}


	public Account_Contents () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Account_Contents.class)
			mono.android.TypeManager.Activate ("App2.Account_Contents, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public Account_Contents (int p0, int p1, java.lang.String p2, java.lang.String p3, int p4, int p5) throws java.lang.Throwable
	{
		super ();
		if (getClass () == Account_Contents.class)
			mono.android.TypeManager.Activate ("App2.Account_Contents, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3, p4, p5 });
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
