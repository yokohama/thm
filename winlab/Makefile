#
# [ Usage ]
# $ make SRC=source.cpp
#

SRC ?= hello-world.cpp
EXE = $(SRC:.cpp=.exe)

CC = x86_64-w64-mingw32-g++
CFLAGS = -static-libgcc -static-libstdc++ -mwindows -I/usr/share/mingw-w64/include

all: $(EXE)

$(EXE): $(SRC)
	$(CC) -o $@ $< $(CFLAGS)

clean:
	rm -f $(EXE)

