using SDL2;
using System;
using System.Runtime.CompilerServices;

unsafe static class HAL {
    static IntPtr window;
    static IntPtr renderer;
    static bool running = true;



    public static IntPtr getNewTexture(string filepath) {
        return SDL_image.IMG_LoadTexture(renderer, filepath);
    }
    public static void drawTexture(IntPtr texture, ushort x, ushort y, ushort w, ushort h) {
	    SDL.SDL_Rect r = new SDL.SDL_Rect(); r.x = x; r.y = y; r.w = w; r.h = h;
	    SDL.SDL_RenderCopy(renderer, texture, ref Unsafe.NullRef<SDL.SDL_Rect>(), ref r);
    }
    public static void releaseTexture(IntPtr texture) {
        SDL.SDL_DestroyTexture(texture);
    }
    static uint lastFontWidth = 0;
    static IntPtr font = IntPtr.Zero;
    public static unsafe void drawText(string str, ushort x, ushort y, uint fontWidth) {
	    if (fontWidth == 0) return;
	    if (fontWidth != lastFontWidth) {
		    if (font != IntPtr.Zero) SDL_ttf.TTF_CloseFont(font);
            string fontPath = getDirData(); fontPath += "Fonts/consola.ttf";
		    font = SDL_ttf.TTF_OpenFont(fontPath, (int)fontWidth);
            if (font == IntPtr.Zero) {
                string errStr = "Error: Can't load font: " + fontPath;
                Console.WriteLine(errStr);
                return;
            }
		    lastFontWidth = fontWidth;
	    }

	    SDL.SDL_Color color = new SDL.SDL_Color();
        color.r = color.g = color.b = 255;
	    SDL.SDL_Surface* surface = (SDL.SDL_Surface*)SDL_ttf.TTF_RenderText_Solid(font, str, color);
	    surface->refcount++;  //Added to prevent crash in debug
        IntPtr texture = SDL.SDL_CreateTextureFromSurface(renderer, (IntPtr)surface);
        uint strLen = (uint)str.Length;
        SDL.SDL_Rect r = new SDL.SDL_Rect();
        r.x = x; r.y = y; r.w = (int)fontWidth * (int)strLen; r.h = (int)fontWidth;
        SDL.SDL_RenderCopy(renderer, texture, ref Unsafe.NullRef<SDL.SDL_Rect>(), ref r);


	    SDL.SDL_DestroyTexture(texture);
	    SDL.SDL_FreeSurface((IntPtr)surface);
    }

    public static void drawRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a) {
        var rect = new SDL.SDL_Rect {
            x = x,
            y = y,
            w = w,
            h = h
        };
        SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
        SDL.SDL_RenderFillRect(renderer, ref rect);
        SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    }

    public static string getDirData() {
        return "./Data/";
    }



    public static void init() {
        // Initilizes SDL.
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0) {
            Console.WriteLine($"There was an issue initilizing SDL. {SDL.SDL_GetError()}");
        }

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        window = SDL.SDL_CreateWindow("SDL .NET 6 Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

        if (window == IntPtr.Zero) {
            Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
        }

        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        renderer = SDL.SDL_CreateRenderer(window,
                                                -1,
                                                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                                SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (renderer == IntPtr.Zero) {
            Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        }

        // Initilizes SDL_image for use with png files.
        if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0) {
            Console.WriteLine($"There was an issue initilizing SDL2_Image {SDL_image.IMG_GetError()}");
        }

        // Initilizes SDL_ttf for use with ttf files.
        if (SDL_ttf.TTF_Init() != 0) {
            Console.WriteLine($"There was an issue initilizing SDL2_TTF {SDL_ttf.TTF_GetError()}");
        }
    }

    public static void tickStart() {
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1) {
                switch (e.type) {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                }
            if (SDL.SDL_SetRenderDrawColor(renderer, 135, 206, 235, 255) < 0) {
                Console.WriteLine($"There was an issue with setting the render draw color. {SDL.SDL_GetError()}");
            }
            if (SDL.SDL_RenderClear(renderer) < 0) {
                Console.WriteLine($"There was an issue with clearing the render surface. {SDL.SDL_GetError()}");
            }
        }
    }

    public static void tickEnd() {
        SDL.SDL_RenderPresent(renderer);
    }

    public static void end() {
        // Clean up the resources that were created.
        SDL_ttf.TTF_Quit();
        SDL_image.IMG_Quit();
        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }

    public static bool isRunning() {
        return running;
    }
}
