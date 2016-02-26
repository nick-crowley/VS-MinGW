// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Shared.InternalErrorException
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Microsoft.Build.Shared
{
  [Serializable]
  internal sealed class InternalErrorException : Exception
  {
    internal InternalErrorException()
    {
    }

    internal InternalErrorException(string message)
      : base("MSB0001: Internal MSBuild Error: " + message)
    {
      InternalErrorException.ConsiderDebuggerLaunch(message, (Exception) null);
    }

    internal InternalErrorException(string message, Exception innerException)
      : base("MSB0001: Internal MSBuild Error: " + message + (innerException == null ? string.Empty : "\n=============\n" + innerException.ToString() + "\n\n"), innerException)
    {
      InternalErrorException.ConsiderDebuggerLaunch(message, innerException);
    }

    private InternalErrorException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    private static void ConsiderDebuggerLaunch(string message, Exception innerException)
    {
      if (innerException != null)
      {
        innerException.ToString();
      }
      else
      {
        string str = string.Empty;
      }
      if (Environment.GetEnvironmentVariable("MSBUILDLAUNCHDEBUGGER") == null)
        return;
      Debugger.Launch();
    }
  }
}
