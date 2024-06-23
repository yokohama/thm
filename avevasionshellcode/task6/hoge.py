import http.server
import ssl

server_address = ('0.0.0.0', 443)
httpd = http.server.HTTPServer(server_address, http.server.SimpleHTTPRequestHandler)

# TLS 1.3 でも TLS_SERVER を使用する
context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)

# certfile と keyfile のパスを指定
context.load_cert_chain('localhost.pem')

httpd.socket = context.wrap_socket(httpd.socket, server_side=True)
httpd.serve_forever()
