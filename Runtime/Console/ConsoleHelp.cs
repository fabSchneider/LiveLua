using System;
using Fab.Lua.Core;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace Fab.Lua.Console
{
	/// <summary>
	/// Class implementing console help methods
	/// </summary>
    public class ConsoleHelp : IConsoleCommand
    {
		private LuaHelpInfoCache helpInfoCache;
		private ILuaHelpFormatter helpFormatter;
		private ILuaHelpFormatter listFormatter;

		public ConsoleHelp(ILuaHelpFormatter helpFormatter, ILuaHelpFormatter listFormatter)
		{
			helpInfoCache = new LuaHelpInfoCache();
			this.helpFormatter = helpFormatter;
			this.listFormatter = listFormatter;
		}

		void IConsoleCommand.Register(Console console)
		{
			console.RegisterMethod<DynValue>("help", (value) => help(console, value));
			console.RegisterMethod("list", () => list(console));
		}

		[LuaHelpInfo("Show help information for the module or method with the given name")]
		private void help(Console console, DynValue name)
		{
			switch (name.Type)
			{
				case DataType.Nil:
					console.Print("Nil");
					break;
				case DataType.Void:
					console.Print("Type <b>help(<i>name</i>)</b> to show help information for the module or method with the given name." + Environment.NewLine +
						"Type <b>list()</b> to get a list of all available modules.");
					break;
				case DataType.UserData:
					IUserDataDescriptor descriptor = name.UserData.Descriptor;
					if (descriptor is ProxyUserDataDescriptor proxyDescriptor)
						descriptor = proxyDescriptor.InnerDescriptor;

					LuaHelpInfo helpInfo = helpInfoCache.GetHelpInfoForUserData((StandardUserDataDescriptor)descriptor);
					string formatted = helpFormatter.Format(helpInfo);
					console.Print(formatted);
					break;
				case DataType.ClrFunction:
					break;
				default:
					console.Print("No help information available");
					break;
			}
		}

		[LuaHelpInfo("Lists all available modules")]
		private void list(Console console)
		{
			foreach (StandardUserDataDescriptor descriptor in LuaEnvironment.Registry.GetRegisteredTypes(true))
			{
				LuaHelpInfo helpInfo = helpInfoCache.GetHelpInfoForUserData(descriptor);
				console.Print(listFormatter.Format(helpInfo));
			}
		}
	}
}
