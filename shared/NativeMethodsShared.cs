// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Shared.NativeMethodsShared
// Assembly: vs-mingw.Build.CPPTasks.MinGW, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2F18145C-3300-4432-9D5D-084A88FCA512


using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Microsoft.Build.Shared
{
  internal static class NativeMethodsShared
  {
    internal static HandleRef NullHandleRef = new HandleRef((object) null, IntPtr.Zero);
    internal static IntPtr NullIntPtr = new IntPtr(0);
    internal static int MAX_PATH = 260;
    private static readonly Version ThreadErrorModeMinOsVersion = new Version(6, 1, 7600);
    internal const uint ERROR_INSUFFICIENT_BUFFER = 2147942522U;
    internal const uint STARTUP_LOADER_SAFEMODE = 16U;
    internal const uint S_OK = 0U;
    internal const uint S_FALSE = 1U;
    internal const uint ERROR_FILE_NOT_FOUND = 2147942402U;
    internal const uint FUSION_E_PRIVATE_ASM_DISALLOWED = 2148732996U;
    internal const uint RUNTIME_INFO_DONT_SHOW_ERROR_DIALOG = 64U;
    internal const uint FILE_TYPE_CHAR = 2U;
    internal const int STD_OUTPUT_HANDLE = -11;
    internal const uint RPC_S_CALLPENDING = 2147549461U;
    internal const uint E_ABORT = 2147500036U;
    internal const int FILE_ATTRIBUTE_READONLY = 1;
    internal const int FILE_ATTRIBUTE_DIRECTORY = 16;
    internal const int FILE_ATTRIBUTE_REPARSE_POINT = 1024;
    private const string kernel32Dll = "kernel32.dll";
    private const string mscoreeDLL = "mscoree.dll";
    internal const ushort PROCESSOR_ARCHITECTURE_INTEL = (ushort) 0;
    internal const ushort PROCESSOR_ARCHITECTURE_ARM = (ushort) 5;
    internal const ushort PROCESSOR_ARCHITECTURE_IA64 = (ushort) 6;
    internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = (ushort) 9;
    internal const uint INFINITE = 4294967295U;
    internal const uint WAIT_ABANDONED_0 = 128U;
    internal const uint WAIT_OBJECT_0 = 0U;
    internal const uint WAIT_TIMEOUT = 258U;
    internal const int BinaryType_64Bit = 6;

    internal static int SetErrorMode(int newMode)
    {
      if (!(Environment.OSVersion.Version >= NativeMethodsShared.ThreadErrorModeMinOsVersion))
        return NativeMethodsShared.SetErrorMode_VistaAndOlder(newMode);
      int oldMode;
      NativeMethodsShared.SetErrorMode_Win7AndNewer(newMode, out oldMode);
      return oldMode;
    }

    [DllImport("kernel32.dll", EntryPoint = "SetThreadErrorMode", SetLastError = true)]
    private static extern bool SetErrorMode_Win7AndNewer(int newMode, out int oldMode);

    [DllImport("kernel32.dll", EntryPoint = "SetErrorMode")]
    private static extern int SetErrorMode_VistaAndOlder(int newMode);
    

    internal static string GetShortFilePath(string path)
    {
      if (path != null)
      {
        int shortPathName = NativeMethodsShared.GetShortPathName(path, (StringBuilder) null, 0);
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (shortPathName > 0)
        {
          StringBuilder fullpath = new StringBuilder(shortPathName);
          shortPathName = NativeMethodsShared.GetShortPathName(path, fullpath, shortPathName);
          lastWin32Error = Marshal.GetLastWin32Error();
          if (shortPathName > 0)
            path = fullpath.ToString();
        }
        if (shortPathName == 0 && lastWin32Error != 0)
          NativeMethodsShared.ThrowExceptionForErrorCode(lastWin32Error);
      }
      return path;
    }

    internal static string GetLongFilePath(string path)
    {
      if (path != null)
      {
        int longPathName = NativeMethodsShared.GetLongPathName(path, (StringBuilder) null, 0);
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (longPathName > 0)
        {
          StringBuilder fullpath = new StringBuilder(longPathName);
          longPathName = NativeMethodsShared.GetLongPathName(path, fullpath, longPathName);
          lastWin32Error = Marshal.GetLastWin32Error();
          if (longPathName > 0)
            path = fullpath.ToString();
        }
        if (longPathName == 0 && lastWin32Error != 0)
          NativeMethodsShared.ThrowExceptionForErrorCode(lastWin32Error);
      }
      return path;
    }
    
    public static bool HResultSucceeded(int hr)
    {
      return hr >= 0;
    }

    public static bool HResultFailed(int hr)
    {
      return hr < 0;
    }

    public static void ThrowExceptionForErrorCode(int errorCode)
    {
      errorCode = -2147024896 | errorCode;
      Marshal.ThrowExceptionForHR(errorCode);
    }

    internal static string FindOnPath(string filename)
    {
      StringBuilder buffer = new StringBuilder(NativeMethodsShared.MAX_PATH + 1);
      string str = (string) null;
      for (int index = 0; index < 2; ++index)
      {
        uint num = NativeMethodsShared.SearchPath((string) null, filename, (string) null, buffer.Capacity, buffer, (int[]) null);
        if ((long) num > (long) buffer.Capacity)
        {
          ErrorUtilities.VerifyThrow(index == 0, "We should not have to resize the buffer twice.");
          buffer.Capacity = (int) num;
        }
        else
        {
          if (num > 0U)
          {
            str = buffer.ToString();
            break;
          }
          break;
        }
      }
      return str;
    }
    
    internal static string GetCurrentDirectory()
    {
      StringBuilder lpBuffer = new StringBuilder(NativeMethodsShared.MAX_PATH);
      if (NativeMethodsShared.GetCurrentDirectory(NativeMethodsShared.MAX_PATH, lpBuffer) > 0)
        return lpBuffer.ToString();
      return (string) null;
    }

    [DllImport("kernel32.dll")]
    internal static extern int GetOEMCP();
    
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint SearchPath(string path, string fileName, string extension, int numBufferChars, [Out] StringBuilder buffer, int[] filePart);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FreeLibrary([In] IntPtr module);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
    internal static extern IntPtr GetProcAddress(IntPtr module, string procName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr LoadLibrary(string fileName);

    [DllImport("mscoree.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint GetRequestedRuntimeInfo(string pExe, string pwszVersion, string pConfigurationFile, uint startupFlags, uint runtimeInfoFlags, [Out] StringBuilder pDirectory, int dwDirectory, out uint dwDirectoryLength, [Out] StringBuilder pVersion, int cchBuffer, out uint dwlength);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GetModuleFileName(HandleRef hModule, [Out] StringBuilder buffer, int length);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    internal static extern uint GetFileType(IntPtr hFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GetCurrentDirectory(int nBufferLength, [Out] StringBuilder lpBuffer);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetCurrentDirectory(string path);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int GetFullPathName(string target, int bufferLength, [Out] StringBuilder buffer, IntPtr mustBeZero);
    
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, BestFitMapping = false)]
    internal static extern int GetShortPathName(string path, [Out] StringBuilder fullpath, [In] int length);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, BestFitMapping = false)]
    internal static extern int GetLongPathName([In] string path, [Out] StringBuilder fullpath, [In] int length);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool CreatePipe(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, NativeMethodsShared.SecurityAttributes lpPipeAttributes, int nSize);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool ReadFile(SafeFileHandle hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);
    
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool GetBinaryType([In] string lpApplicationName, out int pdwType);
    
    internal static bool Is64bitApplication(string filePath, out bool is64bit)
    {
      int pdwType = 0;
      int num = NativeMethodsShared.GetBinaryType(filePath, out pdwType) ? 1 : 0;
      is64bit = pdwType == 6;
      return num != 0;
    }
    

    private enum eDesiredAccess
    {
      PROCESS_TERMINATE = 1,
      PROCESS_CREATE_THREAD = 2,
      PROCESS_SET_SESSIONID = 4,
      PROCESS_VM_OPERATION = 8,
      PROCESS_VM_READ = 16,
      PROCESS_VM_WRITE = 32,
      PROCESS_DUP_HANDLE = 64,
      PROCESS_CREATE_PROCESS = 128,
      PROCESS_SET_QUOTA = 256,
      PROCESS_SET_INFORMATION = 512,
      PROCESS_QUERY_INFORMATION = 1024,
      DELETE = 65536,
      READ_CONTROL = 131072,
      WRITE_DAC = 262144,
      WRITE_OWNER = 524288,
      SYNCHRONIZE = 1048576,
      PROCESS_ALL_ACCESS = 1052671,
      STANDARD_RIGHTS_ALL = 2031616,
    }
    

    internal class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
      private SafeProcessHandle()
        : base(true)
      {
      }

      protected override bool ReleaseHandle()
      {
        return NativeMethodsShared.SafeProcessHandle.CloseHandle(this.handle);
      }

      [DllImport("KERNEL32.DLL")]
      private static extern bool CloseHandle(IntPtr hObject);
    }
    
    

    [StructLayout(LayoutKind.Sequential)]
    internal class SecurityAttributes
    {
      private uint nLength;
      public IntPtr lpSecurityDescriptor;
      public bool bInheritHandle;

      public SecurityAttributes()
      {
        this.nLength = (uint) Marshal.SizeOf(typeof (NativeMethodsShared.SecurityAttributes));
      }
    }
  }
}
