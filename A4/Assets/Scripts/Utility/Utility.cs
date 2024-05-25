using System.Diagnostics;
using System.Runtime.InteropServices;

using System;
using UnityEngine;

/// <summary>
/// Contains all utility functions & variable globally available.
/// </summary>
public static class Utility
{
    public const float PI = UnityEngine.Mathf.PI;
    public const float TWO_PI = 2.0f * UnityEngine.Mathf.PI;
    public const float HALF_PI = .5f * UnityEngine.Mathf.PI;

    /// <summary>
    /// Finds a direct child of parent that has tag.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    /// <returns>the child found, or null if not found.</returns>
    public static GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject ret = null;
        foreach(Transform childTr in parent.transform)
        {
            if(childTr.tag == tag)
            {
                ret = childTr.gameObject;
                break;
            }
        }

        return ret;
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-exitprocess
    /// </summary>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern void ExitProcess(UInt32 uExitCode);

    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox
    /// </summary>
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int MessageBox(IntPtr hWnd, string text, string title, UInt32 uType);

    /// <summary>
    /// Don't know who the fuck decided that Unity should catch and ignore all assertions and exceptions.
    /// I won't allow that to happen.
    /// </summary>
    public static void MyDebugAssert(bool condition, String msg = "")
    {
        if (!condition)
        {
// Don't do this in the actual release version even if an assertion fails.
#if UNITY_EDITOR
            Debugger.Break();
            Debugger.Log(0, "Assertion", "Debug Assertion Failed!\n" + msg);
            MessageBox
            (
                IntPtr.Zero, // no parent window
                "Message: " + msg,
                "Debug Assertion Failed!",
                0x00000000 | 0x00000010 // MB_OK | MB_ICONERROR
            );
            //ExitProcess(1);
#endif
        }
    }
}
