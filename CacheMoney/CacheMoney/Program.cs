using System;

namespace CacheMoney
{
	/* Part 2 of Boomtown! employment test. Started at 3:40pm. */
	class MainClass
	{
		public const int PORT = 1234;

		public static void Main (string[] args)
		{
			CacheMoneyServer server = new CacheMoneyServer (PORT);
			server.Start ();
		}
	}
}
