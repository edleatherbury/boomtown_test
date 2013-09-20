using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CacheMoney
{
	public class CacheMoneyServer
	{
		private HttpListener listener;
		private int port;

		private Dictionary<String, Item> Store;

		private Dictionary<String, Command> Commands;

		public CacheMoneyServer (int port)
		{
			this.port = port;
			listener = new HttpListener ();
			listener.Prefixes.Add ("http://+:" + port + "/");

			Store = new Dictionary<String, Item> ();

			Commands = new Dictionary<String, Command> ();
			RegisterCommand (new SetCommand());
			RegisterCommand (new GetCommand());
		}

		private void RegisterCommand(Command c) {
			Console.WriteLine ("Registered command " + c.CommandName());
			Commands.Add (c.CommandName (), c);
		}

		public void Start() {
			listener.Start();
			Console.WriteLine ("CacheMoney server started on port " + port);
			while (listener.IsListening) {
				IAsyncResult result = listener.BeginGetContext (new AsyncCallback (HandleRequest), listener);
				result.AsyncWaitHandle.WaitOne ();
			}
		}

		public void Stop() {
			listener.Stop();
			Console.WriteLine ("CacheMoney server stopped");
		}

		private void HandleRequest(IAsyncResult result)
		{
			HttpListener listener = (HttpListener) result.AsyncState;
			HttpListenerContext context = listener.EndGetContext(result);
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;

			/* First, we should only accept GET or POST requests. */
			if ((request.HttpMethod != "POST") && (request.HttpMethod != "GET")) {
				response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
				response.Close ();
				Console.WriteLine ("Unsupported method " + request.HttpMethod);
			} else {
				/* If we've gotten this far, parse out the request URI. */
				string[] chunks = request.RawUrl.Split ('/');
				string CommandName = chunks [1];
				string key = chunks [2];
				
				/* If we have a command to process this request, do so. */
				Command cmd;
				if (Commands.TryGetValue (CommandName, out cmd)) {
					/* If this is a retrieval operation (a GET), don't bother parsing a JSON payload. */
					Item item = null;
					if ((request.HttpMethod == "POST") && (request.HasEntityBody)) {
						System.IO.StreamReader reader = new System.IO.StreamReader (request.InputStream, request.ContentEncoding);
						String payload = reader.ReadToEnd ();
						reader.Close ();
						item = JsonConvert.DeserializeObject<Item>(payload);
					}

					/* Actually process the request! */
					String retval = cmd.Process (item, key, Store);

					/* And ship the output. */
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes (retval);
					response.ContentLength64 = buffer.Length;
					System.IO.Stream output = response.OutputStream;
					output.Write (buffer, 0, buffer.Length);

					output.Close ();
				} else {
					Console.WriteLine ("Unimplemented command " + CommandName);
					response.StatusCode = (int)HttpStatusCode.NotImplemented;
					response.Close ();						
				}
			}
		}
	}
}
