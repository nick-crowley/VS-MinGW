// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.ClangLink
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Build.CPPTasks
{
  public class MingLink : TrackedVCToolTask
  {
    private ITaskItem[] preprocessOutput = new ITaskItem[0];
    private ArrayList switchOrderList;

    protected override string ToolName
    {
      get
      {
        return "clang.exe";
      }
    }

    public virtual bool MSVCErrorReport
    {
      get
      {
        if (this.IsPropertySet("MSVCErrorReport"))
          return this.ActiveToolSwitches["MSVCErrorReport"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("MSVCErrorReport");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Visual Studio Errors Reporting";
        switchToAdd.Description = "Report errors that Visual Studio can parse file and line information.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-fdiagnostics-format=msvc";
        switchToAdd.Name = "MSVCErrorReport";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("MSVCErrorReport", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

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
        switchToAdd.DisplayName = "Output File";
        switchToAdd.Description = "The option overrides the default name and location of the program that the linker creates. (-o)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-o";
        switchToAdd.Name = "OutputFile";
        switchToAdd.Value = value;
        this.ActiveToolSwitches.Add("OutputFile", switchToAdd);
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
        switchToAdd.Description = "Prints Linker Progress Messages.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "--stats";
        switchToAdd.Name = "ShowProgress";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("ShowProgress", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool Version
    {
      get
      {
        if (this.IsPropertySet("Version"))
          return this.ActiveToolSwitches["Version"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Version");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Version";
        switchToAdd.Description = "The -version option tells the linker to put a version number in the header of the executable.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-Wl,--version";
        switchToAdd.Name = "Version";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("Version", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool VerboseOutput
    {
      get
      {
        if (this.IsPropertySet("VerboseOutput"))
          return this.ActiveToolSwitches["VerboseOutput"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("VerboseOutput");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Enable Verbose Output";
        switchToAdd.Description = "The -verbose option tells the linker to output verbose messages for debugging.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-Wl,--verbose";
        switchToAdd.Name = "VerboseOutput";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("VerboseOutput", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool IncrementalLink
    {
      get
      {
        if (this.IsPropertySet("IncrementalLink"))
          return this.ActiveToolSwitches["IncrementalLink"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("IncrementalLink");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Enable Incremental Linking";
        switchToAdd.Description = "The option tells the linker to enable incremental linking.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-Wl,--incremental";
        switchToAdd.Name = "IncrementalLink";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("IncrementalLink", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] SharedLibrarySearchPath
    {
      get
      {
        if (this.IsPropertySet("SharedLibrarySearchPath"))
          return this.ActiveToolSwitches["SharedLibrarySearchPath"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("SharedLibrarySearchPath");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Shared Library Search Path";
        switchToAdd.Description = "Allows the user to populate the shared library search path.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-Wl,-rpath-link=";
        switchToAdd.Name = "SharedLibrarySearchPath";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("SharedLibrarySearchPath", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string Subsystem
    {
      get
      {
        if (this.IsPropertySet("Subsystem"))
          return this.ActiveToolSwitches["Subsystem"].Value;
        return (string)null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Subsystem");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Subsystem";
        switchToAdd.Description = "The type of application to be generated";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[3][]
        {
          new string[2]
          {
            "Console",
            "-mconsole"
          },
          new string[2]
          {
            "Windows",
            "-mwindows"
          },
          new string[2]
          {
            "DLL",
            "-mdll"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("Subsystem", switchMap, value);
        switchToAdd.Name = "Subsystem";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("Subsystem", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool ThreadSupport
    {
      get
      {
        if (this.IsPropertySet("ThreadSupport"))
          return this.ActiveToolSwitches["ThreadSupport"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ThreadSupport");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "ThreadSupport";
        switchToAdd.Description = "Specifies that MinGW-specific thread support is to be used.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-mthread";
        switchToAdd.Name = "ThreadSupport";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("ThreadSupport", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool UnicodeBuild
    {
      get
      {
        if (this.IsPropertySet("UnicodeBuild"))
          return this.ActiveToolSwitches["UnicodeBuild"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("UnicodeBuild");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "UnicodeBuild";
        switchToAdd.Description = "Causes the UNICODE preprocessor macro to be predefined, and chooses Unicode-capable runtime startup code.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-municode";
        switchToAdd.Name = "UnicodeBuild";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("UnicodeBuild", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool Win32Build
    {
      get
      {
        if (this.IsPropertySet("Win32Build"))
          return this.ActiveToolSwitches["Win32Build"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Win32Build");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Win32 Build";
        switchToAdd.Description = "Specifies that the typical Microsoft Windows predefined macros are to be set in the pre-processor, but does not influence the choice of runtime library/startup code.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-mwin32";
        switchToAdd.Name = "Win32Build";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("Win32Build", switchToAdd);
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
        switchToAdd.Description = "Allows the user to override the environmental library path. (-L folder).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-L";
        switchToAdd.Name = "AdditionalLibraryDirectories";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("AdditionalLibraryDirectories", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool UnresolvedSymbolReferences
    {
      get
      {
        if (this.IsPropertySet("UnresolvedSymbolReferences"))
          return this.ActiveToolSwitches["UnresolvedSymbolReferences"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("UnresolvedSymbolReferences");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Report Unresolved Symbol References";
        switchToAdd.Description = "This option when enabled will report unresolved symbol references.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-Wl,--no-undefined";
        switchToAdd.Name = "UnresolvedSymbolReferences";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("UnresolvedSymbolReferences", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool OptimizeforMemory
    {
      get
      {
        if (this.IsPropertySet("OptimizeforMemory"))
          return this.ActiveToolSwitches["OptimizeforMemory"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("OptimizeforMemory");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Optimize For Memory Usage";
        switchToAdd.Description = "Optimize for memory usage, by rereading the symbol tables as necessary.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "Wl,--no-keep-memory";
        switchToAdd.Name = "OptimizeforMemory";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("OptimizeforMemory", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string GccToolChain
    {
      get
      {
        if (this.IsPropertySet("GccToolChain"))
          return this.ActiveToolSwitches["GccToolChain"].Value;
        return (string) null;
      }
      set
      {
        //this.ActiveToolSwitches.Remove("GccToolChain");
        //ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        //switchToAdd.DisplayName = "Gcc Tool Chain";
        //switchToAdd.Description = "Folder path to Gcc Tool Chain.";
        //switchToAdd.ArgumentRelationList = new ArrayList();
        //switchToAdd.Name = "GccToolChain";
        //switchToAdd.Value = value;
        //switchToAdd.SwitchValue = "-gcc-toolchain ";
        //this.ActiveToolSwitches.Add("GccToolChain", switchToAdd);
        //this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string Target
    {
      get
      {
        if (this.IsPropertySet("Target"))
          return this.ActiveToolSwitches["Target"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Target");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Target";
        switchToAdd.Description = "Target";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "Target";
        switchToAdd.Value = value;
        switchToAdd.SwitchValue = "-target ";
        this.ActiveToolSwitches.Add("Target", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string Sysroot
    {
      get
      {
        if (this.IsPropertySet("Sysroot"))
          return this.ActiveToolSwitches["Sysroot"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Sysroot");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Sysroot";
        switchToAdd.Description = "Folder path to the root directory for headers and libraries.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "Sysroot";
        switchToAdd.Value = value;
        switchToAdd.SwitchValue = "Wl,--sysroot=";
        this.ActiveToolSwitches.Add("Sysroot", switchToAdd);
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

    public virtual string[] IgnoreSpecificDefaultLibraries
    {
      get
      {
        if (this.IsPropertySet("IgnoreSpecificDefaultLibraries"))
          return this.ActiveToolSwitches["IgnoreSpecificDefaultLibraries"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("IgnoreSpecificDefaultLibraries");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Ignore Specific Default Libraries";
        switchToAdd.Description = "Specifies one or more names of default libraries to ignore.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "Wl,--nostdlib";
        switchToAdd.Name = "IgnoreSpecificDefaultLibraries";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("IgnoreSpecificDefaultLibraries", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] ForceSymbolReferences
    {
      get
      {
        if (this.IsPropertySet("ForceSymbolReferences"))
          return this.ActiveToolSwitches["ForceSymbolReferences"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ForceSymbolReferences");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Force Symbol References";
        switchToAdd.Description = "Force symbol to be entered in the output file as an undefined symbol.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "Wl,-u--undefined=";
        switchToAdd.Name = "ForceSymbolReferences";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("ForceSymbolReferences", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string DebuggerSymbolInformation
    {
      get
      {
        if (this.IsPropertySet("DebuggerSymbolInformation"))
          return this.ActiveToolSwitches["DebuggerSymbolInformation"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("DebuggerSymbolInformation");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Debugger Symbol Information";
        switchToAdd.Description = "Debugger symbol information from the output file.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[5][]
        {
          new string[2]
          {
            "true",
            ""
          },
          new string[2]
          {
            "false",
            ""
          },
          new string[2]
          {
            "OmitUnneededSymbolInformation",
            "Wl,--strip-unneeded"
          },
          new string[2]
          {
            "OmitDebuggerSymbolInformation",
            "Wl,--strip-debug"
          },
          new string[2]
          {
            "OmitAllSymbolInformation",
            "Wl,--strip-all"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("DebuggerSymbolInformation", switchMap, value);
        switchToAdd.Name = "DebuggerSymbolInformation";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("DebuggerSymbolInformation", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool PackageDebugSymbols
    {
      get
      {
        if (this.IsPropertySet("PackageDebugSymbols"))
          return this.ActiveToolSwitches["PackageDebugSymbols"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("PackageDebugSymbols");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Package Debugger Symbol Information";
        switchToAdd.Description = "Strip the Debugger Symbols Information before Packaging.  A copy of the original binary will be used for debugging.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "PackageDebugSymbols";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("PackageDebugSymbols", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string GenerateMapFile
    {
      get
      {
        if (this.IsPropertySet("GenerateMapFile"))
          return this.ActiveToolSwitches["GenerateMapFile"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("GenerateMapFile");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Map File Name";
        switchToAdd.Description = "The Map option tells the linker to create a map file with the user specified name.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "GenerateMapFile";
        switchToAdd.Value = value;
        switchToAdd.SwitchValue = "Wl,-Map=";
        this.ActiveToolSwitches.Add("GenerateMapFile", switchToAdd);
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
        switchToAdd.Separator = " ";
        switchToAdd.Required = true;
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.TaskItemArray = value;
        this.ActiveToolSwitches.Add("Sources", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
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
        switchToAdd.Description = "Specifies additional items to add to the link command line.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "AdditionalDependencies";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("AdditionalDependencies", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] LibraryDependencies
    {
      get
      {
        if (this.IsPropertySet("LibraryDependencies"))
          return this.ActiveToolSwitches["LibraryDependencies"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("LibraryDependencies");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Library Dependencies";
        switchToAdd.Description = "This option allows specifying additional libraries to be  added to the linker command line. The additional library will be added to the end of the linker command line  prefixed with 'lib' and end with the '.a' extension.  (-lFILE)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "Wl,-l";
        switchToAdd.Name = "LibraryDependencies";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("LibraryDependencies", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool BuildingInIde
    {
      get
      {
        if (this.IsPropertySet("BuildingInIde"))
          return this.ActiveToolSwitches["BuildingInIde"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("BuildingInIde");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "BuildingInIde";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("BuildingInIde", switchToAdd);
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

    public bool GNUMode { get; set; }

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

    protected override ITaskItem[] TrackedInputFiles
    {
      get
      {
        return this.Sources;
      }
    }

    protected override Encoding ResponseFileEncoding
    {
      get
      {
        if (!this.GNUMode)
          return base.ResponseFileEncoding;
        return (Encoding) new UTF8Encoding(false);
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

    public MingLink()
      : base(new ResourceManager("Microsoft.Build.CPPTasks.Strings", Assembly.GetExecutingAssembly()))
    {
      this.switchOrderList = new ArrayList();
      this.switchOrderList.Add((object) "MSVCErrorReport");
      this.switchOrderList.Add((object) "OutputFile");
      this.switchOrderList.Add((object) "ShowProgress");
      this.switchOrderList.Add((object) "Version");
      this.switchOrderList.Add((object) "VerboseOutput");
      this.switchOrderList.Add((object) "IncrementalLink");
      this.switchOrderList.Add((object) "SharedLibrarySearchPath");
      this.switchOrderList.Add((object) "AdditionalLibraryDirectories");
      this.switchOrderList.Add((object) "UnresolvedSymbolReferences");
      this.switchOrderList.Add((object) "OptimizeforMemory");
      this.switchOrderList.Add((object) "GccToolChain");
      this.switchOrderList.Add((object) "Target");
      this.switchOrderList.Add((object) "Sysroot");
      this.switchOrderList.Add((object) "TrackerLogDirectory");
      this.switchOrderList.Add((object) "IgnoreSpecificDefaultLibraries");
      this.switchOrderList.Add((object) "ForceSymbolReferences");
      this.switchOrderList.Add((object) "DebuggerSymbolInformation");
      this.switchOrderList.Add((object) "PackageDebugSymbols");
      this.switchOrderList.Add((object) "GenerateMapFile");
      this.switchOrderList.Add((object) "Subsystem");
      this.switchOrderList.Add((object)"ThreadSupport");
      this.switchOrderList.Add((object)"UnicodeBuild");
      this.switchOrderList.Add((object)"Win32Build");
      this.switchOrderList.Add((object) "AdditionalOptions");
      this.switchOrderList.Add((object) "Sources");
      this.switchOrderList.Add((object) "AdditionalDependencies");
      this.switchOrderList.Add((object) "LibraryDependencies");
      this.switchOrderList.Add((object) "BuildingInIde");
    }

    protected override string GenerateResponseFileCommandsExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.EscapeTrailingSlash)
    {
      string str = base.GenerateResponseFileCommandsExceptSwitches(switchesToRemove, format, escapeFormat | VCToolTask.EscapeFormat.EscapeTrailingSlash);
      if (this.GNUMode && format == VCToolTask.CommandLineFormat.ForBuildLog)
        str = str.Replace("\\", "\\\\").Replace("\\\\\\\\ ", "\\\\ ");
      return str;
    }

    protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
    {
      Match match = new Regex("^\\s*(?<ORIGIN>(?<FILENAME>.*):(?<LOCATION>(?<LINE>[0-9]*))):(?<CATEGORY> error| warning):(?<TEXT>.*)", RegexOptions.IgnoreCase).Match(singleLine);
      if (!match.Success)
      {
        base.LogEventsFromTextOutput(singleLine, messageImportance);
      }
      else
      {
        string strA = match.Groups["CATEGORY"].Value.Trim();
        string message = match.Groups["TEXT"].Value.Trim();
        string file = match.Groups["FILENAME"].Value.Trim();
        int result = 0;
        int.TryParse(match.Groups["LINE"].Value.Trim(), out result);
        if (string.Compare(strA, "error", StringComparison.OrdinalIgnoreCase) == 0)
          this.LogPrivate.LogError((string) null, (string) null, (string) null, file, result, 0, 0, 0, message);
        else if (string.Compare(strA, "warning", StringComparison.OrdinalIgnoreCase) == 0)
          this.LogPrivate.LogWarning((string) null, (string) null, (string) null, file, result, 0, 0, 0, message);
        else
          base.LogEventsFromTextOutput(singleLine, messageImportance);
      }
    }
  }
}
