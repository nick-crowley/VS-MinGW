// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Shared.FileUtilities
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using System;
using System.IO;

namespace Microsoft.Build.Shared
{
  internal static class FileUtilities
  {
    internal static string GetTemporaryFile()
    {
      return FileUtilities.GetTemporaryFile(".tmp");
    }

    internal static string GetTemporaryFile(string extension)
    {
      return FileUtilities.GetTemporaryFile((string) null, extension);
    }

    internal static string GetTemporaryFile(string directory, string extension)
    {
      ErrorUtilities.VerifyThrowArgumentLengthIfNotNull(directory, "directory");
      ErrorUtilities.VerifyThrowArgumentLength(extension, "extension");
      if ((int) extension[0] != 46)
        extension = "." + extension;
      string path;
      try
      {
        directory = directory ?? Path.GetTempPath();
        if (!Directory.Exists(directory))
          Directory.CreateDirectory(directory);
        path = Path.Combine(directory, "tmp" + Guid.NewGuid().ToString("N") + extension);
        ErrorUtilities.VerifyThrow(!File.Exists(path), "Guid should be unique");
        File.WriteAllText(path, string.Empty);
      }
      catch (Exception ex)
      {
        if (ExceptionHandling.NotExpectedException(ex))
          throw;
        else
          throw new IOException(ResourceUtilities.FormatResourceString("Shared.FailedCreatingTempFile", (object) ex.Message), ex);
      }
      return path;
    }
  }
}
