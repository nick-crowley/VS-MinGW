// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.MingRC
// Assembly: s-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 36D7C1E9-A694-4A78-9DA4-BFB01CA4D38F

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace Microsoft.Build.CPPTasks
{
  public class MingRC : TrackedVCToolTask
  {
    private ArrayList switchOrderList;

    protected override string ToolName
    {
      get
      {
        return "windres.exe";
      }
    }

    public virtual string[] PreprocessorDefinitions
    {
      get
      {
        if (this.IsPropertySet("PreprocessorDefinitions"))
          return this.ActiveToolSwitches["PreprocessorDefinitions"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("PreprocessorDefinitions");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
        switchToAdd.DisplayName = "Preprocessor Definitions";
        switchToAdd.Description = "Specifies one or more defines for the resource compiler. (-D [macro])";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-D ";
        switchToAdd.Name = "PreprocessorDefinitions";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("PreprocessorDefinitions", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] DesigntimePreprocessorDefinitions
    {
      get
      {
        if (this.IsPropertySet("DesigntimePreprocessorDefinitions"))
          return this.ActiveToolSwitches["DesigntimePreprocessorDefinitions"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("DesigntimePreprocessorDefinitions");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
        switchToAdd.DisplayName = "Designtime Preprocessor Definitions";
        switchToAdd.Description = "Resource editor defines.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "DesigntimePreprocessorDefinitions";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("DesigntimePreprocessorDefinitions", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] UndefinePreprocessorDefinitions
    {
      get
      {
        if (this.IsPropertySet("UndefinePreprocessorDefinitions"))
          return this.ActiveToolSwitches["UndefinePreprocessorDefinitions"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("UndefinePreprocessorDefinitions");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
        switchToAdd.DisplayName = "Undefine Preprocessor Definitions";
        switchToAdd.Description = "Undefine a symbol. (-U)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-U ";
        switchToAdd.Name = "UndefinePreprocessorDefinitions";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("UndefinePreprocessorDefinitions", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string Culture
    {
      get
      {
        if (this.IsPropertySet("Culture"))
          return this.ActiveToolSwitches["Culture"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Culture");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Culture";
        switchToAdd.Description = "Lists the culture (such as US English or Italian) used in the resources. (-l [num])";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "Culture";
        switchToAdd.Value = value;
        switchToAdd.SwitchValue = "-l ";
        this.ActiveToolSwitches.Add("Culture", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] AdditionalIncludeDirectories
    {
      get
      {
        if (this.IsPropertySet("AdditionalIncludeDirectories"))
          return this.ActiveToolSwitches["AdditionalIncludeDirectories"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("AdditionalIncludeDirectories");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Additional Include Directories";
        switchToAdd.Description = "Specifies one or more directories to add to the include path; use semi-colon delimiter if more than one. (-I [path])";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-I ";
        switchToAdd.Name = "AdditionalIncludeDirectories";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("AdditionalIncludeDirectories", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool IgnoreStandardIncludePath
    {
      get
      {
        if (this.IsPropertySet("IgnoreStandardIncludePath"))
          return this.ActiveToolSwitches["IgnoreStandardIncludePath"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("IgnoreStandardIncludePath");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Ignore Standard Include Paths";
        switchToAdd.Description = "Prevents the resource compiler from searching for include files in directories specified in the INCLUDE environment variables. (/X)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "/X ";
        switchToAdd.Name = "IgnoreStandardIncludePath";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("IgnoreStandardIncludePath", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool ShowProgress
    {
      get
      {
        if (this.IsPropertySet("ShowProgress"))
          return this.ActiveToolSwitches["ShowProgress"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ShowProgress");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Show Progress";
        switchToAdd.Description = "Send progress messages to output window. (-v)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-v";
        switchToAdd.Name = "ShowProgress";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("ShowProgress", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool SuppressStartupBanner
    {
      get
      {
        if (this.IsPropertySet("SuppressStartupBanner"))
          return this.ActiveToolSwitches["SuppressStartupBanner"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("SuppressStartupBanner");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Suppress Startup Banner";
        switchToAdd.Description = "Suppress the display of version information message (-V)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-V";
        switchToAdd.Name = "SuppressStartupBanner";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("SuppressStartupBanner", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string ResourceOutputFileName
    {
      get
      {
        if (this.IsPropertySet("ResourceOutputFileName"))
          return this.ActiveToolSwitches["ResourceOutputFileName"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ResourceOutputFileName");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.File);
        switchToAdd.DisplayName = "Resource File Name";
        switchToAdd.Description = "Specifies the name of the resource file (-o [file])";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-o ";
        switchToAdd.Name = "ResourceOutputFileName";
        switchToAdd.Value = value;
        this.ActiveToolSwitches.Add("ResourceOutputFileName", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }
    

    [Required]
    public virtual ITaskItem Source
    {
      get
      {
        if (this.IsPropertySet("Source"))
          return this.ActiveToolSwitches["Source"].TaskItem;
        return (ITaskItem) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Source");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.ITaskItem);
        switchToAdd.DisplayName = "Source";
        switchToAdd.Description = "RC input file (-i [file])";
        switchToAdd.Required = true;
        switchToAdd.SwitchValue = "-i ";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.TaskItem = value;
        this.ActiveToolSwitches.Add("Source", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string TrackerLogDirectory
    {
      get
      {
        if (this.IsPropertySet("TrackerLogDirectory"))
          return this.ActiveToolSwitches["TrackerLogDirectory"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("TrackerLogDirectory");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Directory);
        switchToAdd.DisplayName = "Tracker Log Directory";
        switchToAdd.Description = "Tracker Log Directory.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Value = VCToolTask.EnsureTrailingSlash(value);
        this.ActiveToolSwitches.Add("TrackerLogDirectory", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    protected override ArrayList SwitchOrderList
    {
      get
      {
        return this.switchOrderList;
      }
    }

    protected override string[] ReadTLogNames
    {
      get
      {
        return new string[1]
        {
          "rc.read.1.tlog"
        };
      }
    }

    protected override string[] WriteTLogNames
    {
      get
      {
        return new string[1]
        {
          "rc.write.1.tlog"
        };
      }
    }

    protected override string CommandTLogName
    {
      get
      {
        return "rc.command.1.tlog";
      }
    }

    protected override string TrackerIntermediateDirectory
    {
      get
      {
        if (this.TrackerLogDirectory != null)
          return this.TrackerLogDirectory;
        return string.Empty;
      }
    }

    public override string SourcesPropertyName
    {
      get
      {
        return "Source";
      }
    }

    protected override ITaskItem[] TrackedInputFiles
    {
      get
      {
        return new ITaskItem[1]
        {
          this.Source
        };
      }
    }

    public MingRC()
      : base(new ResourceManager("Microsoft.Build.CPPTasks.Strings", Assembly.GetExecutingAssembly()))
    {
      this.switchOrderList = new ArrayList();
      this.switchOrderList.Add((object) "PreprocessorDefinitions");
      this.switchOrderList.Add((object) "DesigntimePreprocessorDefinitions");
      this.switchOrderList.Add((object) "UndefinePreprocessorDefinitions");
      this.switchOrderList.Add((object) "Culture");
      this.switchOrderList.Add((object) "AdditionalIncludeDirectories");
      this.switchOrderList.Add((object) "IgnoreStandardIncludePath");
      this.switchOrderList.Add((object) "ShowProgress");
      this.switchOrderList.Add((object) "SuppressStartupBanner");
      this.switchOrderList.Add((object) "AdditionalOptions");
      this.switchOrderList.Add((object) "TrackerLogDirectory");
      this.switchOrderList.Add((object)"Source");
      this.switchOrderList.Add((object)"ResourceOutputFileName");
    }

    protected override string GenerateResponseFileCommandsExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      return string.Empty;
    }

    protected override string GenerateCommandLineCommandsExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.Default)
    {
      CommandLineBuilder commandLineBuilder = new CommandLineBuilder();
      string str = base.GenerateResponseFileCommandsExceptSwitches(switchesToRemove, format, escapeFormat);
      if (!this.TrackFileAccess)
        str = str.Replace("\\\"", "\"").Replace("\\\\\"", "\\\"");
      if (this.Source != null && !new List<string>((IEnumerable<string>) switchesToRemove).Contains(this.SourcesPropertyName))
      {
        if (format == VCToolTask.CommandLineFormat.ForTracking)
          commandLineBuilder.AppendFileNameIfNotNull(this.Source.ItemSpec.ToUpper());
        else
          commandLineBuilder.AppendFileNameIfNotNull(this.Source.ItemSpec);
      }
      return str + " " + commandLineBuilder.ToString();
    }
  }
}
