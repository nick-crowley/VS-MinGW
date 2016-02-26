// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.VCToolTask
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using Microsoft.Build.Framework;
using Microsoft.Build.Shared;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;

namespace Microsoft.Build.CPPTasks
{
  public abstract class VCToolTask : ToolTask
  {
    private Dictionary<string, ToolSwitch> activeToolSwitchesValues = new Dictionary<string, ToolSwitch>();
    private Dictionary<string, ToolSwitch> activeToolSwitches = new Dictionary<string, ToolSwitch>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private string additionalOptions = string.Empty;
    private char prefix = '/';
    private IntPtr cancelEvent;
    private string cancelEventName;
    private TaskLoggingHelper logPrivate;

    protected Dictionary<string, ToolSwitch> ActiveToolSwitches
    {
      get
      {
        return this.activeToolSwitches;
      }
    }

    public string AdditionalOptions
    {
      get
      {
        return this.additionalOptions;
      }
      set
      {
        this.additionalOptions = this.TranslateAdditionalOptions(value);
      }
    }

    protected override Encoding ResponseFileEncoding
    {
      get
      {
        return Encoding.Unicode;
      }
    }

    protected virtual ArrayList SwitchOrderList
    {
      get
      {
        return (ArrayList) null;
      }
    }

    protected string CancelEventName
    {
      get
      {
        return this.cancelEventName;
      }
    }

    protected TaskLoggingHelper LogPrivate
    {
      get
      {
        return this.logPrivate;
      }
    }

    protected override MessageImportance StandardOutputLoggingImportance
    {
      get
      {
        return MessageImportance.High;
      }
    }

    protected override MessageImportance StandardErrorLoggingImportance
    {
      get
      {
        return MessageImportance.High;
      }
    }

    protected virtual string AlwaysAppend
    {
      get
      {
        return string.Empty;
      }
      set
      {
      }
    }

    public virtual string[] AcceptableNonzeroExitCodes { get; set; }

    public Dictionary<string, ToolSwitch> ActiveToolSwitchesValues
    {
      get
      {
        return this.activeToolSwitchesValues;
      }
      set
      {
        this.activeToolSwitchesValues = value;
      }
    }

    public string EffectiveWorkingDirectory { get; set; }

    protected bool IgnoreUnknownSwitchValues { get; set; }

    protected VCToolTask(ResourceManager taskResources)
      : base(taskResources)
    {
      this.cancelEventName = "MSBuildConsole_CancelEvent" + Guid.NewGuid().ToString("N");
      this.cancelEvent = VCTaskNativeMethods.CreateEventW(IntPtr.Zero, false, false, this.cancelEventName);
      this.logPrivate = new TaskLoggingHelper((ITask) this);
      this.logPrivate.TaskResources = AssemblyResources.PrimaryResources;
      this.logPrivate.HelpKeywordPrefix = "MSBuild.";
      this.IgnoreUnknownSwitchValues = false;
    }

    protected virtual string TranslateAdditionalOptions(string options)
    {
      return options;
    }

    protected override string GetWorkingDirectory()
    {
      return this.EffectiveWorkingDirectory;
    }

    protected override string GenerateFullPathToTool()
    {
      return this.ToolName;
    }

    protected override bool ValidateParameters()
    {
      if (!this.logPrivate.HasLoggedErrors)
        return !this.Log.HasLoggedErrors;
      return false;
    }

    protected internal string GenerateCommandLine(VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      string str1 = this.GenerateCommandLineCommands(format, escapeFormat);
      string str2 = this.GenerateResponseFileCommands(format, escapeFormat);
      if (!string.IsNullOrEmpty(str1))
        return str1 + " " + str2;
      return str2;
    }

    protected string GenerateCommandLineExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      string str1 = this.GenerateCommandLineCommandsExceptSwitches(switchesToRemove, format, escapeFormat);
      string str2 = this.GenerateResponseFileCommandsExceptSwitches(switchesToRemove, format, escapeFormat);
      if (!string.IsNullOrEmpty(str1))
        return str1 + " " + str2;
      return str2;
    }

    protected virtual string GenerateCommandLineCommandsExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      return string.Empty;
    }

    protected override string GenerateResponseFileCommands()
    {
      return this.GenerateResponseFileCommands(VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat.Default);
    }

    protected virtual string GenerateResponseFileCommands(VCToolTask.CommandLineFormat format, VCToolTask.EscapeFormat escapeFormat)
    {
      return this.GenerateResponseFileCommandsExceptSwitches(new string[0], format, escapeFormat);
    }

    protected override string GenerateCommandLineCommands()
    {
      return this.GenerateCommandLineCommands(VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat.Default);
    }

    protected virtual string GenerateCommandLineCommands(VCToolTask.CommandLineFormat format, VCToolTask.EscapeFormat escapeFormat)
    {
      return this.GenerateCommandLineCommandsExceptSwitches(new string[0], format, escapeFormat);
    }

    protected virtual string GenerateResponseFileCommandsExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      bool flag1 = false;
      this.AddDefaultsToActiveSwitchList();
      this.AddFallbacksToActiveSwitchList();
      this.PostProcessSwitchList();
      CommandLineBuilder commandLineBuilder = new CommandLineBuilder(true);
      foreach (string index in this.SwitchOrderList)
      {
        if (this.IsPropertySet(index))
        {
          ToolSwitch toolSwitch = this.activeToolSwitches[index];
          if (this.VerifyDependenciesArePresent(toolSwitch) && this.VerifyRequiredArgumentsArePresent(toolSwitch, false))
          {
            bool flag2 = true;
            if (switchesToRemove != null)
            {
              foreach (string str in switchesToRemove)
              {
                if (index.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                  flag2 = false;
                  break;
                }
              }
            }
            if (flag2)
              this.GenerateCommandsAccordingToType(commandLineBuilder, toolSwitch, false, format, escapeFormat);
          }
        }
        else if (string.Equals(index, "additionaloptions", StringComparison.OrdinalIgnoreCase))
        {
          this.BuildAdditionalArgs(commandLineBuilder);
          flag1 = true;
        }
        else if (string.Equals(index, "AlwaysAppend", StringComparison.OrdinalIgnoreCase))
          commandLineBuilder.AppendSwitch(this.AlwaysAppend);
      }
      if (!flag1)
        this.BuildAdditionalArgs(commandLineBuilder);
      return commandLineBuilder.ToString();
    }

    protected override bool HandleTaskExecutionErrors()
    {
      if (this.IsAcceptableReturnValue())
        return true;
      return base.HandleTaskExecutionErrors();
    }

    public override bool Execute()
    {
      int num = base.Execute() ? 1 : 0;
      VCTaskNativeMethods.CloseHandle(this.cancelEvent);
      return num != 0;
    }

    public override void Cancel()
    {
      VCTaskNativeMethods.SetEvent(this.cancelEvent);
    }

    protected bool VerifyRequiredArgumentsArePresent(ToolSwitch property, bool throwOnError)
    {
      if (property.ArgumentRelationList != null)
      {
        foreach (ArgumentRelation argumentRelation in property.ArgumentRelationList)
        {
          if (argumentRelation.Required && (property.Value == argumentRelation.Value || argumentRelation.Value == string.Empty) && !this.HasSwitch(argumentRelation.Argument))
          {
            string message;
            if (string.Empty == argumentRelation.Value)
              message = this.Log.FormatResourceString("MissingRequiredArgument", (object) argumentRelation.Argument, (object) property.Name);
            else
              message = this.Log.FormatResourceString("MissingRequiredArgumentWithValue", (object) argumentRelation.Argument, (object) property.Name, (object) argumentRelation.Value);
            this.Log.LogError(message);
            if (throwOnError)
              throw new LoggerException(message);
            return false;
          }
        }
      }
      return true;
    }

    protected bool IsAcceptableReturnValue()
    {
      if (this.AcceptableNonzeroExitCodes != null)
      {
        foreach (string str in this.AcceptableNonzeroExitCodes)
        {
          if (this.ExitCode == Convert.ToInt32(str, (IFormatProvider) CultureInfo.InvariantCulture))
            return true;
        }
      }
      return false;
    }

    protected void RemoveSwitchToolBasedOnValue(string switchValue)
    {
      if (this.ActiveToolSwitchesValues.Count <= 0 || !this.ActiveToolSwitchesValues.ContainsKey("/" + switchValue))
        return;
      ToolSwitch toolSwitch = this.ActiveToolSwitchesValues["/" + switchValue];
      if (toolSwitch == null)
        return;
      this.ActiveToolSwitches.Remove(toolSwitch.Name);
    }

    protected void AddActiveSwitchToolValue(ToolSwitch switchToAdd)
    {
      if (switchToAdd.Type != ToolSwitchType.Boolean || switchToAdd.BooleanValue)
      {
        if (!(switchToAdd.SwitchValue != string.Empty))
          return;
        this.ActiveToolSwitchesValues.Add(switchToAdd.SwitchValue, switchToAdd);
      }
      else
      {
        if (!(switchToAdd.ReverseSwitchValue != string.Empty))
          return;
        this.ActiveToolSwitchesValues.Add(switchToAdd.ReverseSwitchValue, switchToAdd);
      }
    }

    protected string GetEffectiveArgumentsValues(ToolSwitch property)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      string str = string.Empty;
      if (property.ArgumentRelationList != null)
      {
        foreach (ArgumentRelation argumentRelation in property.ArgumentRelationList)
        {
          if (str != string.Empty && str != argumentRelation.Argument)
            flag = true;
          str = argumentRelation.Argument;
          if ((property.Value == argumentRelation.Value || argumentRelation.Value == string.Empty || property.Type == ToolSwitchType.Boolean && property.BooleanValue) && this.HasSwitch(argumentRelation.Argument))
          {
            ToolSwitch toolSwitch = this.ActiveToolSwitches[argumentRelation.Argument];
            stringBuilder.Append(argumentRelation.Separator);
            CommandLineBuilder builder = new CommandLineBuilder();
            this.GenerateCommandsAccordingToType(builder, toolSwitch, true, VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat.Default);
            stringBuilder.Append(builder.ToString());
          }
        }
      }
      CommandLineBuilder commandLineBuilder = new CommandLineBuilder();
      if (flag)
        commandLineBuilder.AppendSwitchIfNotNull("", stringBuilder.ToString());
      else
        commandLineBuilder.AppendSwitchUnquotedIfNotNull("", stringBuilder.ToString());
      return commandLineBuilder.ToString();
    }

    protected virtual void PostProcessSwitchList()
    {
      this.ValidateRelations();
      this.ValidateOverrides();
    }

    protected virtual void ValidateRelations()
    {
    }

    protected virtual void ValidateOverrides()
    {
      List<string> list = new List<string>();
      foreach (KeyValuePair<string, ToolSwitch> keyValuePair1 in this.ActiveToolSwitches)
      {
        foreach (KeyValuePair<string, string> keyValuePair2 in keyValuePair1.Value.Overrides)
        {
          string key = keyValuePair2.Key;
          string b;
          if (keyValuePair1.Value.Type != ToolSwitchType.Boolean || keyValuePair1.Value.BooleanValue)
            b = keyValuePair1.Value.SwitchValue.TrimStart('/');
          else
            b = keyValuePair1.Value.ReverseSwitchValue.TrimStart('/');
          int num = 5;
          if (string.Equals(key, b, (StringComparison) num))
          {
            foreach (KeyValuePair<string, ToolSwitch> keyValuePair3 in this.ActiveToolSwitches)
            {
              if (!string.Equals(keyValuePair3.Key, keyValuePair1.Key, StringComparison.OrdinalIgnoreCase))
              {
                if (string.Equals(keyValuePair3.Value.SwitchValue.TrimStart('/'), keyValuePair2.Value, StringComparison.OrdinalIgnoreCase))
                {
                  list.Add(keyValuePair3.Key);
                  break;
                }
                if (keyValuePair3.Value.Type == ToolSwitchType.Boolean && !keyValuePair3.Value.BooleanValue)
                {
                  if (string.Equals(keyValuePair3.Value.ReverseSwitchValue.TrimStart('/'), keyValuePair2.Value, StringComparison.OrdinalIgnoreCase))
                  {
                    list.Add(keyValuePair3.Key);
                    break;
                  }
                }
              }
            }
          }
        }
      }
      foreach (string key in list)
        this.ActiveToolSwitches.Remove(key);
    }

    protected bool IsSwitchValueSet(string switchValue)
    {
      if (!string.IsNullOrEmpty(switchValue))
        return this.ActiveToolSwitchesValues.ContainsKey("/" + switchValue);
      return false;
    }

    protected virtual bool VerifyDependenciesArePresent(ToolSwitch value)
    {
      if (value.Parents.Count <= 0)
        return true;
      bool flag = false;
      foreach (string propertyName in value.Parents)
        flag = flag || this.HasSwitch(propertyName);
      return flag;
    }

    protected virtual void AddDefaultsToActiveSwitchList()
    {
    }

    protected virtual void AddFallbacksToActiveSwitchList()
    {
    }

    protected void GenerateCommandsAccordingToType(CommandLineBuilder builder, ToolSwitch toolSwitch, bool recursive, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      if (toolSwitch.Parents.Count > 0 && !recursive)
        return;
      switch (toolSwitch.Type)
      {
        case ToolSwitchType.Boolean:
          this.EmitBooleanSwitch(builder, toolSwitch);
          break;
        case ToolSwitchType.Integer:
          this.EmitIntegerSwitch(builder, toolSwitch);
          break;
        case ToolSwitchType.String:
          this.EmitStringSwitch(builder, toolSwitch);
          break;
        case ToolSwitchType.StringArray:
          VCToolTask.EmitStringArraySwitch(builder, toolSwitch, VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat.Default);
          break;
        case ToolSwitchType.File:
          VCToolTask.EmitFileSwitch(builder, toolSwitch, format);
          break;
        case ToolSwitchType.Directory:
          VCToolTask.EmitDirectorySwitch(builder, toolSwitch, format);
          break;
        case ToolSwitchType.ITaskItem:
          VCToolTask.EmitTaskItemSwitch(builder, toolSwitch);
          break;
        case ToolSwitchType.ITaskItemArray:
          VCToolTask.EmitTaskItemArraySwitch(builder, toolSwitch, format);
          break;
        case ToolSwitchType.AlwaysAppend:
          VCToolTask.EmitAlwaysAppendSwitch(builder, toolSwitch);
          break;
        case ToolSwitchType.StringPathArray:
          VCToolTask.EmitStringArraySwitch(builder, toolSwitch, format, escapeFormat);
          break;
        default:
          ErrorUtilities.VerifyThrow(false, "InternalError");
          break;
      }
    }

    protected void BuildAdditionalArgs(CommandLineBuilder cmdLine)
    {
      if (cmdLine == null || string.IsNullOrEmpty(this.additionalOptions))
        return;
      cmdLine.AppendSwitch(Environment.ExpandEnvironmentVariables(this.additionalOptions));
    }

    protected bool ValidateInteger(string switchName, int min, int max, int value)
    {
      if (value >= min && value <= max)
        return true;
      this.logPrivate.LogErrorFromResources("ArgumentOutOfRange", (object) switchName, (object) value);
      return false;
    }

    protected string ReadSwitchMap(string propertyName, string[][] switchMap, string value)
    {
      if (switchMap != null)
      {
        for (int index = 0; index < switchMap.Length; ++index)
        {
          if (string.Equals(switchMap[index][0], value, StringComparison.CurrentCultureIgnoreCase))
            return switchMap[index][1];
        }
        if (!this.IgnoreUnknownSwitchValues)
          this.logPrivate.LogErrorFromResources("ArgumentOutOfRange", (object) propertyName, (object) value);
      }
      return string.Empty;
    }

    protected bool IsPropertySet(string propertyName)
    {
      if (!string.IsNullOrEmpty(propertyName))
        return this.activeToolSwitches.ContainsKey(propertyName);
      return false;
    }

    protected bool IsSetToTrue(string propertyName)
    {
      if (this.activeToolSwitches.ContainsKey(propertyName))
        return this.activeToolSwitches[propertyName].BooleanValue;
      return false;
    }

    protected bool IsExplicitlySetToFalse(string propertyName)
    {
      if (this.activeToolSwitches.ContainsKey(propertyName))
        return !this.activeToolSwitches[propertyName].BooleanValue;
      return false;
    }

    protected bool HasSwitch(string propertyName)
    {
      if (this.IsPropertySet(propertyName))
        return !string.IsNullOrEmpty(this.activeToolSwitches[propertyName].Name);
      return false;
    }

    protected static string EnsureTrailingSlash(string directoryName)
    {
      ErrorUtilities.VerifyThrow(directoryName != null, "InternalError");
      if (!string.IsNullOrEmpty(directoryName))
      {
        string str = directoryName;
        int index = str.Length - 1;
        char ch = str[index];
        if ((int) ch != (int) Path.DirectorySeparatorChar && (int) ch != (int) Path.AltDirectorySeparatorChar)
          directoryName += Path.DirectorySeparatorChar.ToString();
      }
      return directoryName;
    }

    private static void EmitAlwaysAppendSwitch(CommandLineBuilder builder, ToolSwitch toolSwitch)
    {
      builder.AppendSwitch(toolSwitch.Name);
    }

    private static void EmitTaskItemArraySwitch(CommandLineBuilder builder, ToolSwitch toolSwitch, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog)
    {
      if (string.IsNullOrEmpty(toolSwitch.Separator))
      {
        foreach (ITaskItem taskItem in toolSwitch.TaskItemArray)
          builder.AppendSwitchIfNotNull(toolSwitch.SwitchValue, Environment.ExpandEnvironmentVariables(taskItem.ItemSpec));
      }
      else
      {
        ITaskItem[] parameters = new ITaskItem[toolSwitch.TaskItemArray.Length];
        for (int index = 0; index < toolSwitch.TaskItemArray.Length; ++index)
        {
          parameters[index] = (ITaskItem) new TaskItem(Environment.ExpandEnvironmentVariables(toolSwitch.TaskItemArray[index].ItemSpec));
          if (format == VCToolTask.CommandLineFormat.ForTracking)
            parameters[index].ItemSpec = parameters[index].ItemSpec.ToUpperInvariant();
        }
        builder.AppendSwitchIfNotNull(toolSwitch.SwitchValue, parameters, toolSwitch.Separator);
      }
    }

    private static void EmitTaskItemSwitch(CommandLineBuilder builder, ToolSwitch toolSwitch)
    {
      if (string.IsNullOrEmpty(toolSwitch.Name))
        return;
      builder.AppendSwitch(Environment.ExpandEnvironmentVariables(toolSwitch.Name + toolSwitch.Separator));
    }

    private static void EmitDirectorySwitch(CommandLineBuilder builder, ToolSwitch toolSwitch, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog)
    {
      if (string.IsNullOrEmpty(toolSwitch.SwitchValue))
        return;
      if (format == VCToolTask.CommandLineFormat.ForBuildLog)
        builder.AppendSwitch(toolSwitch.SwitchValue + toolSwitch.Separator);
      else
        builder.AppendSwitch(toolSwitch.SwitchValue.ToUpperInvariant() + toolSwitch.Separator);
    }

    private static void EmitFileSwitch(CommandLineBuilder builder, ToolSwitch toolSwitch, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog)
    {
      if (string.IsNullOrEmpty(toolSwitch.Value))
        return;
      string parameter = Environment.ExpandEnvironmentVariables(toolSwitch.Value).Trim();
      if (format == VCToolTask.CommandLineFormat.ForTracking)
        parameter = parameter.ToUpperInvariant();
      if (!parameter.StartsWith("\"", StringComparison.Ordinal))
      {
        string str = "\"" + parameter;
        parameter = !str.EndsWith("\\", StringComparison.Ordinal) || str.EndsWith("\\\\", StringComparison.Ordinal) ? str + "\"" : str + "\\\"";
      }
      builder.AppendSwitchUnquotedIfNotNull(toolSwitch.SwitchValue + toolSwitch.Separator, parameter);
    }

    private void EmitIntegerSwitch(CommandLineBuilder builder, ToolSwitch toolSwitch)
    {
      if (!toolSwitch.IsValid)
        return;
      if (!string.IsNullOrEmpty(toolSwitch.Separator))
        builder.AppendSwitch(toolSwitch.SwitchValue + toolSwitch.Separator + toolSwitch.Number.ToString((IFormatProvider) CultureInfo.InvariantCulture) + this.GetEffectiveArgumentsValues(toolSwitch));
      else
        builder.AppendSwitch(toolSwitch.SwitchValue + toolSwitch.Number.ToString((IFormatProvider) CultureInfo.InvariantCulture) + this.GetEffectiveArgumentsValues(toolSwitch));
    }

    private static void EmitStringArraySwitch(CommandLineBuilder builder, ToolSwitch toolSwitch, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      string[] parameters = new string[toolSwitch.StringList.Length];
      for (int index = 0; index < toolSwitch.StringList.Length; ++index)
      {
        string str = !toolSwitch.StringList[index].StartsWith("\"", StringComparison.Ordinal) || !toolSwitch.StringList[index].EndsWith("\"", StringComparison.Ordinal) ? Environment.ExpandEnvironmentVariables(toolSwitch.StringList[index]) : Environment.ExpandEnvironmentVariables(toolSwitch.StringList[index].Substring(1, toolSwitch.StringList[index].Length - 2));
        if (!string.IsNullOrEmpty(str))
        {
          if (format == VCToolTask.CommandLineFormat.ForTracking)
            str = str.ToUpperInvariant();
          if (escapeFormat.HasFlag((Enum) VCToolTask.EscapeFormat.EscapeTrailingSlash) && !str.Contains(" ") && (str.EndsWith("\\", StringComparison.Ordinal) && !str.EndsWith("\\\\", StringComparison.Ordinal)))
            str += "\\";
          parameters[index] = str;
        }
      }
      if (string.IsNullOrEmpty(toolSwitch.Separator))
      {
        foreach (string parameter in parameters)
          builder.AppendSwitchIfNotNull(toolSwitch.SwitchValue, parameter);
      }
      else
        builder.AppendSwitchIfNotNull(toolSwitch.SwitchValue, parameters, toolSwitch.Separator);
    }

    private void EmitStringSwitch(CommandLineBuilder builder, ToolSwitch toolSwitch)
    {
      string switchName = string.Empty + toolSwitch.SwitchValue + toolSwitch.Separator;
      StringBuilder stringBuilder = new StringBuilder(this.GetEffectiveArgumentsValues(toolSwitch));
      string str1 = toolSwitch.Value;
      if (!toolSwitch.MultipleValues)
      {
        string str2 = str1.Trim();
        if (!str2.StartsWith("\"", StringComparison.Ordinal))
        {
          string str3 = "\"" + str2;
          str2 = !str3.EndsWith("\\", StringComparison.Ordinal) || str3.EndsWith("\\\\", StringComparison.Ordinal) ? str3 + "\"" : str3 + "\\\"";
        }
        stringBuilder.Insert(0, str2);
      }
      if (switchName.Length == 0 && stringBuilder.ToString().Length == 0)
        return;
      builder.AppendSwitchUnquotedIfNotNull(switchName, stringBuilder.ToString());
    }

    private void EmitBooleanSwitch(CommandLineBuilder builder, ToolSwitch toolSwitch)
    {
      if (toolSwitch.BooleanValue)
      {
        if (string.IsNullOrEmpty(toolSwitch.SwitchValue))
          return;
        StringBuilder stringBuilder = new StringBuilder(this.GetEffectiveArgumentsValues(toolSwitch));
        stringBuilder.Insert(0, toolSwitch.Separator);
        stringBuilder.Insert(0, toolSwitch.TrueSuffix);
        stringBuilder.Insert(0, toolSwitch.SwitchValue);
        builder.AppendSwitch(stringBuilder.ToString());
      }
      else
        this.EmitReversibleBooleanSwitch(builder, toolSwitch);
    }

    private void EmitReversibleBooleanSwitch(CommandLineBuilder builder, ToolSwitch toolSwitch)
    {
      if (string.IsNullOrEmpty(toolSwitch.ReverseSwitchValue))
        return;
      string str = toolSwitch.BooleanValue ? toolSwitch.TrueSuffix : toolSwitch.FalseSuffix;
      StringBuilder stringBuilder = new StringBuilder(this.GetEffectiveArgumentsValues(toolSwitch));
      stringBuilder.Insert(0, str);
      stringBuilder.Insert(0, toolSwitch.Separator);
      stringBuilder.Insert(0, toolSwitch.TrueSuffix);
      stringBuilder.Insert(0, toolSwitch.ReverseSwitchValue);
      builder.AppendSwitch(stringBuilder.ToString());
    }

    private string Prefix(string toolSwitch)
    {
      if (!string.IsNullOrEmpty(toolSwitch) && (int) toolSwitch[0] != (int) this.prefix)
        return this.prefix.ToString() + toolSwitch;
      return toolSwitch;
    }

    public enum CommandLineFormat
    {
      ForBuildLog,
      ForTracking,
    }

    [Flags]
    public enum EscapeFormat
    {
      Default = 0,
      EscapeTrailingSlash = 1,
    }
  }
}
