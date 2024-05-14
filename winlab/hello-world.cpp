#include <windows.h>

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmLine, int nCmdShow) {
  MessageBoxW(NULL, L"Hello, World!", L"Hello-World", MB_OK);
  return 0;
}
