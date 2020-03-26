using System;
using System.IO;
using System.Threading;

namespace hexseekr
{
    class Program
    {
        public struct sector {
            public sector(ulong a, ulong b)
            {
                _start = a;
                __end = b;
            }

            public ulong _start;
            public ulong __end;
        };

        static TimeSpan intv;
        static ulong addr, startpos = 0, forbuffer = 100000000, endpos = 0x100000000;
        static long jumpsize = 1;
        static byte[] data = System.Text.Encoding.ASCII.GetBytes("\0\0\0\012345678\0\0\0\0placeholder_text");
        static bool[] waitmodes = new bool[]
            { true, false, false };
        static sector[] regions = new sector[] { new sector(0,32) };
        static bool randdata = true, slowinit = false, verboseinit = true,
            invalid_hex_rand = false, invalid_char_rand = false;
        static int[] size = new int[2] { 81, 41 };
        static double seconds = 0.012, inittime = 2; //, freq = 0.4, glitchintensity = 0;
        static int randseed = (int)DateTime.Now.Ticks, innum = 0, innums = 0, datacursor = 0,
            its = 100000, rand, tableoff = 8, width = 16; //, glitchcount = 10;
        static Random random = new Random(randseed);
        static string[][] infile;
        static string[] infnames, instrs,  fnamesep = { "::\\\\//<<>>\"\"??^^**" };
        static string infiles, instr, invalid_hex = "??";
        static char invalid_char = '.';
        
