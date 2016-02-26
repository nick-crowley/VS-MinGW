// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.ClangCompile
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Microsoft.Build.CPPTasks
{
  public class MingCompile : TrackedVCToolTask
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

    protected override string AlwaysAppend
    {
      get
      {
        return "-c";
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
        switchToAdd.SwitchValue = "--sysroot=";
        this.ActiveToolSwitches.Add("Sysroot", switchToAdd);
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
        switchToAdd.Description = "Specifies one or more directories to add to the include path; separate with semi-colons if more than one. (-I[path]).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-I ";
        switchToAdd.Name = "AdditionalIncludeDirectories";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("AdditionalIncludeDirectories", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }


    public virtual string[] SystemIncludeDirectories
    {
      get
      {
        if (this.IsPropertySet("SystemIncludeDirectories"))
          return this.ActiveToolSwitches["SystemIncludeDirectories"].StringList;
        return (string[])null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("SystemIncludeDirectories");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Additional Include Directories";
        switchToAdd.Description = "Specifies one or more directories containing the operating system headers; separate with semi-colons if more than one. (-isystem [path]).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-isystem ";
        switchToAdd.Name = "SystemIncludeDirectories";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("SystemIncludeDirectories", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string DebugInformation
    {
      get
      {
        if (this.IsPropertySet("DebugInformation"))
          return this.ActiveToolSwitches["DebugInformation"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("DebugInformation");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Debug Information";
        switchToAdd.Description = "Specifies how much debugging information is generated by the compiler.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[4][]
        {
          new string[2]
          {
            "None",
            "-g0"
          },
          new string[2]
          {
            "Minimal",
            "-g1"
          },
          new string[2]
          {
            "Full",
            "-g2"
          },
          new string[2]
          {
            "Extra",
            "-g3"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("DebugInformation", switchMap, value);
        switchToAdd.Name = "DebugInformation";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("DebugInformation", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }


    public virtual string DebugInformationFormat
    {
      get
      {
        if (this.IsPropertySet("DebugInformationFormat"))
          return this.ActiveToolSwitches["DebugInformationFormat"].Value;
        return (string)null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("DebugInformationFormat");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Debug Information Format";
        switchToAdd.Description = "Specifies the type of debugging information generated by the compiler.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[5][]
        {
          new string[2]
          {
            "COFF",
            "-gcoff"
          },
          new string[2]
          {
            "DWARF2",
            "-gdwarf-2"
          },
          new string[2]
          {
            "DWARF3",
            "-gdwarf-3"
          },
          new string[2]
          {
            "DWARF4",
            "-gdwarf-4"
          },
          new string[2]
          {
            "GDB",
            "-ggdb"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("DebugInformationFormat", switchMap, value);
        switchToAdd.Name = "DebugInformationFormat";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("DebugInformationFormat", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string ObjectFileName
    {
      get
      {
        if (this.IsPropertySet("ObjectFileName"))
          return this.ActiveToolSwitches["ObjectFileName"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ObjectFileName");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.File);
        switchToAdd.DisplayName = "Object File Name";
        switchToAdd.Description = "Specifies a name to override the default object file name; can be file or directory name. (/Fo[name]).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-o ";
        switchToAdd.Name = "ObjectFileName";
        switchToAdd.Value = value;
        this.ActiveToolSwitches.Add("ObjectFileName", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string WarningLevel
    {
      get
      {
        if (this.IsPropertySet("WarningLevel"))
          return this.ActiveToolSwitches["WarningLevel"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("WarningLevel");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Warning Level";
        switchToAdd.Description = "Select how strict you want the compiler to be about code errors.  Other flags should be added directly to Additional Options. (/w, /Weverything).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[3][]
        {
          new string[2]
          {
            "Default",
            ""
          },
          new string[2]
          {
            "TurnOffAllWarnings",
            "-w"
          },
          new string[2]
          {
            "EnableAllWarnings",
            "-Wall"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("WarningLevel", switchMap, value);
        switchToAdd.Name = "WarningLevel";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("WarningLevel", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool TreatWarningAsError
    {
      get
      {
        if (this.IsPropertySet("TreatWarningAsError"))
          return this.ActiveToolSwitches["TreatWarningAsError"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("TreatWarningAsError");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Treat Warnings As Errors";
        switchToAdd.Description = "Treats all compiler warnings as errors. For a new project, it may be best to use /WX in all compilations; resolving all warnings will ensure the fewest possible hard-to-find code defects.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-Werror";
        switchToAdd.Name = "TreatWarningAsError";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("TreatWarningAsError", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool Pedantic
    {
      get
      {
        if (this.IsPropertySet("Pedantic"))
          return this.ActiveToolSwitches["Pedantic"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Pedantic");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Pedantic mode";
        switchToAdd.Description = "Issue all the warnings demanded by strict ISO C and ISO C++; reject all programs that use forbidden extensions";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-pedantic";
        switchToAdd.Name = "Pedantic";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("Pedantic", switchToAdd);
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
        switchToAdd.DisplayName = "Verbose mode";
        switchToAdd.Description = "When Verbose mode is enabled, this tool would print out more information that for diagnosing the build.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-v";
        switchToAdd.Name = "Verbose";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("Verbose", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }


    public virtual string WarnReorder
    {
      get
      {
        if (this.IsPropertySet("WarnReorder"))
          return this.ActiveToolSwitches["WarnReorder"].Value;
        return (string)null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("WarnReorder");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Field Initialization Order";
        switchToAdd.Description = "Warn when a member initializer list initializes a field out of declarative order.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[2][]
        {
          new string[2]
          {
            "Enable",
            "-Wreorder"
          },
          new string[2]
          {
            "Disable",
            "-Wno-reorder"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("WarnReorder", switchMap, value);
        switchToAdd.Name = "WarnReorder";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("WarnReorder", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string WarnSwitch
    {
      get
      {
        if (this.IsPropertySet("WarnSwitch"))
          return this.ActiveToolSwitches["WarnSwitch"].Value;
        return (string)null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("WarnSwitch");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Missing Switch Labels";
        switchToAdd.Description = "Warn whenever a switch statement has an index of enumerated type and lacks a case for one or more of the named codes of that enumeration.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[2][]
        {
          new string[2]
          {
            "Enable",
            "-Wswitch"
          },
          new string[2]
          {
            "Disable",
            "-Wno-switch"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("WarnSwitch", switchMap, value);
        switchToAdd.Name = "WarnSwitch";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("WarnSwitch", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }


    public virtual string WarnSwitchDefault
    {
      get
      {
        if (this.IsPropertySet("WarnSwitchDefault"))
          return this.ActiveToolSwitches["WarnSwitchDefault"].Value;
        return (string)null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("WarnSwitchDefault");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Missing Default Labels";
        switchToAdd.Description = "Warn whenever a switch statement does not have a default case.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[2][]
        {
          new string[2]
          {
            "Enable",
            "-Wswitch-default"
          },
          new string[2]
          {
            "Disable",
            "-Wno-switch-default"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("WarnSwitchDefault", switchMap, value);
        switchToAdd.Name = "WarnSwitchDefault";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("WarnSwitchDefault", switchToAdd);
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

    public virtual string Optimization
    {
      get
      {
        if (this.IsPropertySet("Optimization"))
          return this.ActiveToolSwitches["Optimization"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("Optimization");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Optimization";
        switchToAdd.Description = "Specifies the optimization level for the application.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[5][]
        {
          new string[2]
          {
            "Custom",
            ""
          },
          new string[2]
          {
            "Disabled",
            "-O0"
          },
          new string[2]
          {
            "MinSize",
            "-Os"
          },
          new string[2]
          {
            "MaxSpeed",
            "-O2"
          },
          new string[2]
          {
            "Full",
            "-O3"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("Optimization", switchMap, value);
        switchToAdd.Name = "Optimization";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("Optimization", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool StrictAliasing
    {
      get
      {
        if (this.IsPropertySet("StrictAliasing"))
          return this.ActiveToolSwitches["StrictAliasing"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("StrictAliasing");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Strict Aliasing";
        switchToAdd.Description = "Enforce strict aliasing rules: An object may only be accessed through its static type, dynamic type, or array of unsigned char.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-fstrict-aliasing";
        switchToAdd.ReverseSwitchValue = "-fno-strict-aliasing";
        switchToAdd.Name = "StrictAliasing";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("StrictAliasing", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }
    
    public virtual bool OmitFramePointers
    {
      get
      {
        if (this.IsPropertySet("OmitFramePointers"))
          return this.ActiveToolSwitches["OmitFramePointers"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("OmitFramePointers");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Omit Frame Pointer";
        switchToAdd.Description = "Suppresses creation of frame pointers on the call stack.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-fomit-frame-pointer";
        switchToAdd.ReverseSwitchValue = "-fno-omit-frame-pointer";
        switchToAdd.Name = "OmitFramePointers";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("OmitFramePointers", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string ExceptionHandling
    {
      get
      {
        if (this.IsPropertySet("ExceptionHandling"))
          return this.ActiveToolSwitches["ExceptionHandling"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ExceptionHandling");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Enable C++ Exceptions";
        switchToAdd.Description = "Specifies the model of exception handling to be used by the compiler.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[3][]
        {
          new string[2]
          {
            "Disabled",
            "-fno-exceptions"
          },
          new string[2]
          {
            "Enabled",
            "-fexceptions"
          },
          new string[2]
          {
            "UnwindTables",
            "-funwind-tables"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("ExceptionHandling", switchMap, value);
        switchToAdd.Name = "ExceptionHandling";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("ExceptionHandling", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool FunctionLevelLinking
    {
      get
      {
        if (this.IsPropertySet("FunctionLevelLinking"))
          return this.ActiveToolSwitches["FunctionLevelLinking"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("FunctionLevelLinking");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Enable Function-Level Linking";
        switchToAdd.Description = "Allows the compiler to package individual functions in the form of packaged functions (COMDATs). Required for edit and continue to work.     (ffunction-sections).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-ffunction-sections";
        switchToAdd.Name = "FunctionLevelLinking";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("FunctionLevelLinking", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool DataLevelLinking
    {
      get
      {
        if (this.IsPropertySet("DataLevelLinking"))
          return this.ActiveToolSwitches["DataLevelLinking"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("DataLevelLinking");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Enable Data-Level Linking";
        switchToAdd.Description = "Enables linker optimizations to remove unused data by emitting each data item in a separate section.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-fdata-sections";
        switchToAdd.Name = "DataLevelLinking";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("DataLevelLinking", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }
    

    public virtual string FloatABI
    {
      get
      {
        if (this.IsPropertySet("FloatABI"))
          return this.ActiveToolSwitches["FloatABI"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("FloatABI");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Floating-point ABI";
        switchToAdd.Description = "Selection option to choose the floating point ABI.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[3][]
        {
          new string[2]
          {
            "soft",
            "-mfloat-abi=soft"
          },
          new string[2]
          {
            "softfp",
            "-mfloat-abi=softfp"
          },
          new string[2]
          {
            "hard",
            "-mfloat-abi=hard"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("FloatABI", switchMap, value);
        switchToAdd.Name = "FloatABI";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("FloatABI", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string BufferSecurityCheck
    {
      get
      {
        if (this.IsPropertySet("BufferSecurityCheck"))
          return this.ActiveToolSwitches["BufferSecurityCheck"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("BufferSecurityCheck");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Security Check";
        switchToAdd.Description = "The Security Check helps detect stack-buffer over-runs, a common attempted attack upon a program's security. (fstack-protector).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[2][]
        {
          new string[2]
          {
            "false",
            ""
          },
          new string[2]
          {
            "true",
            "-fstack-protector"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("BufferSecurityCheck", switchMap, value);
        switchToAdd.Name = "BufferSecurityCheck";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("BufferSecurityCheck", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool PositionIndependentCode
    {
      get
      {
        if (this.IsPropertySet("PositionIndependentCode"))
          return this.ActiveToolSwitches["PositionIndependentCode"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("PositionIndependentCode");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Position Independent Code";
        switchToAdd.Description = "Generate Position Independent Code (PIC) for use in a shared library.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-fpic";
        switchToAdd.Name = "PositionIndependentCode";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("PositionIndependentCode", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool UseShortEnums
    {
      get
      {
        if (this.IsPropertySet("UseShortEnums"))
          return this.ActiveToolSwitches["UseShortEnums"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("UseShortEnums");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Use Short Enums";
        switchToAdd.Description = "Enum type uses only as many bytes required by input set of possible values.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-fshort-enums";
        switchToAdd.ReverseSwitchValue = "-fno-short-enums";
        switchToAdd.Name = "UseShortEnums";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("UseShortEnums", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool RuntimeTypeInfo
    {
      get
      {
        if (this.IsPropertySet("RuntimeTypeInfo"))
          return this.ActiveToolSwitches["RuntimeTypeInfo"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("RuntimeTypeInfo");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Enable Run-Time Type Information";
        switchToAdd.Description = "Adds code for checking C++ object types at run time (runtime type information).     (frtti, fno-rtti)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-frtti";
        switchToAdd.ReverseSwitchValue = "-fno-rtti";
        switchToAdd.Name = "RuntimeTypeInfo";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("RuntimeTypeInfo", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string CLanguageStandard
    {
      get
      {
        if (this.IsPropertySet("CLanguageStandard"))
          return this.ActiveToolSwitches["CLanguageStandard"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("CLanguageStandard");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "C Language Standard";
        switchToAdd.Description = "Determines the C language standard.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[8][]
        {
          new string[2]
          {
            "Default",
            ""
          },
          new string[2]
          {
            "c89",
            "-std=c89"
          },
          new string[2]
          {
            "iso9899:199409",
            "-std=iso9899:199409"
          },
          new string[2]
          {
            "c99",
            "-std=c99"
          },
          new string[2]
          {
            "c11",
            "-std=c11"
          },
          new string[2]
          {
            "gnu89",
            "-std=gnu89"
          },
          new string[2]
          {
            "gnu99",
            "-std=gnu99"
          },
          new string[2]
          {
            "gnu11",
            "-std=gnu11"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("CLanguageStandard", switchMap, value);
        switchToAdd.Name = "CLanguageStandard";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("CLanguageStandard", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string CppLanguageStandard
    {
      get
      {
        if (this.IsPropertySet("CppLanguageStandard"))
          return this.ActiveToolSwitches["CppLanguageStandard"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("CppLanguageStandard");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "C++ Language Standard";
        switchToAdd.Description = "Determines the C++ language standard.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[8][]
        {
          new string[2]
          {
            "Default",
            ""
          },
          new string[2]
          {
            "c++98",
            "-std=c++98"
          },
          new string[2]
          {
            "c++11",
            "-std=c++11"
          },
          new string[2]
          {
            "c++1y",
            "-std=c++1y"
          },
          new string[2]
          {
            "c++1y",
            "-std=c++1z"
          },
          new string[2]
          {
            "gnu++98",
            "-std=gnu++98"
          },
          new string[2]
          {
            "gnu++11",
            "-std=gnu++11"
          },
          new string[2]
          {
            "gnu++1y",
            "-std=gnu++1y"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("CppLanguageStandard", switchMap, value);
        switchToAdd.Name = "CppLanguageStandard";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("CppLanguageStandard", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
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
        switchToAdd.Description = "Defines a preprocessing symbols for your source file. (-D)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-D ";
        switchToAdd.Name = "PreprocessorDefinitions";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("PreprocessorDefinitions", switchToAdd);
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
        switchToAdd.Description = "Specifies one or more preprocessor undefines.  (-U [macro])";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-U ";
        switchToAdd.Name = "UndefinePreprocessorDefinitions";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("UndefinePreprocessorDefinitions", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool UndefineAllPreprocessorDefinitions
    {
      get
      {
        if (this.IsPropertySet("UndefineAllPreprocessorDefinitions"))
          return this.ActiveToolSwitches["UndefineAllPreprocessorDefinitions"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("UndefineAllPreprocessorDefinitions");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Undefine All Preprocessor Definitions";
        switchToAdd.Description = "Undefine all previously defined preprocessor values.  (-undef)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-undef";
        switchToAdd.Name = "UndefineAllPreprocessorDefinitions";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("UndefineAllPreprocessorDefinitions", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual bool ShowIncludes
    {
      get
      {
        if (this.IsPropertySet("ShowIncludes"))
          return this.ActiveToolSwitches["ShowIncludes"].BooleanValue;
        return false;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ShowIncludes");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
        switchToAdd.DisplayName = "Show Includes";
        switchToAdd.Description = "Generates a list of include files with compiler output.  (-H)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-H";
        switchToAdd.Name = "ShowIncludes";
        switchToAdd.BooleanValue = value;
        this.ActiveToolSwitches.Add("ShowIncludes", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string PrecompiledHeader
    {
      get
      {
        if (this.IsPropertySet("PrecompiledHeader"))
          return this.ActiveToolSwitches["PrecompiledHeader"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("PrecompiledHeader");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Precompiled Header";
        switchToAdd.Description = "Create/Use Precompiled Header:Enables creation or use of a precompiled header during the build.";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[3][]
        {
          new string[2]
          {
            "Create",
            ""
          },
          new string[2]
          {
            "Use",
            ""
          },
          new string[2]
          {
            "NotUsing",
            ""
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("PrecompiledHeader", switchMap, value);
        switchToAdd.Name = "PrecompiledHeader";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("PrecompiledHeader", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string PrecompiledHeaderFile
    {
      get
      {
        if (this.IsPropertySet("PrecompiledHeaderFile"))
          return this.ActiveToolSwitches["PrecompiledHeaderFile"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("PrecompiledHeaderFile");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.File);
        switchToAdd.DisplayName = "Precompiled Header File";
        switchToAdd.Description = "Specifies header file name to use for precompiled header file. This file will be also added to 'Forced Include Files' during build";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "PrecompiledHeaderFile";
        switchToAdd.Value = value;
        this.ActiveToolSwitches.Add("PrecompiledHeaderFile", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string PrecompiledHeaderOutputFileDirectory
    {
      get
      {
        if (this.IsPropertySet("PrecompiledHeaderOutputFileDirectory"))
          return this.ActiveToolSwitches["PrecompiledHeaderOutputFileDirectory"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("PrecompiledHeaderOutputFileDirectory");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Precompiled Header Output File Directory";
        switchToAdd.Description = "Specifies the directory for the generated precompiled header. This directory will be also added to 'Additional Include Directories' during build";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.Name = "PrecompiledHeaderOutputFileDirectory";
        switchToAdd.Value = value;
        switchToAdd.SwitchValue = "";
        this.ActiveToolSwitches.Add("PrecompiledHeaderOutputFileDirectory", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string PrecompiledHeaderCompileAs
    {
      get
      {
        if (this.IsPropertySet("PrecompiledHeaderCompileAs"))
          return this.ActiveToolSwitches["PrecompiledHeaderCompileAs"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("PrecompiledHeaderCompileAs");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Compile Precompiled Header As";
        switchToAdd.Description = "Select compile language option for precompiled header file (-x c-header, -x c++-header).";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[2][]
        {
          new string[2]
          {
            "CompileAsC",
            "-x c-header"
          },
          new string[2]
          {
            "CompileAsCpp",
            "-x c++-header"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("PrecompiledHeaderCompileAs", switchMap, value);
        switchToAdd.Name = "PrecompiledHeaderCompileAs";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("PrecompiledHeaderCompileAs", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string CompileAs
    {
      get
      {
        if (this.IsPropertySet("CompileAs"))
          return this.ActiveToolSwitches["CompileAs"].Value;
        return (string) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("CompileAs");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
        switchToAdd.DisplayName = "Compile As";
        switchToAdd.Description = "Select compile language option for .c and .cpp files.  'Default' will detect based on .c or .cpp extention. (-x c, -x c++)";
        switchToAdd.ArgumentRelationList = new ArrayList();
        string[][] switchMap = new string[3][]
        {
          new string[2]
          {
            "Default",
            ""
          },
          new string[2]
          {
            "CompileAsC",
            "-x c"
          },
          new string[2]
          {
            "CompileAsCpp",
            "-x c++"
          }
        };
        switchToAdd.SwitchValue = this.ReadSwitchMap("CompileAs", switchMap, value);
        switchToAdd.Name = "CompileAs";
        switchToAdd.Value = value;
        switchToAdd.MultipleValues = true;
        this.ActiveToolSwitches.Add("CompileAs", switchToAdd);
        this.AddActiveSwitchToolValue(switchToAdd);
      }
    }

    public virtual string[] ForcedIncludeFiles
    {
      get
      {
        if (this.IsPropertySet("ForcedIncludeFiles"))
          return this.ActiveToolSwitches["ForcedIncludeFiles"].StringList;
        return (string[]) null;
      }
      set
      {
        this.ActiveToolSwitches.Remove("ForcedIncludeFiles");
        ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
        switchToAdd.DisplayName = "Forced Include Files";
        switchToAdd.Description = "one or more forced include files.     (-include [name])";
        switchToAdd.ArgumentRelationList = new ArrayList();
        switchToAdd.SwitchValue = "-include ";
        switchToAdd.Name = "ForcedIncludeFiles";
        switchToAdd.StringList = value;
        this.ActiveToolSwitches.Add("ForcedIncludeFiles", switchToAdd);
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

    public MingCompile()
      : base(new ResourceManager("Microsoft.Build.CPPTasks.Strings", Assembly.GetExecutingAssembly()))
    {
      this.switchOrderList = new ArrayList();
      this.switchOrderList.Add((object) "AlwaysAppend");
      this.switchOrderList.Add((object) "MSVCErrorReport");
      this.switchOrderList.Add((object) "GccToolChain");
      this.switchOrderList.Add((object) "Target");
      this.switchOrderList.Add((object) "Sysroot");
      this.switchOrderList.Add((object) "AdditionalIncludeDirectories");
      this.switchOrderList.Add((object) "SystemIncludeDirectories");
      this.switchOrderList.Add((object) "DebugInformation");
      this.switchOrderList.Add((object) "DebugInformationFormat");
      this.switchOrderList.Add((object) "ObjectFileName");
      this.switchOrderList.Add((object) "WarningLevel");
      this.switchOrderList.Add((object) "TreatWarningAsError");
      this.switchOrderList.Add((object) "Verbose");
      this.switchOrderList.Add((object) "Pedantic");
      this.switchOrderList.Add((object) "WarnReorder");
      this.switchOrderList.Add((object) "WarnSwitch");
      this.switchOrderList.Add((object) "WarnSwitchDefault");
      this.switchOrderList.Add((object) "TrackerLogDirectory");
      this.switchOrderList.Add((object) "Optimization");
      this.switchOrderList.Add((object) "StrictAliasing");
      this.switchOrderList.Add((object) "OmitFramePointers");
      this.switchOrderList.Add((object) "ExceptionHandling");
      this.switchOrderList.Add((object) "FunctionLevelLinking");
      this.switchOrderList.Add((object) "DataLevelLinking");
      this.switchOrderList.Add((object) "FloatABI");
      this.switchOrderList.Add((object) "BufferSecurityCheck");
      this.switchOrderList.Add((object) "PositionIndependentCode");
      this.switchOrderList.Add((object) "UseShortEnums");
      this.switchOrderList.Add((object) "RuntimeTypeInfo");
      this.switchOrderList.Add((object) "CLanguageStandard");
      this.switchOrderList.Add((object) "CppLanguageStandard");
      this.switchOrderList.Add((object) "PreprocessorDefinitions");
      this.switchOrderList.Add((object) "UndefinePreprocessorDefinitions");
      this.switchOrderList.Add((object) "UndefineAllPreprocessorDefinitions");
      this.switchOrderList.Add((object) "ShowIncludes");
      this.switchOrderList.Add((object) "PrecompiledHeader");
      this.switchOrderList.Add((object) "PrecompiledHeaderFile");
      this.switchOrderList.Add((object) "PrecompiledHeaderOutputFileDirectory");
      this.switchOrderList.Add((object) "PrecompiledHeaderCompileAs");
      this.switchOrderList.Add((object) "CompileAs");
      this.switchOrderList.Add((object) "ForcedIncludeFiles");
      this.switchOrderList.Add((object) "AdditionalOptions");
      this.switchOrderList.Add((object) "Sources");
      this.switchOrderList.Add((object) "BuildingInIde");
    }

    protected override void RemoveTaskSpecificInputs(CanonicalTrackedInputFiles compactInputs)
    {
      if (this.IsPropertySet("PrecompiledHeader") && this.PrecompiledHeader != "Create" || !this.IsPropertySet("ObjectFileName"))
        return;
      TaskItem taskItem = new TaskItem(this.ObjectFileName);
      compactInputs.RemoveDependencyFromEntry(this.Sources, (ITaskItem) taskItem);
    }

    protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
    {
      foreach (ITaskItem taskItem in this.SourcesCompiled)
        this.Log.LogMessage(MessageImportance.High, Path.GetFileName(taskItem.ItemSpec), new object[0]);
      return base.ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
    }

    protected override string GenerateResponseFileCommandsExceptSwitches(string[] switchesToRemove, VCToolTask.CommandLineFormat format = VCToolTask.CommandLineFormat.ForBuildLog, VCToolTask.EscapeFormat escapeFormat = VCToolTask.EscapeFormat.EscapeTrailingSlash)
    {
      string str = base.GenerateResponseFileCommandsExceptSwitches(switchesToRemove, format, VCToolTask.EscapeFormat.EscapeTrailingSlash);
      if (this.GNUMode && format == VCToolTask.CommandLineFormat.ForBuildLog)
        str = str.Replace("\\", "\\\\").Replace("\\\\\\\\ ", "\\\\ ");
      return str;
    }
  }
}
