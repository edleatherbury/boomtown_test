using System;

namespace CacheMoney
{
	public class Item
	{
		public uint Flag 		{ get; set; }
		public long Expiration 	{ get; set; }
		public long CAS 		{ get; set; }
		public string Data 		{ get; set; }

		public Item ()
		{
		}
	}
}

