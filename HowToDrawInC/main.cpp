#include <fstream>
#include <cstdio>
#include <iostream>
#include "writebitmap.h"

void main(void* arg, void** args)
{
    Color col;
    col.blue = 255;
    col.green = 255;
    col.red = 255;

    ClearBitmap(col);

    col.red = 0;
    col.green = 0;
    col.blue = 255;
    DrawLine(0, 0, 50, 50, col);

    WriteBitmap("maze.bmp");

    system("pause");
}
