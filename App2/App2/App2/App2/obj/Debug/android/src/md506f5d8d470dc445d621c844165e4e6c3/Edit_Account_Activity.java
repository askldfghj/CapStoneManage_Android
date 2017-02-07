package md506f5d8d470dc445d621c844165e4e6c3;


public class Edit_Account_Activity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("App2.Edit_Account_Activity, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Edit_Account_Activity.class, __md_methods);
	}


	public Edit_Account_Activity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Edit_Account_Activity.class)
			mono.android.TypeManager.Activate ("App2.Edit_Account_Activity, App2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
