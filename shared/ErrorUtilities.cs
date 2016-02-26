// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Shared.ErrorUtilities
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Microsoft.Build.Shared
{
  internal static class ErrorUtilities
  {
    private static readonly bool throwExceptions = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MSBUILDDONOTTHROWINTERNAL"));
    private static readonly bool enableMSBuildDebugTracing = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MSBUILDENABLEDEBUGTRACING"));

    public static void DebugTraceMessage(string category, string formatstring, params object[] parameters)
    {
      if (!ErrorUtilities.enableMSBuildDebugTracing)
        return;
      if (parameters != null)
        Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.CurrentCulture, formatstring, parameters), category);
      else
        Trace.WriteLine(formatstring, category);
    }

    internal static void ThrowInternalError(string message, params object[] args)
    {
      if (ErrorUtilities.throwExceptions)
        throw new InternalErrorException(ResourceUtilities.FormatString(message, args));
    }

    internal static void ThrowInternalError(string message, Exception innerException, params object[] args)
    {
      if (ErrorUtilities.throwExceptions)
        throw new InternalErrorException(ResourceUtilities.FormatString(message, args), innerException);
    }

    internal static void ThrowInternalErrorUnreachable()
    {
      if (ErrorUtilities.throwExceptions)
        throw new InternalErrorException("Unreachable?");
    }

    internal static void ThrowIfTypeDoesNotImplementToString(object param)
    {
    }

    internal static void VerifyThrowInternalNull(object parameter, string parameterName)
    {
      if (parameter != null)
        return;
      ErrorUtilities.ThrowInternalError("{0} unexpectedly null", (object) parameterName);
    }

    internal static void VerifyThrowInternalLockHeld(object locker)
    {
      if (Monitor.IsEntered(locker))
        return;
      ErrorUtilities.ThrowInternalError("Lock should already have been taken");
    }

    internal static void VerifyThrowInternalLength(string parameterValue, string parameterName)
    {
      ErrorUtilities.VerifyThrowInternalNull((object) parameterValue, parameterName);
      if (parameterValue.Length != 0)
        return;
      ErrorUtilities.ThrowInternalError("{0} unexpectedly empty", (object) parameterName);
    }

    internal static void VerifyThrowInternalRooted(string value)
    {
      if (Path.IsPathRooted(value))
        return;
      ErrorUtilities.ThrowInternalError("{0} unexpectedly not a rooted path", (object) value);
    }

    internal static void VerifyThrow(bool condition, string unformattedMessage)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInternalError(unformattedMessage, (Exception) null, (object[]) null);
    }

    internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInternalError(unformattedMessage, arg0);
    }

    internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0, object arg1)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInternalError(unformattedMessage, new object[2]
      {
        arg0,
        arg1
      });
    }

    internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0, object arg1, object arg2)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInternalError(unformattedMessage, arg0, arg1, arg2);
    }

    internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0, object arg1, object arg2, object arg3)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInternalError(unformattedMessage, arg0, arg1, arg2, arg3);
    }

    internal static void ThrowInvalidOperation(string resourceName, params object[] args)
    {
      if (ErrorUtilities.throwExceptions)
        throw new InvalidOperationException(ResourceUtilities.FormatResourceString(resourceName, args));
    }

    internal static void VerifyThrowInvalidOperation(bool condition, string resourceName)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInvalidOperation(resourceName, (object[]) null);
    }

    internal static void VerifyThrowInvalidOperation(bool condition, string resourceName, object arg0)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInvalidOperation(resourceName, arg0);
    }

    internal static void VerifyThrowInvalidOperation(bool condition, string resourceName, object arg0, object arg1)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInvalidOperation(resourceName, arg0, arg1);
    }

    internal static void VerifyThrowInvalidOperation(bool condition, string resourceName, object arg0, object arg1, object arg2)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowInvalidOperation(resourceName, arg0, arg1, arg2);
    }

    internal static void ThrowArgument(string resourceName, params object[] args)
    {
      ErrorUtilities.ThrowArgument((Exception) null, resourceName, args);
    }

    private static void ThrowArgument(Exception innerException, string resourceName, params object[] args)
    {
      if (ErrorUtilities.throwExceptions)
        throw new ArgumentException(ResourceUtilities.FormatResourceString(resourceName, args), innerException);
    }

    internal static void VerifyThrowArgument(bool condition, string resourceName)
    {
      ErrorUtilities.VerifyThrowArgument(condition, (Exception) null, resourceName);
    }

    internal static void VerifyThrowArgument(bool condition, string resourceName, object arg0)
    {
      ErrorUtilities.VerifyThrowArgument(condition, (Exception) null, resourceName, arg0);
    }

    internal static void VerifyThrowArgument(bool condition, string resourceName, object arg0, object arg1)
    {
      ErrorUtilities.VerifyThrowArgument(condition, (Exception) null, resourceName, arg0, arg1);
    }

    internal static void VerifyThrowArgument(bool condition, string resourceName, object arg0, object arg1, object arg2)
    {
      ErrorUtilities.VerifyThrowArgument(condition, (Exception) null, resourceName, arg0, arg1, arg2);
    }

    internal static void VerifyThrowArgument(bool condition, string resourceName, object arg0, object arg1, object arg2, object arg3)
    {
      ErrorUtilities.VerifyThrowArgument(condition, (Exception) null, resourceName, arg0, arg1, arg2, arg3);
    }

    internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowArgument(innerException, resourceName, (object[]) null);
    }

    internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName, object arg0)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowArgument(innerException, resourceName, arg0);
    }

    internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName, object arg0, object arg1)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowArgument(innerException, resourceName, arg0, arg1);
    }

    internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName, object arg0, object arg1, object arg2)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowArgument(innerException, resourceName, arg0, arg1, arg2);
    }

    internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName, object arg0, object arg1, object arg2, object arg3)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowArgument(innerException, resourceName, arg0, arg1, arg2, arg3);
    }

    internal static void ThrowArgumentOutOfRange(string parameterName)
    {
      if (ErrorUtilities.throwExceptions)
        throw new ArgumentOutOfRangeException(parameterName);
    }

    internal static void VerifyThrowArgumentOutOfRange(bool condition, string parameterName)
    {
      if (condition)
        return;
      ErrorUtilities.ThrowArgumentOutOfRange(parameterName);
    }

    internal static void VerifyThrowArgumentLength(string parameter, string parameterName)
    {
      ErrorUtilities.VerifyThrowArgumentNull((object) parameter, parameterName);
      if (parameter.Length == 0 && ErrorUtilities.throwExceptions)
        throw new ArgumentException(ResourceUtilities.FormatResourceString("Shared.ParameterCannotHaveZeroLength", (object) parameterName));
    }

    internal static void VerifyThrowArgumentLengthIfNotNull(string parameter, string parameterName)
    {
      if (parameter != null && parameter.Length == 0 && ErrorUtilities.throwExceptions)
        throw new ArgumentException(ResourceUtilities.FormatResourceString("Shared.ParameterCannotHaveZeroLength", (object) parameterName));
    }

    internal static void VerifyThrowArgumentNull(object parameter, string parameterName)
    {
      ErrorUtilities.VerifyThrowArgumentNull(parameter, parameterName, "Shared.ParameterCannotBeNull");
    }

    internal static void VerifyThrowArgumentNull(object parameter, string parameterName, string resourceName)
    {
      if (parameter == null && ErrorUtilities.throwExceptions)
        throw new ArgumentNullException(ResourceUtilities.FormatResourceString(resourceName, (object) parameterName), (Exception) null);
    }

    internal static void VerifyThrowArgumentArraysSameLength(Array parameter1, Array parameter2, string parameter1Name, string parameter2Name)
    {
      ErrorUtilities.VerifyThrowArgumentNull((object) parameter1, parameter1Name);
      ErrorUtilities.VerifyThrowArgumentNull((object) parameter2, parameter2Name);
      if (parameter1.Length != parameter2.Length && ErrorUtilities.throwExceptions)
        throw new ArgumentException(ResourceUtilities.FormatResourceString("Shared.ParametersMustHaveTheSameLength", (object) parameter1Name, (object) parameter2Name));
    }
  }
}
