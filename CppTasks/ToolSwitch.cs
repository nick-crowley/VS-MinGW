// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.ToolSwitch
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using Microsoft.Build.Framework;
using Microsoft.Build.Shared;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Build.CPPTasks
{
  public class ToolSwitch
  {
    private string name = string.Empty;
    private string falseSuffix = string.Empty;
    private string trueSuffix = string.Empty;
    private string separator = string.Empty;
    private string argumentParameter = string.Empty;
    private string fallback = string.Empty;
    private LinkedList<string> parents = new LinkedList<string>();
    private LinkedList<KeyValuePair<string, string>> overrides = new LinkedList<KeyValuePair<string, string>>();
    private bool booleanValue = true;
    private string value = string.Empty;
    private string switchValue = string.Empty;
    private string reverseSwitchValue = string.Empty;
    private string description = string.Empty;
    private string displayName = string.Empty;
    private ToolSwitchType type;
    private bool argumentRequired;
    private bool required;
    private ArrayList argumentRelationList;
    private bool isValid;
    private bool reversible;
    private int number;
    private string[] stringList;
    private ITaskItem taskItem;
    private ITaskItem[] taskItemArray;
    private const string typeBoolean = "ToolSwitchType.Boolean";
    private const string typeInteger = "ToolSwitchType.Integer";
    private const string typeITaskItem = "ToolSwitchType.ITaskItem";
    private const string typeITaskItemArray = "ToolSwitchType.ITaskItemArray";
    private const string typeStringArray = "ToolSwitchType.StringArray or ToolSwitchType.StringPathArray";

    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = value;
      }
    }

    public string Value
    {
      get
      {
        return this.value;
      }
      set
      {
        this.value = value;
      }
    }

    public bool IsValid
    {
      get
      {
        return this.isValid;
      }
      set
      {
        this.isValid = value;
      }
    }

    public string SwitchValue
    {
      get
      {
        return this.switchValue;
      }
      set
      {
        this.switchValue = value;
      }
    }

    public string ReverseSwitchValue
    {
      get
      {
        return this.reverseSwitchValue;
      }
      set
      {
        this.reverseSwitchValue = value;
      }
    }

    public string DisplayName
    {
      get
      {
        return this.displayName;
      }
      set
      {
        this.displayName = value;
      }
    }

    public string Description
    {
      get
      {
        return this.description;
      }
      set
      {
        this.description = value;
      }
    }

    public ToolSwitchType Type
    {
      get
      {
        return this.type;
      }
      set
      {
        this.type = value;
      }
    }

    public bool Reversible
    {
      get
      {
        return this.reversible;
      }
      set
      {
        this.reversible = value;
      }
    }

    public bool MultipleValues { get; set; }

    public string FalseSuffix
    {
      get
      {
        return this.falseSuffix;
      }
      set
      {
        this.falseSuffix = value;
      }
    }

    public string TrueSuffix
    {
      get
      {
        return this.trueSuffix;
      }
      set
      {
        this.trueSuffix = value;
      }
    }

    public string Separator
    {
      get
      {
        return this.separator;
      }
      set
      {
        this.separator = value;
      }
    }

    public string FallbackArgumentParameter
    {
      get
      {
        return this.fallback;
      }
      set
      {
        this.fallback = value;
      }
    }

    public bool ArgumentRequired
    {
      get
      {
        return this.argumentRequired;
      }
      set
      {
        this.argumentRequired = value;
      }
    }

    public bool Required
    {
      get
      {
        return this.required;
      }
      set
      {
        this.required = value;
      }
    }

    public LinkedList<string> Parents
    {
      get
      {
        return this.parents;
      }
    }

    public LinkedList<KeyValuePair<string, string>> Overrides
    {
      get
      {
        return this.overrides;
      }
    }

    public ArrayList ArgumentRelationList
    {
      get
      {
        return this.argumentRelationList;
      }
      set
      {
        this.argumentRelationList = value;
      }
    }

    public bool BooleanValue
    {
      get
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.Boolean, "InvalidType", (object) "ToolSwitchType.Boolean");
        return this.booleanValue;
      }
      set
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.Boolean, "InvalidType", (object) "ToolSwitchType.Boolean");
        this.booleanValue = value;
      }
    }

    public int Number
    {
      get
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.Integer, "InvalidType", (object) "ToolSwitchType.Integer");
        return this.number;
      }
      set
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.Integer, "InvalidType", (object) "ToolSwitchType.Integer");
        this.number = value;
      }
    }

    public string[] StringList
    {
      get
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.StringArray || this.type == ToolSwitchType.StringPathArray, "InvalidType", (object) "ToolSwitchType.StringArray or ToolSwitchType.StringPathArray");
        return this.stringList;
      }
      set
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.StringArray || this.type == ToolSwitchType.StringPathArray, "InvalidType", (object) "ToolSwitchType.StringArray or ToolSwitchType.StringPathArray");
        this.stringList = value;
      }
    }

    public ITaskItem TaskItem
    {
      get
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.ITaskItem, "InvalidType", (object) "ToolSwitchType.ITaskItem");
        return this.taskItem;
      }
      set
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.ITaskItem, "InvalidType", (object) "ToolSwitchType.ITaskItem");
        this.taskItem = value;
      }
    }

    public ITaskItem[] TaskItemArray
    {
      get
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.ITaskItemArray, "InvalidType", (object) "ToolSwitchType.ITaskItemArray");
        return this.taskItemArray;
      }
      set
      {
        ErrorUtilities.VerifyThrow(this.type == ToolSwitchType.ITaskItemArray, "InvalidType", (object) "ToolSwitchType.ITaskItemArray");
        this.taskItemArray = value;
      }
    }

    public ToolSwitch()
    {
    }

    public ToolSwitch(ToolSwitchType toolType)
    {
      this.type = toolType;
    }
  }
}
