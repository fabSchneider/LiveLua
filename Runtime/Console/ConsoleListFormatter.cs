using Fab.Lua.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fab.Lua.Console
{
    public class ConsoleListFormatter : ILuaHelpFormatter
    {
        public string Format(LuaHelpInfo helpInfo)
        {
            return $"<b>{helpInfo.name}</b>{Environment.NewLine}<i>{helpInfo.description}</i>{Environment.NewLine}";
        }
    }
}
