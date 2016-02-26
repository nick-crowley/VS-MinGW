// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.TrackedVCToolTask
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using Microsoft.Build.Framework;
using Microsoft.Build.Shared;
using Microsoft.Build.Utilities;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Microsoft.Build.CPPTasks
{
  public abstract class TrackedVCToolTask : VCToolTask
  {
    private bool trackCommandLines = true;
    protected string pathToLog = string.Empty;
    private bool skippedExecution;
    private CanonicalTrackedInputFiles sourceDependencies;
    private CanonicalTrackedOutputFiles sourceOutputs;
    private bool trackFileAccess;
    private bool minimalRebuildFromTracking;
    private bool deleteOutputOnExecute;
    private string rootSource;
    private ITaskItem[] tlogReadFiles;
    private ITaskItem[] tlogWriteFiles;
    private ITaskItem tlogCommandFile;
    private ITaskItem[] sourcesCompiled;
    private ITaskItem[] trackedInputFilesToIgnore;
    private ITaskItem[] trackedOutputFilesToIgnore;
    private ITaskItem[] excludedInputPaths;
    private string pathOverride;
    private SafeFileHandle unicodePipeReadHandle;
    private SafeFileHandle unicodePipeWriteHandle;
    private AutoResetEvent unicodeOutputEnded;

    protected abstract string TrackerIntermediateDirectory { get; }

    protected abstract ITaskItem[] TrackedInputFiles { get; }

    protected CanonicalTrackedInputFiles SourceDependencies
    {
      get
      {
        return this.sourceDependencies;
      }
      set
      {
        this.sourceDependencies = value;
      }
    }

    protected CanonicalTrackedOutputFiles SourceOutputs
    {
      get
      {
        return this.sourceOutputs;
      }
      set
      {
        this.sourceOutputs = value;
      }
    }

    [Output]
    public bool SkippedExecution
    {
      get
      {
        return this.skippedExecution;
      }
      set
      {
        this.skippedExecution = value;
      }
    }

    protected string RootSource
    {
      get
      {
        return this.rootSource;
      }
      set
      {
        this.rootSource = value;
      }
    }

    protected abstract string[] ReadTLogNames { get; }

    protected abstract string[] WriteTLogNames { get; }

    protected abstract string CommandTLogName { get; }

    public ITaskItem[] TLogReadFiles
    {
      get
      {
        return this.tlogReadFiles;
      }
      set
      {
        this.tlogReadFiles = value;
      }
    }

    public ITaskItem[] TLogWriteFiles
    {
      get
      {
        return this.tlogWriteFiles;
      }
      set
      {
        this.tlogWriteFiles = value;
      }
    }

    public ITaskItem TLogCommandFile
    {
      get
      {
        return this.tlogCommandFile;
      }
      set
      {
        this.tlogCommandFile = value;
      }
    }

    public bool TrackFileAccess
    {
      get
      {
        return this.trackFileAccess;
      }
      set
      {
        this.trackFileAccess = value;
      }
    }

    public bool TrackCommandLines
    {
      get
      {
        return this.trackCommandLines;
      }
      set
      {
        this.trackCommandLines = value;
      }
    }

    public bool PostBuildTrackingCleanup { get; set; }

    public bool EnableExecuteTool { get; set; }

    public bool MinimalRebuildFromTracking
    {
      get
      {
        return this.minimalRebuildFromTracking;
      }
      set
      {
        this.minimalRebuildFromTracking = value;
      }
    }

    public virtual bool AttributeFileTracking
    {
      get
      {
        return false;
      }
    }

    [Output]
    public ITaskItem[] SourcesCompiled
    {
      get
      {
        return this.sourcesCompiled;
      }
      set
      {
        this.sourcesCompiled = value;
      }
    }

    public ITaskItem[] TrackedOutputFilesToIgnore
    {
      get
      {
        return this.trackedOutputFilesToIgnore;
      }
      set
      {
        this.trackedOutputFilesToIgnore = value;
      }
    }

    public ITaskItem[] TrackedInputFilesToIgnore
    {
      get
      {
        return this.trackedInputFilesToIgnore;
      }
      set
      {
        this.trackedInputFilesToIgnore = value;
      }
    }

    public bool DeleteOutputOnExecute
    {
      get
      {
        return this.deleteOutputOnExecute;
      }
      set
      {
        this.deleteOutputOnExecute = value;
      }
    }

    protected virtual bool MaintainCompositeRootingMarkers
    {
      get
      {
        return false;
      }
    }

    protected virtual bool UseMinimalRebuildOptimization
    {
      get
      {
        return false;
      }
    }

    public virtual string SourcesPropertyName
    {
      get
      {
        return "Sources";
      }
    }

    protected virtual ExecutableType? ToolType
    {
      get
      {
        return new ExecutableType?();
      }
    }

    public string ToolArchitecture { get; set; }

    public string TrackerFrameworkPath { get; set; }

    public string TrackerSdkPath { get; set; }

    public ITaskItem[] ExcludedInputPaths
    {
      get
      {
        return this.excludedInputPaths;
      }
      set
      {
        this.excludedInputPaths = value;
      }
    }

    public string PathOverride
    {
      get
      {
        return this.pathOverride;
      }
      set
      {
        this.pathOverride = value;
      }
    }

    protected virtual bool UseUnicodeOutput
    {
      get
      {
        return false;
      }
    }

    protected TrackedVCToolTask(ResourceManager taskResources)
      : base(taskResources)
    {
      this.PostBuildTrackingCleanup = true;
      this.EnableExecuteTool = true;
    }

    protected virtual void AssignDefaultTLogPaths()
    {
      if (this.TLogReadFiles == null)
      {
        this.TLogReadFiles = new ITaskItem[this.ReadTLogNames.Length];
        for (int index = 0; index < this.ReadTLogNames.Length; ++index)
          this.TLogReadFiles[index] = (ITaskItem) new TaskItem(Path.Combine(this.TrackerIntermediateDirectory, this.ReadTLogNames[index]));
      }
      if (this.TLogWriteFiles == null)
      {
        this.TLogWriteFiles = new ITaskItem[this.WriteTLogNames.Length];
        for (int index = 0; index < this.WriteTLogNames.Length; ++index)
          this.TLogWriteFiles[index] = (ITaskItem) new TaskItem(Path.Combine(this.TrackerIntermediateDirectory, this.WriteTLogNames[index]));
      }
      if (this.TLogCommandFile != null)
        return;
      this.TLogCommandFile = (ITaskItem) new TaskItem(Path.Combine(this.TrackerIntermediateDirectory, this.CommandTLogName));
    }

    protected override bool SkipTaskExecution()
    {
      return this.ComputeOutOfDateSources();
    }

    protected internal virtual bool ComputeOutOfDateSources()
    {
      if (this.TrackerIntermediateDirectory != null)
      {
        string intermediateDirectory = this.TrackerIntermediateDirectory;
      }
      else
      {
        string str = string.Empty;
      }
      if (this.MinimalRebuildFromTracking || this.TrackFileAccess)
        this.AssignDefaultTLogPaths();
      if (this.MinimalRebuildFromTracking && !this.ForcedRebuildRequired())
      {
        this.sourceOutputs = new CanonicalTrackedOutputFiles((ITask) this, this.TLogWriteFiles);
        this.sourceDependencies = new CanonicalTrackedInputFiles((ITask) this, this.TLogReadFiles, this.TrackedInputFiles, this.ExcludedInputPaths, this.sourceOutputs, this.UseMinimalRebuildOptimization, this.MaintainCompositeRootingMarkers);
        this.SourcesCompiled = this.MergeOutOfDateSourceLists(this.SourceDependencies.ComputeSourcesNeedingCompilation(false), this.GenerateSourcesOutOfDateDueToCommandLine());
        if (this.SourcesCompiled.Length == 0)
        {
          this.SkippedExecution = true;
          return this.SkippedExecution;
        }
        this.SourcesCompiled = this.AssignOutOfDateSources(this.SourcesCompiled);
        this.SourceDependencies.RemoveEntriesForSource(this.SourcesCompiled);
        this.SourceDependencies.SaveTlog();
        if (this.DeleteOutputOnExecute)
          TrackedVCToolTask.DeleteFiles(this.sourceOutputs.OutputsForSource(this.SourcesCompiled));
        this.sourceOutputs.RemoveEntriesForSource(this.SourcesCompiled);
        this.sourceOutputs.SaveTlog();
      }
      else
      {
        this.SourcesCompiled = this.TrackedInputFiles;
        if (this.SourcesCompiled == null || this.SourcesCompiled.Length == 0)
        {
          this.SkippedExecution = true;
          return this.SkippedExecution;
        }
      }
      if (this.TrackFileAccess)
        this.RootSource = FileTracker.FormatRootingMarker(this.SourcesCompiled);
      this.SkippedExecution = false;
      return this.SkippedExecution;
    }

    protected virtual ITaskItem[] AssignOutOfDateSources(ITaskItem[] sources)
    {
      return sources;
    }

    protected virtual bool ForcedRebuildRequired()
    {
      string metadata;
      try
      {
        metadata = this.TLogCommandFile.GetMetadata("FullPath");
      }
      catch (Exception ex)
      {
        if (!(ex is InvalidOperationException) && !(ex is NullReferenceException))
        {
          throw;
        }
        else
        {
          this.Log.LogWarningWithCodeFromResources("TrackedVCToolTask.RebuildingDueToInvalidTLog", (object) ex.Message);
          return true;
        }
      }
      if (File.Exists(metadata))
        return false;
      this.Log.LogMessageFromResources(MessageImportance.Low, "TrackedVCToolTask.RebuildingNoCommandTLog", (object) this.TLogCommandFile.GetMetadata("FullPath"));
      return true;
    }

    protected virtual List<ITaskItem> GenerateSourcesOutOfDateDueToCommandLine()
    {
      IDictionary<string, string> dictionary = this.MapSourcesToCommandLines();
      List<ITaskItem> list = new List<ITaskItem>();
      if (!this.TrackCommandLines)
        return list;
      if (dictionary.Count == 0)
      {
        foreach (ITaskItem taskItem in this.TrackedInputFiles)
          list.Add(taskItem);
      }
      else if (this.MaintainCompositeRootingMarkers)
      {
        string str1 = this.ApplyPrecompareCommandFilter(this.GenerateCommandLine(VCToolTask.CommandLineFormat.ForTracking, VCToolTask.EscapeFormat.Default));
        string str2 = (string) null;
        if (dictionary.TryGetValue(FileTracker.FormatRootingMarker(this.TrackedInputFiles), out str2))
        {
          string str3 = this.ApplyPrecompareCommandFilter(str2);
          if (str3 == null || !str1.Equals(str3, StringComparison.Ordinal))
          {
            foreach (ITaskItem taskItem in this.TrackedInputFiles)
              list.Add(taskItem);
          }
        }
        else
        {
          foreach (ITaskItem taskItem in this.TrackedInputFiles)
            list.Add(taskItem);
        }
      }
      else
      {
        string str1 = this.GenerateCommandLineExceptSwitches(new string[1]
        {
          this.SourcesPropertyName ?? "Sources"
        }, VCToolTask.CommandLineFormat.ForTracking, VCToolTask.EscapeFormat.Default);
        foreach (ITaskItem source in this.TrackedInputFiles)
        {
          string str2 = this.ApplyPrecompareCommandFilter(str1 + " " + source.GetMetadata("FullPath").ToUpperInvariant());
          string str3 = (string) null;
          if (dictionary.TryGetValue(FileTracker.FormatRootingMarker(source), out str3))
          {
            str3 = this.ApplyPrecompareCommandFilter(str3);
            if (str3 == null || !str2.Equals(str3, StringComparison.Ordinal))
              list.Add(source);
          }
          else
            list.Add(source);
        }
      }
      return list;
    }

    protected ITaskItem[] MergeOutOfDateSourceLists(ITaskItem[] sourcesOutOfDateThroughTracking, List<ITaskItem> sourcesWithChangedCommandLines)
    {
      if (sourcesWithChangedCommandLines.Count == 0)
        return sourcesOutOfDateThroughTracking;
      if (sourcesOutOfDateThroughTracking.Length == 0)
      {
        if (sourcesWithChangedCommandLines.Count == this.TrackedInputFiles.Length)
        {
          this.Log.LogMessageFromResources(MessageImportance.Low, "TrackedVCToolTask.RebuildingAllSourcesCommandLineChanged", new object[0]);
        }
        else
        {
          foreach (ITaskItem taskItem in sourcesWithChangedCommandLines)
            this.Log.LogMessageFromResources(MessageImportance.Low, "TrackedVCToolTask.RebuildingSourceCommandLineChanged", (object) taskItem.GetMetadata("FullPath"));
        }
        return sourcesWithChangedCommandLines.ToArray();
      }
      if (sourcesOutOfDateThroughTracking.Length == this.TrackedInputFiles.Length)
        return this.TrackedInputFiles;
      if (sourcesWithChangedCommandLines.Count == this.TrackedInputFiles.Length)
      {
        this.Log.LogMessageFromResources(MessageImportance.Low, "TrackedVCToolTask.RebuildingAllSourcesCommandLineChanged", new object[0]);
        return this.TrackedInputFiles;
      }
      Dictionary<ITaskItem, bool> dictionary = new Dictionary<ITaskItem, bool>();
      foreach (ITaskItem index in sourcesOutOfDateThroughTracking)
        dictionary[index] = false;
      foreach (ITaskItem key in sourcesWithChangedCommandLines)
      {
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, true);
      }
      List<ITaskItem> list = new List<ITaskItem>();
      foreach (ITaskItem key in this.TrackedInputFiles)
      {
        bool flag = false;
        if (dictionary.TryGetValue(key, out flag))
        {
          list.Add(key);
          if (flag)
            this.Log.LogMessageFromResources(MessageImportance.Low, "TrackedVCToolTask.RebuildingSourceCommandLineChanged", (object) key.GetMetadata("FullPath"));
        }
      }
      return list.ToArray();
    }

    protected IDictionary<string, string> MapSourcesToCommandLines()
    {
      IDictionary<string, string> dictionary1 = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      string metadata = this.TLogCommandFile.GetMetadata("FullPath");
      if (File.Exists(metadata))
      {
        using (StreamReader streamReader = File.OpenText(metadata))
        {
          bool flag = false;
          string key = string.Empty;
          for (string str1 = streamReader.ReadLine(); str1 != null; str1 = streamReader.ReadLine())
          {
            if (str1.Length == 0)
            {
              flag = true;
              break;
            }
            if ((int) str1[0] == 94)
            {
              if (str1.Length == 1)
              {
                flag = true;
                break;
              }
              key = str1.Substring(1);
            }
            else
            {
              string str2 = (string) null;
              if (!dictionary1.TryGetValue(key, out str2))
              {
                dictionary1[key] = str1;
              }
              else
              {
                IDictionary<string, string> dictionary2 = dictionary1;
                string index = key;
                dictionary2[index] = dictionary2[index] + "\r\n" + str1;
              }
            }
          }
          if (flag)
          {
            this.Log.LogWarningWithCodeFromResources("TrackedVCToolTask.RebuildingDueToInvalidTLogContents", (object) metadata);
            dictionary1 = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          }
        }
      }
      return dictionary1;
    }

    protected void WriteSourcesToCommandLinesTable(IDictionary<string, string> sourcesToCommandLines)
    {
      using (StreamWriter streamWriter = new StreamWriter(this.TLogCommandFile.GetMetadata("FullPath"), false, Encoding.Unicode))
      {
        foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) sourcesToCommandLines)
        {
          streamWriter.WriteLine("^" + keyValuePair.Key);
          streamWriter.WriteLine(this.ApplyPrecompareCommandFilter(keyValuePair.Value));
        }
      }
    }

    public override bool Execute()
    {
      this.BeginUnicodeOutput();
      try
      {
        return base.Execute();
      }
      finally
      {
        this.EndUnicodeOutput();
      }
    }

    protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
    {
      int exitCode = 0;
      if (this.EnableExecuteTool)
      {
        try
        {
          exitCode = this.TrackerExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
        }
        finally
        {
          if (this.PostBuildTrackingCleanup)
            exitCode = this.PostExecuteTool(exitCode);
        }
      }
      return exitCode;
    }

    protected virtual int PostExecuteTool(int exitCode)
    {
      if (this.MinimalRebuildFromTracking || this.TrackFileAccess)
      {
        this.SourceOutputs = new CanonicalTrackedOutputFiles(this.TLogWriteFiles);
        this.SourceDependencies = new CanonicalTrackedInputFiles(this.TLogReadFiles, this.TrackedInputFiles, this.ExcludedInputPaths, this.SourceOutputs, false, this.MaintainCompositeRootingMarkers);
        string[] strArray = (string[]) null;
        IDictionary<string, string> sourcesToCommandLines = this.MapSourcesToCommandLines();
        if (exitCode != 0)
        {
          this.SourceOutputs.RemoveEntriesForSource(this.SourcesCompiled);
          this.SourceOutputs.SaveTlog();
          this.SourceDependencies.RemoveEntriesForSource(this.SourcesCompiled);
          this.SourceDependencies.SaveTlog();
          if (this.TrackCommandLines)
          {
            if (this.MaintainCompositeRootingMarkers)
            {
              sourcesToCommandLines.Remove(FileTracker.FormatRootingMarker(this.SourcesCompiled));
            }
            else
            {
              foreach (ITaskItem source in this.SourcesCompiled)
                sourcesToCommandLines.Remove(FileTracker.FormatRootingMarker(source));
            }
            this.WriteSourcesToCommandLinesTable(sourcesToCommandLines);
          }
        }
        else
        {
          this.AddTaskSpecificOutputs(this.SourcesCompiled, this.SourceOutputs);
          this.RemoveTaskSpecificOutputs(this.SourceOutputs);
          this.SourceOutputs.RemoveDependenciesFromEntryIfMissing(this.SourcesCompiled);
          if (this.MaintainCompositeRootingMarkers)
          {
            strArray = this.SourceOutputs.RemoveRootsWithSharedOutputs(this.SourcesCompiled);
            foreach (string rootingMarker in strArray)
              this.SourceDependencies.RemoveEntryForSourceRoot(rootingMarker);
          }
          if (this.TrackedOutputFilesToIgnore != null && this.TrackedOutputFilesToIgnore.Length != 0)
          {
            Dictionary<string, ITaskItem> trackedOutputFilesToRemove = new Dictionary<string, ITaskItem>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
            foreach (ITaskItem taskItem in this.TrackedOutputFilesToIgnore)
              trackedOutputFilesToRemove.Add(taskItem.GetMetadata("FullPath"), taskItem);
            this.SourceOutputs.SaveTlog((DependencyFilter) (fullTrackedPath => !trackedOutputFilesToRemove.ContainsKey(fullTrackedPath)));
          }
          else
            this.SourceOutputs.SaveTlog();
          TrackedVCToolTask.DeleteEmptyFile(this.TLogWriteFiles);
          this.RemoveTaskSpecificInputs(this.SourceDependencies);
          this.SourceDependencies.RemoveDependenciesFromEntryIfMissing(this.SourcesCompiled);
          if (this.TrackedInputFilesToIgnore != null && this.TrackedInputFilesToIgnore.Length != 0)
          {
            Dictionary<string, ITaskItem> trackedInputFilesToRemove = new Dictionary<string, ITaskItem>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
            foreach (ITaskItem taskItem in this.TrackedInputFilesToIgnore)
              trackedInputFilesToRemove.Add(taskItem.GetMetadata("FullPath"), taskItem);
            this.SourceDependencies.SaveTlog((DependencyFilter) (fullTrackedPath => !trackedInputFilesToRemove.ContainsKey(fullTrackedPath)));
          }
          else
            this.SourceDependencies.SaveTlog();
          TrackedVCToolTask.DeleteEmptyFile(this.TLogReadFiles);
          if (this.TrackCommandLines)
          {
            if (this.MaintainCompositeRootingMarkers)
            {
              string str = this.GenerateCommandLine(VCToolTask.CommandLineFormat.ForTracking, VCToolTask.EscapeFormat.Default);
              sourcesToCommandLines[FileTracker.FormatRootingMarker(this.SourcesCompiled)] = str;
              if (strArray != null)
              {
                foreach (string key in strArray)
                  sourcesToCommandLines.Remove(key);
              }
            }
            else
            {
              string str = this.GenerateCommandLineExceptSwitches(new string[1]
              {
                this.SourcesPropertyName ?? "Sources"
              }, VCToolTask.CommandLineFormat.ForTracking, VCToolTask.EscapeFormat.Default);
              foreach (ITaskItem source in this.SourcesCompiled)
                sourcesToCommandLines[FileTracker.FormatRootingMarker(source)] = str + " " + source.GetMetadata("FullPath").ToUpperInvariant();
            }
            this.WriteSourcesToCommandLinesTable(sourcesToCommandLines);
          }
        }
      }
      return exitCode;
    }

    protected virtual void RemoveTaskSpecificOutputs(CanonicalTrackedOutputFiles compactOutputs)
    {
    }

    protected virtual void RemoveTaskSpecificInputs(CanonicalTrackedInputFiles compactInputs)
    {
    }

    protected virtual void AddTaskSpecificOutputs(ITaskItem[] sources, CanonicalTrackedOutputFiles compactOutputs)
    {
    }

    protected override void LogPathToTool(string toolName, string pathToTool)
    {
      base.LogPathToTool(toolName, this.pathToLog);
    }

    protected int TrackerExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
    {
      string dllName = (string) null;
      string str1 = (string) null;
      bool trackFileAccess = this.TrackFileAccess;
      string str2 = Environment.ExpandEnvironmentVariables(pathToTool);
      string responseFileCommands1 = responseFileCommands;
      string arguments = Environment.ExpandEnvironmentVariables(commandLineCommands);
      try
      {
        this.pathToLog = str2;
        string pathToTool1;
        if (trackFileAccess)
        {
          ExecutableType result = ExecutableType.SameAsCurrentProcess;
          if (!string.IsNullOrEmpty(this.ToolArchitecture))
          {
            if (!Enum.TryParse<ExecutableType>(this.ToolArchitecture, out result))
            {
              this.Log.LogErrorWithCodeFromResources("General.InvalidValue", (object) "ToolArchitecture", (object) this.GetType().Name);
              return -1;
            }
          }
          else
          {
            ExecutableType? toolType = this.ToolType;
            if (toolType.HasValue)
            {
              toolType = this.ToolType;
              result = toolType.Value;
            }
          }
          bool is64bit;
          if ((result == ExecutableType.Native32Bit || result == ExecutableType.Native64Bit) && NativeMethodsShared.Is64bitApplication(str2, out is64bit))
            result = is64bit ? ExecutableType.Native64Bit : ExecutableType.Native32Bit;
          try
          {
            pathToTool1 = FileTracker.GetTrackerPath(result, this.TrackerSdkPath);
            if (pathToTool1 == null)
              this.Log.LogErrorFromResources("Error.MissingFile", (object) "tracker.exe");
          }
          catch (Exception ex)
          {
            if (ExceptionHandling.NotExpectedException(ex))
            {
              throw;
            }
            else
            {
              this.Log.LogErrorWithCodeFromResources("General.InvalidValue", (object) "TrackerSdkPath", (object) this.GetType().Name);
              return -1;
            }
          }
          try
          {
            dllName = FileTracker.GetFileTrackerPath(result, this.TrackerFrameworkPath);
          }
          catch (Exception ex)
          {
            if (ExceptionHandling.NotExpectedException(ex))
            {
              throw;
            }
            else
            {
              this.Log.LogErrorWithCodeFromResources("General.InvalidValue", (object) "TrackerFrameworkPath", (object) this.GetType().Name);
              return -1;
            }
          }
        }
        else
          pathToTool1 = str2;
        if (string.IsNullOrEmpty(pathToTool1))
          return -1;
        ErrorUtilities.VerifyThrowInternalRooted(pathToTool1);
        string commandLineCommands1;
        if (trackFileAccess)
        {
          string str3 = FileTracker.TrackerArguments(str2, arguments, dllName, this.TrackerIntermediateDirectory, this.RootSource, this.CancelEventName);
          this.Log.LogMessageFromResources(MessageImportance.Low, "Native_TrackingCommandMessage", new object[0]);
          this.Log.LogMessage(MessageImportance.Low, pathToTool1 + (this.AttributeFileTracking ? " /a " : " ") + str3 + " " + responseFileCommands1, new object[0]);
          str1 = FileUtilities.GetTemporaryFile();
          using (StreamWriter streamWriter = new StreamWriter(str1, false, Encoding.Unicode))
            streamWriter.Write(FileTracker.TrackerResponseFileArguments(dllName, this.TrackerIntermediateDirectory, this.RootSource, this.CancelEventName));
          commandLineCommands1 = (this.AttributeFileTracking ? "/a @\"" : "@\"") + str1 + "\"" + FileTracker.TrackerCommandArguments(str2, arguments);
        }
        else
          commandLineCommands1 = arguments;
        return base.ExecuteTool(pathToTool1, responseFileCommands1, commandLineCommands1);
      }
      finally
      {
        if (str1 != null)
          this.DeleteTempFile(str1);
      }
    }

    private void BeginUnicodeOutput()
    {
      this.unicodePipeReadHandle = (SafeFileHandle) null;
      this.unicodePipeWriteHandle = (SafeFileHandle) null;
      this.unicodeOutputEnded = (AutoResetEvent) null;
      if (!this.UseUnicodeOutput)
        return;
      if (NativeMethodsShared.CreatePipe(out this.unicodePipeReadHandle, out this.unicodePipeWriteHandle, new NativeMethodsShared.SecurityAttributes()
      {
        lpSecurityDescriptor = NativeMethodsShared.NullIntPtr,
        bInheritHandle = true
      }, 0))
      {
        List<string> list = new List<string>();
        if (this.EnvironmentVariables != null)
          list.AddRange((IEnumerable<string>) this.EnvironmentVariables);
        list.Add("VS_UNICODE_OUTPUT=" + this.unicodePipeWriteHandle.DangerousGetHandle().ToString());
        this.EnvironmentVariables = list.ToArray();
        this.unicodeOutputEnded = new AutoResetEvent(false);
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReadUnicodeOutput));
      }
      else
        this.Log.LogWarningWithCodeFromResources("TrackedVCToolTask.CreateUnicodeOutputPipeFailed", (object) this.ToolName);
    }

    private void EndUnicodeOutput()
    {
      if (!this.UseUnicodeOutput)
        return;
      if (this.unicodePipeWriteHandle != null)
        this.unicodePipeWriteHandle.Close();
      if (this.unicodeOutputEnded != null)
      {
        this.unicodeOutputEnded.WaitOne();
        this.unicodeOutputEnded.Close();
      }
      if (this.unicodePipeReadHandle == null)
        return;
      this.unicodePipeReadHandle.Close();
    }

    private void ReadUnicodeOutput(object stateInfo)
    {
      byte[] numArray = new byte[1024];
      string lineOfText1 = string.Empty;
      uint lpNumberOfBytesRead;
      while (NativeMethodsShared.ReadFile(this.unicodePipeReadHandle, numArray, 1024U, out lpNumberOfBytesRead, NativeMethodsShared.NullIntPtr) && (int) lpNumberOfBytesRead != 0)
      {
        string str = lineOfText1 + Encoding.Unicode.GetString(numArray, 0, (int) lpNumberOfBytesRead);
        while (true)
        {
          int length;
          if ((length = str.IndexOf('\n')) != -1)
          {
            string lineOfText2 = str.Substring(0, length);
            str = str.Substring(length + 1);
            if (lineOfText2.Length > 0 && lineOfText2.EndsWith("\r", StringComparison.Ordinal))
              lineOfText2 = lineOfText2.Substring(0, lineOfText2.Length - 1);
            this.Log.LogMessageFromText(lineOfText2, this.StandardOutputImportanceToUse);
          }
          else
            break;
        }
        lineOfText1 = str;
      }
      if (!string.IsNullOrEmpty(lineOfText1))
        this.Log.LogMessageFromText(lineOfText1, this.StandardOutputImportanceToUse);
      this.unicodeOutputEnded.Set();
    }

    public virtual string ApplyPrecompareCommandFilter(string value)
    {
      return Regex.Replace(value, "(\\r?\\n)?(\\r?\\n)+", "$1");
    }

    protected string RemoveSwitchFromCommandLine(string removalWord, string cmdString, bool removeMultiple = false)
    {
      int startIndex1 = 0;
      int startIndex2;
      while ((startIndex2 = cmdString.IndexOf(removalWord, startIndex1, StringComparison.Ordinal)) >= 0)
      {
        if (startIndex2 == 0 || (int) cmdString[startIndex2 - 1] == 32)
        {
          int num1 = cmdString.IndexOf(' ', startIndex2);
          int num2 = num1 < 0 ? cmdString.Length : num1 + 1;
          cmdString = cmdString.Remove(startIndex2, num2 - startIndex2);
          if (!removeMultiple)
            break;
        }
        startIndex1 = startIndex2 + 1;
        if (startIndex1 >= cmdString.Length)
          break;
      }
      return cmdString;
    }

    protected static int DeleteFiles(ITaskItem[] filesToDelete)
    {
      if (filesToDelete == null)
        return 0;
      ITaskItem[] taskItemArray = TrackedDependencies.ExpandWildcards(filesToDelete);
      if (taskItemArray.Length == 0)
        return 0;
      int num = 0;
      foreach (ITaskItem taskItem in taskItemArray)
      {
        try
        {
          FileInfo fileInfo = new FileInfo(taskItem.ItemSpec);
          if (fileInfo.Exists)
          {
            fileInfo.Delete();
            ++num;
          }
        }
        catch (Exception ex)
        {
          if (!(ex is SecurityException))
          {
            if (!(ex is ArgumentException))
            {
              if (!(ex is UnauthorizedAccessException))
              {
                if (!(ex is PathTooLongException))
                {
                  if (!(ex is NotSupportedException))
                    throw;
                }
              }
            }
          }
        }
      }
      return num;
    }

    protected static int DeleteEmptyFile(ITaskItem[] filesToDelete)
    {
      if (filesToDelete == null)
        return 0;
      ITaskItem[] taskItemArray = TrackedDependencies.ExpandWildcards(filesToDelete);
      if (taskItemArray.Length == 0)
        return 0;
      int num = 0;
      foreach (ITaskItem taskItem in taskItemArray)
      {
        bool flag = false;
        try
        {
          FileInfo fileInfo = new FileInfo(taskItem.ItemSpec);
          if (fileInfo.Exists)
          {
            if (fileInfo.Length <= 4L)
              flag = true;
            if (flag)
            {
              fileInfo.Delete();
              ++num;
            }
          }
        }
        catch (Exception ex)
        {
          if (!(ex is SecurityException))
          {
            if (!(ex is ArgumentException))
            {
              if (!(ex is UnauthorizedAccessException))
              {
                if (!(ex is PathTooLongException))
                {
                  if (!(ex is NotSupportedException))
                    throw;
                }
              }
            }
          }
        }
      }
      return num;
    }
  }
}
