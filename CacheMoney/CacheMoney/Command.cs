using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CacheMoney
{
	/* Interface for memcache commands. */
	public interface Command
	{
		String Process (Item item, String key, Dictionary<String,Item> store);
		String CommandName();
	}
}

