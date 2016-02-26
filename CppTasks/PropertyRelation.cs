// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.PropertyRelation
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


namespace Microsoft.Build.CPPTasks
{
  public class PropertyRelation
  {
    public string Argument { get; set; }

    public string Value { get; set; }

    public bool Required { get; set; }

    public PropertyRelation()
    {
    }

    public PropertyRelation(string argument, string value, bool required)
    {
      this.Argument = argument;
      this.Value = value;
      this.Required = required;
    }
  }
}
