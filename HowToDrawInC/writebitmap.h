#pragma once

#define WIDTH 100
#define HEIGHT 100

struct Color
{
    uint8_t red;
    uint8_t green;
    uint8_t blue;
};
Color bitmap[HEIGHT][WIDTH];

static void ClearBitmap(const Color& color)
{
    for (int y = 0; y < HEIGHT; y++)
    {
        for (int x = 0; x < HEIGHT; x++)
        {
            bitmap[y][x] = color;
        }
    }
}

static void PutPixel(int x, int y, const Color& color)
{
    bitmap[y][x].red = color.red;
    bitmap[y][x].green = color.green;
    bitmap[y][x].blue = color.blue;
}

static void DrawLine(int x1, int y1, int x2, int y2, const Color& color)
{
    int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;
    dx = x2 - x1;
    dy = y2 - y1;
    dx1 = (int)fabs(dx);
    dy1 = (int)fabs(dy);
    px = 2 * dy1 - dx1;
    py = 2 * dx1 - dy1;
    if (dy1 <= dx1)
    {
        if (dx >= 0)
        {
            x = x1;
            y = y1;
            xe = x2;
        }
        else
        {
            x = x2;
            y = y2;
            xe = x1;
        }
        PutPixel(x, y, color);
        for (i = 0; x<xe; i++)
        {
            x = x + 1;
            if (px<0)
            {
                px = px + 2 * dy1;
            }
            else
            {
                if ((dx<0 && dy<0) || (dx>0 && dy>0))
                {
                    y = y + 1;
                }
                else
                {
                    y = y - 1;
                }
                px = px + 2 * (dy1 - dx1);
            }
            PutPixel(x, y, color);
        }
    }
    else
    {
        if (dy >= 0)
        {
            x = x1;
            y = y1;
            ye = y2;
        }
        else
        {
            x = x2;
            y = y2;
            ye = y1;
        }
        PutPixel(x, y, color);
        for (i = 0; y<ye; i++)
        {
            y = y + 1;
            if (py <= 0)
            {
                py = py + 2 * dx1;
            }
            else
            {
                if ((dx<0 && dy<0) || (dx>0 && dy>0))
                {
                    x = x + 1;
                }
                else
                {
                    x = x - 1;
                }
                py = py + 2 * (dx1 - dy1);
            }
            PutPixel(x, y, color);
        }
    }
}
/*
This rather hacky function to write a bitmap is taken from here.
Just give it the size of your array and the RGB (24Bit)
https://en.wikipedia.org/wiki/User:Evercat/Buddhabrot.c
*/
static void WriteBitmap(char * filename)
{
    unsigned int headers[13];
    FILE * outfile;
    int extrabytes;
    int paddedsize;
    int x; int y; int n;

    extrabytes = 4 - ((WIDTH * 3) % 4);                 // How many bytes of padding to add to each
                                                        // horizontal line - the size of which must
                                                        // be a multiple of 4 bytes.
    if (extrabytes == 4)
        extrabytes = 0;

    paddedsize = ((WIDTH * 3) + extrabytes) * HEIGHT;

    // Headers...
    // Note that the "BM" identifier in bytes 0 and 1 is NOT included in these "headers".

    headers[0] = paddedsize + 54;      // bfSize (whole file size)
    headers[1] = 0;                    // bfReserved (both)
    headers[2] = 54;                   // bfOffbits
    headers[3] = 40;                   // biSize
    headers[4] = WIDTH;  // biWidth
    headers[5] = HEIGHT; // biHeight

                         // Would have biPlanes and biBitCount in position 6, but they're shorts.
                         // It's easier to write them out separately (see below) than pretend
                         // they're a single int, especially with endian issues...

    headers[7] = 0;                    // biCompression
    headers[8] = paddedsize;           // biSizeImage
    headers[9] = 0;                    // biXPelsPerMeter
    headers[10] = 0;                    // biYPelsPerMeter
    headers[11] = 0;                    // biClrUsed
    headers[12] = 0;                    // biClrImportant

    outfile = fopen(filename, "wb");

    //
    // Headers begin...
    // When printing ints and shorts, we write out 1 character at a time to avoid endian issues.
    //

    fprintf(outfile, "BM");

    for (n = 0; n <= 5; n++)
    {
        fprintf(outfile, "%c", headers[n] & 0x000000FF);
        fprintf(outfile, "%c", (headers[n] & 0x0000FF00) >> 8);
        fprintf(outfile, "%c", (headers[n] & 0x00FF0000) >> 16);
        fprintf(outfile, "%c", (headers[n] & (unsigned int)0xFF000000) >> 24);
    }

    // These next 4 characters are for the biPlanes and biBitCount fields.

    fprintf(outfile, "%c", 1);
    fprintf(outfile, "%c", 0);
    fprintf(outfile, "%c", 24);
    fprintf(outfile, "%c", 0);

    for (n = 7; n <= 12; n++)
    {
        fprintf(outfile, "%c", headers[n] & 0x000000FF);
        fprintf(outfile, "%c", (headers[n] & 0x0000FF00) >> 8);
        fprintf(outfile, "%c", (headers[n] & 0x00FF0000) >> 16);
        fprintf(outfile, "%c", (headers[n] & (unsigned int)0xFF000000) >> 24);
    }

    //
    // Headers done, now write the data...
    //

    for (y = HEIGHT - 1; y >= 0; y--)     // BMP image format is written from bottom to top...
    {
        for (x = 0; x <= WIDTH - 1; x++)
        {
            // Also, it's written in (b,g,r) format...

            fprintf(outfile, "%c", bitmap[y][x].blue);
            fprintf(outfile, "%c", bitmap[y][x].green);
            fprintf(outfile, "%c", bitmap[y][x].red);
        }
        if (extrabytes)      // See above - BMP lines must be of lengths divisible by 4.
        {
            for (n = 1; n <= extrabytes; n++)
            {
                fprintf(outfile, "%c", 0);
            }
        }
    }

    fclose(outfile);
    return;
}