        static bool real(ulong i)
        {
            for (int j = 0; j < regions.Length; j++)
                if (i >= regions[j]._start && i < regions[j].__end)
                    return true;
                else
                    return false;
            return false;
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Length > 2)
                    if (args[i].StartsWith("-"))
                        switch (args[i][1])
                        {
                            case 'i':
                                infiles += args[i].Substring(2) + fnamesep[0];
                                innum++;
                                break;
                            case 'd':
                                instr += args[i].Substring(2) + fnamesep[0];
                                innums++;
                                break;
                            case 's':
                                seconds = Convert.ToDouble(args[i].Substring(2));
                                break;
                            case 'c':
                                its = Convert.ToInt32(args[i].Substring(2));
                                break;
                            case 'b':
                                forbuffer = Convert.ToUInt64(args[i].Substring(2));
                                break;
                            case 'o':
                                startpos = Convert.ToUInt64(args[i].Substring(2));
                                break;
                            case 'e':
                                endpos = Convert.ToUInt64(args[i].Substring(2));
                                break;
                            case 'w':
                                int j = Convert.ToInt32(args[i][2].ToString()) - 1;
                                waitmodes[j] = args[i].Length < 4;
                                if (args[i].Length > 3)
                                    waitmodes[j] = args[i][3] != 'x';
                                break;
                            case 'x':
                                string[] sizetemp = args[i].Substring(2).Split('x');
                                for (int k = 0; k < 2; k++)
                                    size[0] = Convert.ToInt32(sizetemp[0]);
                                break;
                            case 'h':
                                Console.CursorVisible = true;
                                break;
                            case 'j':
                                if (args[i].Length > 2)
                                    switch (args[i][2])
                                    {
                                        case 'h':
                                            if (args[i].Length > 6)
                                            {
                                                if (args[i].Substring(3, 4) == "rand")
                                                    invalid_hex_rand = true;
                                            }
                                            else
                                                invalid_hex = args[i].Substring(3, 2);
                                            break;
                                        case 'c':
                                            if (args[i].Length > 6)
                                            {
                                                if (args[i].Substring(3, 4) == "rand")
                                                    invalid_char_rand = true;
                                            }
                                            else
                                                invalid_char = args[i][3];
                                            break;
                                    }
                                break;
                        }
            }
            if (innums > 0)
            {
                instrs = instr.Split(fnamesep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < innums; i++)
                    datacursor += innums;
            }
            if (innum > 0)
            {
                infile = new string[innum][];
                infnames = infiles.Split(fnamesep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < infnames.Length; i++)
                    infile[i] = File.ReadAllLines(infnames[i]);
                for (int i = 0; i < infile.Length; i++)
                    datacursor += infile[i].Length;
                data = new object[endpos - startpos];
                datacursor = 0;
                for (int i = 0; i < infile.Length; i++)
                    for (int j = 0; j < infile[i].Length; j++)
                    {
                        data[datacursor] = infile[i][j]
                            .Replace("\\n", "\n").Replace("\\t", "\t");
                        datacursor++;
                    }
            }
            if (innums > 0)
                for (int i = 0; i < innums; i++)
                {
                    data[datacursor] = instrs[i]
                            .Replace("\\n", "\n").Replace("\\t", "\t");
                    datacursor++;
                }
            intv = new TimeSpan((long)(seconds * (Math.Pow(10, 7))));
            if (verboseinit) Console.WriteLine("__INITIALIZING(...);");
            if (slowinit) Thread.Sleep((int)(20 * inittime));
            Console.WindowWidth = size[0] + ((width - 16) * 5);
            Console.WindowHeight = size[1];
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            if (slowinit) Thread.Sleep((int)(10 * inittime));
            if (verboseinit) Console.WriteLine("Loading/creating data.....");
            if (slowinit) Thread.Sleep((int)(80 * inittime));
            if (verboseinit) Console.WriteLine("Creating table.....");
            if (slowinit) Thread.Sleep((int)(20 * inittime));
            Console.Clear();
            if (slowinit) Thread.Sleep((int)(40 * inittime));
            Console.CursorTop = 1;
            Console.CursorLeft = 6 + tableoff;
            if (slowinit) Thread.Sleep((int)(10 * inittime));
            for (int i = 0; i < width; i++)
            {
                Console.Write(i.ToString("X2") + ' ');
                if (slowinit) Thread.Sleep((int)(5 * inittime));
            }
            Console.CursorTop = 3;
            Console.CursorLeft = 4 + tableoff;
            Console.Write('+');
            Console.Write(new string('-', (width * 3) + 2) + ' ' + new string('-', width));
            if (slowinit) Thread.Sleep((int)(20 * inittime));
            Console.CursorTop = 3;
            Console.CursorLeft = 5 + tableoff;
            for (int i = 0; i < size[1] - 4; i++)
            {
                Console.CursorTop++;
                Console.CursorLeft--;
                Console.Write('|');
                if (slowinit) Thread.Sleep((int)(5 * inittime));
            }
            addr = startpos;
            while (true)
            {
                if (waitmodes[0])
                    Thread.Sleep(intv);
                if (waitmodes[1])
                    Thread.SpinWait(its);
                if (waitmodes[2])
                    for (ulong l = 0; l < forbuffer; l++) { }
                rand = random.Next();
                Console.CursorTop = 3;
                {
                    int j = 0;
                    for (int i = 0; i < (size[1] - 4) / 2; i++)
                    {
                        Console.CursorTop += 2;
                        Console.CursorLeft = 3;
                        if ((((addr + (ulong)(i * width))) < endpos))
                            Console.Write((addr + (ulong)(i * width)).ToString("X8"));
                        else
                        {
                            Console.Write(((startpos) + (ulong)(j * width)).ToString("X8"));
                            j++;
                        }
                        if (slowinit) Thread.Sleep((int)(5 * inittime));
                    }
                    //Console.ReadKey();
                }
                Console.CursorTop = 3;
                for (int i = 0; i < (size[1] - 4) / 2; i++)
                {
                    Console.CursorTop += 2;
                    Console.CursorLeft = 6 + tableoff;
                    for (int j = 0; j < width; j++)
                    {
                        if (data.Length > (int)(addr - startpos + (ulong)j + (ulong)(i * width)) &&
                            real((addr - startpos + (ulong)j + (ulong)(i * width))))
                            Console.Write(data[addr - startpos + (ulong)j + (ulong)(i * width)].ToString("X2"));
                        else
                        {
                            if (invalid_hex_rand)
                            {
                                Console.Write((char)random.Next(14, 255));
                                Console.Write((char)random.Next(14, 255));
                            }
                            else
                                Console.Write(invalid_hex);
                        }
                        Console.Write(' ');
                        if (slowinit) Thread.Sleep((int)(2 * inittime));
                    }
                    Console.Write("  ");
                    for (int j = 0; j < width; j++)
                    {
                        if (data.Length > (int)(addr - startpos + (ulong)j + (ulong)(i * width)) &&
                            real((addr - startpos + (ulong)j + (ulong)(i * width))))
                            Console.Write((char)data[addr - startpos + (ulong)j + (ulong)(i * width)]);
                        else
                        {
                            if (invalid_char_rand)
                                Console.Write((char)random.Next(14, 255));
                            else
                                Console.Write(invalid_char);
                        }
                        if (slowinit) Thread.Sleep((int)(2 * inittime));
                    }
                }
                if (Console.CursorVisible)
                {
                    Console.CursorLeft = 2 + tableoff + (3 * random.Next(width));
                    Console.CursorTop = 3 + (2 * random.Next(width));
                }
                addr += (ulong)jumpsize;
                if (addr > endpos && jumpsize > 0)
                    addr -= (endpos - startpos);
                if (addr < startpos && jumpsize < 0)
                    addr += endpos;
                slowinit = false;
            }
        }
    }
}
