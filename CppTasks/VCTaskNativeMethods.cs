// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.CPPTasks.VCTaskNativeMethods
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Build.CPPTasks
{
  internal static class VCTaskNativeMethods
  {
    [DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr CreateEventW(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

    [DllImport("KERNEL32.DLL", SetLastError = true)]
    internal static extern bool SetEvent(IntPtr hEvent);

    [DllImport("KERNEL32.DLL", SetLastError = true)]
    internal static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint SearchPath(string lpPath, string lpFileName, string lpExtension, int nBufferLength, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpBuffer, out IntPtr lpFilePart);
  }
}
