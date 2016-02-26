// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Shared.AssemblyResources
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Microsoft.Build.Shared
{
  internal static class AssemblyResources
  {
    private static readonly ResourceManager resources = new ResourceManager("Microsoft.Build.CPPTasks.Strings", Assembly.GetExecutingAssembly());
    private static readonly ResourceManager sharedResources = new ResourceManager("Microsoft.Build.CPPTasks.Strings.shared", Assembly.GetExecutingAssembly());

    internal static ResourceManager PrimaryResources
    {
      get
      {
        return AssemblyResources.resources;
      }
    }

    internal static ResourceManager SharedResources
    {
      get
      {
        return AssemblyResources.sharedResources;
      }
    }

    internal static string GetString(string name)
    {
      return AssemblyResources.resources.GetString(name, CultureInfo.CurrentUICulture) ?? AssemblyResources.sharedResources.GetString(name, CultureInfo.CurrentUICulture);
    }

    internal static string FormatString(string unformatted, params object[] args)
    {
      ErrorUtilities.VerifyThrowArgumentNull((object) unformatted, "unformatted");
      return ResourceUtilities.FormatString(unformatted, args);
    }

    internal static string FormatResourceString(string resourceName, params object[] args)
    {
      ErrorUtilities.VerifyThrowArgumentNull((object) resourceName, "resourceName");
      return AssemblyResources.FormatString(AssemblyResources.GetString(resourceName), args);
    }
  }
}
