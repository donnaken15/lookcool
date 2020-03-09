using System;
using System.IO;
using System.Threading;

namespace conabuse
{
    class Program
    {
        static TimeSpan intv;
        static int randseed = (int)DateTime.Now.Ticks, innum = 0, innums = 0, datacursor = 0,
            its = 1000000, rand, startpos = 100, glitchcount = 10;
        static int[] curpos;
        static double seconds = 0.01, freq = 0.4, glitchintensity = 0;
        static Random random = new Random(randseed);
        static ulong forbuffer = 100000000;
        static bool[] waitmodes = new bool[]
            { true, false, false };
        static bool format = false, ned = false, multicolor, multicolorbk;
        static object retn;
        static object[] data, formatdata;
        static byte[] randbytearr;
        static string[][] infile;
        static string[] infnames, instrs, fnamesep = { "::\\\\//<<>>\"\"??^^**" };
        static string infiles, instr;

        // Thread.SpinWait(1000);

        // 1 second = 10000000 ticks

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
                            case 't':
                                instr += args[i].Substring(2) + fnamesep[0];
                                innums++;
                                break;
                            case 'f':
                                freq = Convert.ToDouble(args[i].Substring(2));
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
                                startpos = Convert.ToInt32(args[i].Substring(2));
                                break;
                            case 'n':
                                if (args[i].Substring(2) == "ed")
                                    ned = true;
                                break;
                            case 'm':
                                if (args[i].Substring(2) == "c")
                                    multicolor = true;
                                if (args[i].Substring(2) == "cb")
                                    multicolorbk = true;
                                break;
                            case 'w':
                                int j = Convert.ToInt32(args[i][2].ToString()) - 1;
                                waitmodes[j] = args[i].Length < 4;
                                if (args[i].Length > 3)
                                    waitmodes[j] = args[i][3] != 'x';
                                break;
                            case 'g':
                                if (args[i].Length > 3)
                                    switch (args[i][2])
                                    {
                                        case 'i':
                                            glitchintensity = Convert.ToDouble(args[i].Substring(3));
                                            break;
                                        case 'c':
                                            glitchcount = Convert.ToInt32(args[i].Substring(3));
                                            break;
                                    }
                                break;
                        }
                if (args[i] == "-r")
                {
                    format = true;
                    if (args[i].Length > 2)
                        formatdata = new object[Convert.ToInt32(args[i].Substring(3))];
                    else
                        formatdata = new object[8];
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
                data = new object[datacursor];
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
            Console.CursorTop = startpos;
            if (glitchintensity > 0)
                curpos = new int[2];
            intv = new TimeSpan((long)(seconds * (Math.Pow(10, 7))));
            while (true)
            {
                if (waitmodes[0])
                    Thread.Sleep(intv);
                if (waitmodes[1])
                    Thread.SpinWait(its);
                if (waitmodes[2])
                    for (ulong l = 0; l < forbuffer; l++) { }
                rand = random.Next();
                for (int i = 0; i < glitchcount; i++)
                    if (rand < (glitchintensity * int.MaxValue))
                    {
                        try
                        {
                            curpos[0] = Console.CursorLeft;
                            curpos[1] = Console.CursorTop;
                            Console.CursorLeft = random.Next(Console.WindowLeft,
                                Console.WindowLeft + Console.WindowWidth);
                            Console.CursorTop = random.Next(Console.WindowTop,
                                Console.WindowTop + Console.WindowHeight);
                            Console.Write((char)random.Next(255));
                            Console.CursorLeft = curpos[0];
                            Console.CursorTop = curpos[1];
                        }
                        catch (Exception e) { if (!ned) retn = e; }
                    }
                {
                    if (multicolor)
                        Console.ForegroundColor = (ConsoleColor)random.Next(1,16);
                    int bkc = random.Next(16);
                    while (bkc == (int)Console.ForegroundColor)
                        bkc = random.Next(16);
                    if (multicolorbk)
                        Console.BackgroundColor = (ConsoleColor)bkc;
                }
                try
                {
                    retn = data[random.Next(data.Length)];
                }
                catch (Exception e) { if (!ned) retn = e; }
                if (rand < (freq * int.MaxValue))
                    if (!format)
                        try { Console.WriteLine(retn); }
                        catch (Exception e) { if (!ned) Console.WriteLine(e); }
                    else
                    {
                        for (int i = 0; i < formatdata.Length; i++)
                        {
                            switch(random.Next(24))
                            {
                                case 0:
                                    randbytearr = new byte[random.Next(256)];
                                    random.NextBytes(randbytearr);
                                    formatdata[i] = BitConverter.ToString(randbytearr).Replace('-',',');
                                    break;
                                case 1:
                                    formatdata[i] = Convert.ToBoolean(random.Next(2));
                                    break;
                                case 2:
                                    try { formatdata[i] = new DateTime(random.Next(9999), random.Next(12), random.Next(31), random.Next(23), random.Next(59), random.Next(59)); }
                                    catch { formatdata[i] = random.Next(); } break;
                                case 3:
                                    formatdata[i] = Convert.ToDecimal(random.NextDouble());
                                    break;
                                case 4:
                                    formatdata[i] = random.NextDouble();
                                    break;
                                case 5:
                                    formatdata[i] = Convert.ToInt16(random.Next(short.MaxValue));
                                    break;
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                    formatdata[i] = random.Next().ToString("X");
                                    break;
                                default:
                                    formatdata[i] = random.Next();
                                    break;
                            }
                        }
                        try
                        {
                            Console.WriteLine(
                            System.Text.RegularExpressions.Regex.Replace(retn.ToString(),
                                "%.*\\w", "{" + random.Next(16) + "}",
                                System.Text.RegularExpressions.RegexOptions.Multiline)
                                /*retn.ToString()
                                .Replace("%s", "{" + random.Next(16) + "}")
                                .Replace("%x", "{" + random.Next(16) + "}")
                                .Replace("%d", "{" + random.Next(16) + "}")
                                .Replace("%i", "{" + random.Next(16) + "}")
                                .Replace("%u", "{" + random.Next(16) + "}")
                                .Replace("%o", "{" + random.Next(16) + "}")
                                .Replace("%X", "{" + random.Next(16) + "}")
                                .Replace("%f", "{" + random.Next(16) + "}")
                                .Replace("%F", "{" + random.Next(16) + "}")
                                .Replace("%e", "{" + random.Next(16) + "}")
                                .Replace("%E", "{" + random.Next(16) + "}")
                                .Replace("%g", "{" + random.Next(16) + "}")
                                .Replace("%G", "{" + random.Next(16) + "}")
                                .Replace("%a", "{" + random.Next(16) + "}")
                                .Replace("%A", "{" + random.Next(16) + "}")
                                .Replace("%c", "{" + random.Next(16) + "}")
                                .Replace("%p", "{" + random.Next(16) + "}")
                                .Replace("%n", "{" + random.Next(16) + "}")
                                .Replace("%hh", "{" + random.Next(16) + "}")
                                .Replace("%h", "{" + random.Next(16) + "}")
                                .Replace("%ll", "{" + random.Next(16) + "}")
                                .Replace("%l", "{" + random.Next(16) + "}")
                                .Replace("%j", "{" + random.Next(16) + "}")
                                .Replace("%z", "{" + random.Next(16) + "}")
                                .Replace("%t", "{" + random.Next(16) + "}")
                                .Replace("%L", "{" + random.Next(16) + "}")
                                .Replace("%x", "{" + random.Next(16) + "}")
                                .Replace("%x", "{" + random.Next(16) + "}")*/
                                ,
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)],
                                formatdata[random.Next(formatdata.Length)]);
                        }
                        catch (Exception e) { if (!ned) Console.WriteLine(e); }
                    }
            }
        }
    }
}
