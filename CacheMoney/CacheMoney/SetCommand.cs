using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CacheMoney
{
	public class SetCommand : Command
	{
		public SetCommand ()
		{
		}

		public String CommandName() {
			return "set";
		}

		public String Process(Item item, String key, Dictionary<String, Item> store) {
			store.Add (key, item);

			return JsonConvert.SerializeObject(item);
		}
	}
}

