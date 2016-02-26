// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.Archive
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using Microsoft.Build.Framework;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Microsoft.Build.CPPTasks
{
  public class Archive : TrackedVCToolTask
  {
    private ITaskItem[] preprocessOutput = new ITaskItem[0];
    private ArrayList switchOrderList;

    protected override string ToolName
    {
      get
      {
        return "ar.exe";
      }
    }

    public virtual string[] AdditionalDependencies
    {
      get
      {
        if (this.IsPropertySet("AdditionalDependencies"))
          return this.ActiveToolSwitches["AdditionalDependencies"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("AdditionalDependencies");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Additional Dependencies";
        switchToAdd.Description = "Specifies additional objects.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "AdditionalDependencies";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("AdditionalDependencies", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] AdditionalLibraryDirectories
    {
      get
      {
        if (this.IsPropertySet("AdditionalLibraryDirectories"))
          return this.ActiveToolSwitches["AdditionalLibraryDirectories"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("AdditionalLibraryDirectories");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Additional Library Directories";
        switchToAdd.Description = "Additional search directories to location objects.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "AdditionalLibraryDirectories";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("AdditionalLibraryDirectories", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool LinkLibraryDependencies
    {
      get
      {
        if (this.IsPropertySet("LinkLibraryDependencies"))
          return this.ActiveToolSwitches["LinkLibraryDependencies"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("LinkLibraryDependencies");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Link Library Dependencies";
        switchToAdd.Description = "Specifies whether or not library outputs from project dependencies are automatically linked in.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "LinkLibraryDependencies";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("LinkLibraryDependencies", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string Command
    {
      get
      {
        if (this.IsPropertySet("Command"))
          return this.ActiveToolSwitches["Command"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Command");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Command";
        switchToAdd.Description = "Command for AR.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.ArgumentRelationList.Add((object) new ArgumentRelation("CreateIndex", "", false, ""));
        switchToAdd.ArgumentRelationList.Add((object) new ArgumentRelation("CreateThinArchive", "", false, ""));
        switchToAdd.ArgumentRelationList.Add((object) new ArgumentRelation("NoWarnOnCreate", "", false, ""));
        switchToAdd.ArgumentRelationList.Add((object) new ArgumentRelation("TruncateTimestamp", "", false, ""));
        switchToAdd.ArgumentRelationList.Add((object) new ArgumentRelation("SuppressStartupBanner", "", false, ""));
        switchToAdd.ArgumentRelationList.Add((object) new ArgumentRelation("Verbose", "", false, ""));
        string[][] switchMap = new string[7][]
        {
          new string[2]
          {
            "Delete",
            "-d"
          },
          new string[2]
          {
            "Move",
            "-m"
          },
          new string[2]
          {
            "Print",
            "-p"
          },
          new string[2]
          {
            "Quick",
            "-q"
          },
          new string[2]
          {
            "Replacement",
            "-r"
          },
          new string[2]
          {
            "Table",
            "-t"
          },
          new string[2]
          {
            "Extract",
            "-x"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("Command", switchMap, value);
        switchToAdd.Name = "Command";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("Command", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    [Required]
    public virtual string OutputFile
    {
      get
      {
        if (this.IsPropertySet("OutputFile"))
          return this.ActiveToolSwitches["OutputFile"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("OutputFile");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.File);
        switchToAdd.Separator = " ";
        switchToAdd.DisplayName = "Output File";
        switchToAdd.Description = "The /OUT option overrides the default name and location of the program that the lib creates.";
        switchToAdd.Required = true;
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "OutputFile";
        switchToAdd.Value = value;
        this.ActiveToolSwitches.Add("OutputFile", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    [Required]
    public virtual ITaskItem[] Sources
    {
      get
      {
        if (this.IsPropertySet("Sources"))
          return this.ActiveToolSwitches["Sources"].TaskItemArray;
        return (ITaskItem[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Sources");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.ITaskItemArray);
        switchToAdd.Required = true;
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.TaskItemArray = value;
        this.ActiveToolSwitches.Add("Sources", switchToAdd);
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
        switchToAdd.Description = "Tracker log directory.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Value = VCToolTask.EnsureTrailingSlash(value);
        this.ActiveToolSwitches.Add("TrackerLogDirectory", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool CreateIndex
    {
      get
      {
        if (this.IsPropertySet("CreateIndex"))
          return this.ActiveToolSwitches["CreateIndex"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("CreateIndex");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Create an archive index";
        switchToAdd.Description = "Create an archive index (cf. ranlib).  This can speed up linking and reduce dependency within its own library.";
        switchToAdd.Parents.AddLast("Command");
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "s";
        switchToAdd.Name = "CreateIndex";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("CreateIndex", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool CreateThinArchive
    {
      get
      {
        if (this.IsPropertySet("CreateThinArchive"))
          return this.ActiveToolSwitches["CreateThinArchive"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("CreateThinArchive");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Create Thin Archive";
        switchToAdd.Description = "Create a thin archive.  A thin archive contains relativepaths to the objects instead of embedding the objects.  Switching between Thin and Normal requires deleting the existing library.";
        switchToAdd.Parents.AddLast("Command");
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "T";
        switchToAdd.Name = "CreateThinArchive";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("CreateThinArchive", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool NoWarnOnCreate
    {
      get
      {
        if (this.IsPropertySet("NoWarnOnCreate"))
          return this.ActiveToolSwitches["NoWarnOnCreate"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("NoWarnOnCreate");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "No Warning on Create";
        switchToAdd.Description = "Do not warn if when the library is created.";
        switchToAdd.Parents.AddLast("Command");
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "c";
        switchToAdd.Name = "NoWarnOnCreate";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("NoWarnOnCreate", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool TruncateTimestamp
    {
      get
      {
        if (this.IsPropertySet("TruncateTimestamp"))
          return this.ActiveToolSwitches["TruncateTimestamp"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("TruncateTimestamp");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Truncate Timestamp";
        switchToAdd.Description = "Use zero for timestamps and uids/gids.";
        switchToAdd.Parents.AddLast("Command");
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "D";
        switchToAdd.Name = "TruncateTimestamp";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("TruncateTimestamp", switchToAdd);
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
        switchToAdd.Description = "Dont show version number.";
        switchToAdd.Parents.AddLast("Command");
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.ReverseSwitchValue = "V";
        switchToAdd.Name = "SuppressStartupBanner";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("SuppressStartupBanner", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool Verbose
    {
      get
      {
        if (this.IsPropertySet("Verbose"))
          return this.ActiveToolSwitches["Verbose"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Verbose");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Verbose";
        switchToAdd.Description = "Verbose";
        switchToAdd.Parents.AddLast("Command");
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "v";
        switchToAdd.Name = "Verbose";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("Verbose", switchToAdd);
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

    protected override bool MaintainCompositeRootingMarkers
    {
      get
      {
        return true;
      }
    }

    protected override string[] ReadTLogNames
    {
      get
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(this.ToolExe);
        return new string[3]
        {
          withoutExtension + ".read.*.tlog",
          withoutExtension + ".*.read.*.tlog",
          withoutExtension + "-*.read.*.tlog"
        };
      }
    }

    protected override string[] WriteTLogNames
    {
      get
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(this.ToolExe);
        return new string[3]
        {
          withoutExtension + ".write.*.tlog",
          withoutExtension + ".*.write.*.tlog",
          withoutExtension + "-*.write.*.tlog"
        };
      }
    }

    protected override string CommandTLogName
    {
      get
      {
        return Path.GetFileNameWithoutExtension(this.ToolExe) + ".command.1.tlog";
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

    protected override Encoding ResponseFileEncoding
    {
      get
      {
        return (Encoding) new UTF8Encoding(false);
      }
    }

    protected override ITaskItem[] TrackedInputFiles
    {
      get
      {
        return this.Sources;
      }
    }

    protected override Encoding StandardOutputEncoding
    {
      get
      {
        return Encoding.UTF8;
      }
    }

    protected override Encoding StandardErrorEncoding
    {
      get
      {
        return Encoding.UTF8;
      }
    }

    public Archive()
      : base(new ResourceManager("Microsoft.Build.CPPTasks.Strings", Assembly.GetExecutingAssembly()))
    {
      this.switchOrderList = new ArrayList();
      this.switchOrderList.Add((object) "AdditionalDependencies");
      this.switchOrderList.Add((object) "AdditionalLibraryDirectories");
      this.switchOrderList.Add((object) "LinkLibraryDependencies");
      this.switchOrderList.Add((object) "Command");
      this.switchOrderList.Add((object) "CreateIndex");
      this.switchOrderList.Add((object) "CreateThinArchive");
      this.switchOrderList.Add((object) "NoWarnOnCreate");
      this.switchOrderList.Add((object) "TruncateTimestamp");
      this.switchOrderList.Add((object) "SuppressStartupBanner");
      this.switchOrderList.Add((object) "Verbose");
      this.switchOrderList.Add((object) "AdditionalOptions");
      this.switchOrderList.Add((object) "OutputFile");
      this.switchOrderList.Add((object) "Sources");
      this.switchOrderList.Add((object) "TrackerLogDirectory");
    }

    protected override string GenerateResponseFileCommandsExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.EscapeTrailingSlash)
    {
      string str = base.GenerateResponseFileCommandsExceptSwitches(switchesToRemove, format, VCToolTask.EscapeFormat.EscapeTrailingSlash);
      if (format == VCToolTask.CommandLineFormat.ForBuildLog)
        str = str.Replace("\\", "\\\\").Replace("\\\\\\\\ ", "\\\\ ");
      return str;
    }
  }
}
