using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CacheMoney
{
	public class GetCommand : Command
	{
		public GetCommand ()
		{
		}

		public String CommandName() {
			return "get";
		}

		public String Process(Item item, String key, Dictionary<String, Item> store) {
			Item i;

			if (store.TryGetValue (key, out i)) {
				return JsonConvert.SerializeObject(i);
			}
			else {
				return "{}";
			}
		}
	}
}

