// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.ArgumentRelation
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


namespace Microsoft.Build.CPPTasks
{
  public class ArgumentRelation : PropertyRelation
  {
    public string Separator { get; set; }

    public ArgumentRelation(string argument, string value, bool required, string separator)
      : base(argument, value, required)
    {
      this.Separator = separator;
    }
  }
}
