// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Shared.ExceptionHandling
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Schema;

namespace Microsoft.Build.Shared
{
  internal static class ExceptionHandling
  {
    private static string dumpFileName;

    internal static bool IsCriticalException(Exception e)
    {
      return e is StackOverflowException || e is OutOfMemoryException || (e is ThreadAbortException || e is ThreadInterruptedException) || (e is InternalErrorException || e is AccessViolationException);
    }

    internal static bool NotExpectedException(Exception e)
    {
      return !(e is UnauthorizedAccessException) && !(e is NotSupportedException) && (!(e is ArgumentException) || e is ArgumentNullException) && (!(e is SecurityException) && !(e is IOException));
    }

    internal static bool IsIoRelatedException(Exception e)
    {
      return !ExceptionHandling.NotExpectedException(e);
    }

    internal static bool IsXmlException(Exception e)
    {
      if (!(e is XmlSyntaxException) && !(e is XmlException) && !(e is XmlSchemaException))
        return e is UriFormatException;
      return true;
    }

    internal static ExceptionHandling.LineAndColumn GetXmlLineAndColumn(Exception e)
    {
      int num1 = 0;
      int num2 = 0;
      XmlException xmlException = e as XmlException;
      if (xmlException != null)
      {
        num1 = xmlException.LineNumber;
        num2 = xmlException.LinePosition;
      }
      else
      {
        XmlSchemaException xmlSchemaException = e as XmlSchemaException;
        if (xmlSchemaException != null)
        {
          num1 = xmlSchemaException.LineNumber;
          num2 = xmlSchemaException.LinePosition;
        }
      }
      return new ExceptionHandling.LineAndColumn()
      {
        Line = num1,
        Column = num2
      };
    }

    internal static bool NotExpectedIoOrXmlException(Exception e)
    {
      return !ExceptionHandling.IsXmlException(e) && ExceptionHandling.NotExpectedException(e);
    }

    internal static bool NotExpectedReflectionException(Exception e)
    {
      return !(e is TypeLoadException) && !(e is MethodAccessException) && (!(e is MissingMethodException) && !(e is MemberAccessException)) && (!(e is BadImageFormatException) && !(e is ReflectionTypeLoadException) && (!(e is CustomAttributeFormatException) && !(e is TargetParameterCountException))) && (!(e is InvalidCastException) && !(e is AmbiguousMatchException) && (!(e is InvalidFilterCriteriaException) && !(e is TargetException)) && (!(e is MissingFieldException) && ExceptionHandling.NotExpectedException(e)));
    }

    internal static bool NotExpectedSerializationException(Exception e)
    {
      return !(e is SerializationException) && ExceptionHandling.NotExpectedReflectionException(e);
    }

    internal static bool NotExpectedRegistryException(Exception e)
    {
      return !(e is SecurityException) && !(e is UnauthorizedAccessException) && (!(e is IOException) && !(e is ObjectDisposedException)) && !(e is ArgumentException);
    }

    internal static bool NotExpectedFunctionException(Exception e)
    {
      return !(e is InvalidCastException) && !(e is ArgumentNullException) && (!(e is FormatException) && !(e is InvalidOperationException)) && ExceptionHandling.NotExpectedReflectionException(e);
    }

    internal static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
      ExceptionHandling.DumpExceptionToFile((Exception) e.ExceptionObject);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static void DumpExceptionToFile(Exception ex)
    {
      if (ExceptionHandling.dumpFileName == null)
      {
        Guid guid = Guid.NewGuid();
        string tempPath = Path.GetTempPath();
        if (!Directory.Exists(tempPath))
          Directory.CreateDirectory(tempPath);
        ExceptionHandling.dumpFileName = Path.Combine(tempPath, "MSBuild_" + guid.ToString() + ".failure.txt");
        using (StreamWriter streamWriter = new StreamWriter(ExceptionHandling.dumpFileName, true))
        {
          streamWriter.WriteLine("UNHANDLED EXCEPTIONS FROM PROCESS {0}:", (object) Process.GetCurrentProcess().Id);
          streamWriter.WriteLine("=====================");
        }
      }
      using (StreamWriter streamWriter = new StreamWriter(ExceptionHandling.dumpFileName, true))
      {
        streamWriter.WriteLine(DateTime.Now.ToString("G", (IFormatProvider) CultureInfo.CurrentCulture));
        streamWriter.WriteLine(ex.ToString());
        streamWriter.WriteLine("===================");
      }
    }

    internal struct LineAndColumn
    {
      internal int Line { get; set; }

      internal int Column { get; set; }
    }
  }
}
