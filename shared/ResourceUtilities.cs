// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Shared.ResourceUtilities
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using System;
using System.Globalization;

namespace Microsoft.Build.Shared
{
  internal static class ResourceUtilities
  {
    internal static string ExtractMessageCode(bool msbuildCodeOnly, string message, out string code)
    {
      ErrorUtilities.VerifyThrowInternalNull((object) message, "message");
      code = (string) null;
      int startIndex1 = 0;
      while (startIndex1 < message.Length && char.IsWhiteSpace(message[startIndex1]))
        ++startIndex1;
      int startIndex2;
      if (msbuildCodeOnly)
      {
        if (message.Length < startIndex1 + 8 || (int) message[startIndex1] != 77 || ((int) message[startIndex1 + 1] != 83 || (int) message[startIndex1 + 2] != 66) || ((int) message[startIndex1 + 3] < 48 || (int) message[startIndex1 + 3] > 57 || ((int) message[startIndex1 + 4] < 48 || (int) message[startIndex1 + 4] > 57)) || ((int) message[startIndex1 + 5] < 48 || (int) message[startIndex1 + 5] > 57 || ((int) message[startIndex1 + 6] < 48 || (int) message[startIndex1 + 6] > 57) || (int) message[startIndex1 + 7] != 58))
          return message;
        code = message.Substring(startIndex1, 7);
        startIndex2 = startIndex1 + 8;
      }
      else
      {
        int index1;
        for (index1 = startIndex1; index1 < message.Length; ++index1)
        {
          char ch = message[index1];
          if (((int) ch < 97 || (int) ch > 122) && ((int) ch < 65 || (int) ch > 90))
            break;
        }
        if (index1 == startIndex1)
          return message;
        int index2;
        for (index2 = index1; index2 < message.Length; ++index2)
        {
          char ch = message[index2];
          if ((int) ch < 48 || (int) ch > 57)
            break;
        }
        if (index2 == index1 || index2 == message.Length || (int) message[index2] != 58)
          return message;
        code = message.Substring(startIndex1, index2 - startIndex1);
        startIndex2 = index2 + 1;
      }
      while (startIndex2 < message.Length && char.IsWhiteSpace(message[startIndex2]))
        ++startIndex2;
      if (startIndex2 < message.Length)
        message = message.Substring(startIndex2, message.Length - startIndex2);
      return message;
    }

    private static string GetHelpKeyword(string resourceName)
    {
      return "MSBuild." + resourceName;
    }

    internal static string GetResourceString(string resourceName)
    {
      return AssemblyResources.GetString(resourceName);
    }

    internal static string FormatResourceString(out string code, out string helpKeyword, string resourceName, params object[] args)
    {
      helpKeyword = ResourceUtilities.GetHelpKeyword(resourceName);
      return ResourceUtilities.ExtractMessageCode(true, ResourceUtilities.FormatString(ResourceUtilities.GetResourceString(resourceName), args), out code);
    }

    internal static string FormatResourceString(string resourceName, params object[] args)
    {
      string code;
      string helpKeyword;
      return ResourceUtilities.FormatResourceString(out code, out helpKeyword, resourceName, args);
    }

    internal static string FormatString(string unformatted, params object[] args)
    {
      string str = unformatted;
      if (args != null && args.Length != 0)
        str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, unformatted, args);
      return str;
    }

    internal static void VerifyResourceStringExists(string resourceName)
    {
    }
  }
}
