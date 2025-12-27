using System;
using System.IO;
using System.Text;

public class DataDumper
{
    private static byte[] L; 
    private static int M;    

    private static int[] aH = new int[28]; 
    private static int aG;  
    private static int[] l = new int[4]; 
    private static bool aI;
    private static long ay, az, aA, aB;
    private static int aC, aD, aE, aF;

    private static byte[] h; 
    private static int[] G, H, I;
    private static int[][] av;
    private static string[] au; 
    private static int[] aw;
    private static int ax;
    private static int n = 0; 

    private static int[] at = new int[] { 2, 3, 4 };

    public static void PrintByteArray(byte[] bytes)
    {
        var sb = new StringBuilder("new byte[] { ");
        foreach (var b in bytes)
        {
            sb.Append(b + ", ");
        }
        sb.Append("}");
        Console.WriteLine(sb.ToString());
    }



    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        if (args.Length == 0)
        {
            Console.WriteLine("usage: <file_path>");
            return;
        }

        try
        {
            L = File.ReadAllBytes(args[0]);
            M = 0;

            ag();

            L_load();

            // 3. Dump the strings
            Console.WriteLine("\n--- DUMPING STRINGS ---");
            int count = 0;
            if (au != null)
            {
                foreach (string s in au)
                {
                    if (s != null)
                    {
                        Console.WriteLine($"[{count:D4}]: {s}");
                        count++;
                    }
                }
            }
            Console.WriteLine($"--- Dumped {count} strings. ---");

        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }

    private static int v()
    {
        return L[M++] & 0xFF;
    }


    private static void ag()
    {
        aG = 0;
        for (int var0 = 0; var0 < 28; var0++)
        {
            aH[var0] = v();
        }

        System.Array.Copy(aH, aG, l, 0, l.Length);
        aG = aG + l.Length;
        aI = l[2] == 68;
        l[2] = 68;
        ay = ai();
        az = ai();
        aA = ai();
        aB = ai();
        aC = aj();
        aD = aj();
        aE = aj();
        aF = aj();
    }


    private static long ai()
    {
        //long var0 = ((long)(aH[aG + 3] & 0xFF) << 24) |
        //              ((long)(aH[aG + 2] & 0xFF) << 16) |
        //              ((long)(aH[aG + 1] & 0xFF) << 8) |
        //              ((long)(aH[aG] & 0xFF));
        long var0 = ((long)(aH[aG + 3] & 0xFF)) |
                  ((long)(aH[aG + 2] & 0xFF) << 8) |
                  ((long)(aH[aG + 1] & 0xFF) << 16) |
                  ((long)(aH[aG] & 0xFF) << 24);
        aG += 4;
        return var0;
    }

    private static int aj()
    {
        //int var0 = ((aH[aG + 1] & 0xFF) << 8) | (aH[aG] & 0xFF);
        int var0 = ((aH[aG + 1] & 0xFF)) | ((aH[aG] & 0xFF) << 8);

        aG += 2;
        return var0;
    }

    /// <summary>
    /// Prints the header info (debug method).
    /// </summary>
    private static void ah()
    {
        string var0 = "HEADER PRINT ---------------------------\n";
        var0 = var0 + "magic       = {" + l[0] + ", " + l[1] + ", " + l[2] + ", " + l[3] + "};" + "\n";
        var0 = var0 + "version     = " + ay + "\n";
        var0 = var0 + "entry       = " + az + "\n";
        var0 = var0 + "codesize    = 0x" + aA.ToString("X") + " = " + aA + "\n";
        var0 = var0 + "datasize    = 0x" + aB.ToString("X") + " = " + aB + "\n";
        var0 = var0 + "tempVarNum  = " + aC + "\n";
        var0 = var0 + "bssVarNum   = " + aD + "\n";
        var0 = var0 + "globalNum   = " + aE + "\n";
        var0 = var0 + "sharedNum   = " + aF + "\n";
        var0 = var0 + "---------------------------------------\n\n";
        Console.WriteLine(var0);
    }


    private static void L_load()
    {
        Console.WriteLine((int)aA);
        n_skip((int)aA); // Skip "code" section
        Console.WriteLine("meow", (int)M);
        Console.WriteLine("ok; ", L.Length);
        Console.WriteLine("hey: ", (int)aB);
        h = new byte[(int)aB]; 
        b_copy(h);       
        ah(); 

        G = new int[aD];
        for (int var0 = 0; var0 < G.Length; var0++)
        {
            G[var0] = A_readInt();
        }

        H = new int[aF];
        for (int var1 = 0; var1 < H.Length; var1++)
        {
            H[var1] = A_readInt();
        }

        I = new int[aE];
        for (int var2 = 0; var2 < I.Length; var2++)
        {
            I[var2] = A_readInt();
        }

        // Process the loaded data to find strings
        Console.WriteLine(h.Length);
        PrintByteArray(h[4134..4150]);
        a_process(h, G, H, I, n);
    }


    public static int A_readInt()
    {
        //int var0 = (L[M + 3]) |
        //             ((255 & L[M + 2]) << 8) |
        //             ((255 & L[M + 1]) << 16) |
        //             (255 & L[M] << 24);
        int var0 = (L[M + 3] << 24) |
             ((255 & L[M + 2]) << 16) |
             ((255 & L[M + 1]) << 8) |
             (255 & L[M]);
        M += 4;
        return var0;
    }


    public static void b_copy(byte[] var0)
    {
        System.Array.Copy(L, M, var0, 0, var0.Length);
        M += var0.Length;
    }


    public static void n_skip(int var0)
    {
        M += var0;
    }

    private static void a_process(byte[] var0, int[] var1, int[] var2, int[] var3, int var4)
    {
        av = new int[7][];
        int[][] var5 = new int[][] { var1, var2, var3 };

        for (int var6 = 0; var6 < var5.Length; var6++)
        {
            av[at[var6]] = var5[var6];
        }

        av[6] = new int[4096];
        av[5] = new int[64];
        au = new string[4096];
        aw = new int[7];
        aw[0] = 512;
        aw[1] = var0.Length + aw[0];
        aw[2] = aw[1] + var1.Length;
        aw[3] = aw[2] + var2.Length;
        aw[4] = aw[3] + var3.Length;
        aw[5] = aw[4] + 1024;
        aw[6] = aw[5] + av[6].Length;
        ax = var0.Length;

        //Console.WriteLine(var0.Length);

        for (int var14 = 2; var14 < 7; var14++)
        {
            ax = ax + av[var14].Length;
        }

        int var15 = -1; 

        for (int var7 = 0; var7 < var5.Length; var7++)
        {
            int[] var8 = av[at[var7]]; 

            for (int var9 = 0; var9 < var8.Length; var9++)
            {
                int var10 = var8[var9]; 
                int type = (int)((uint)var10 >> 28);

                if (type == 6 || type == 5)
                {
                    int var13; 
                    int var12 = var13 = k_getOffset(var10); 

                    while (var0[++var12] != 0)
                    {
                    }

                    var8[var9] = b_pack(type, ++var15); 
                    au[var15] = Encoding.GetEncoding(932).GetString(var0, var13, var12 - var13);
                }

                if (type == 11)
                {
                    int var16;
                    int var11 = (var16 = k_getOffset(var10)) & 65535;
                    var16 >>= 16;
                    byte var18 = 4;
                    if (var16 == 1)
                    {
                        var18 = 6;
                        var11 += var4; 
                    }

                    if (var16 == 2)
                    {
                        var18 = 2;
                    }

                    if (var16 == 3)
                    {
                        var18 = 3;
                    }

                    var8[var9] = b_pack(11, o_getOffset(var18, var11));
                }
            }
        }
    }

    private static int o_getOffset(int var0, int var1)
    {
        return aw[var0 - 1] + var1;
    }

    private static int k_getOffset(int var0)
    {
        return var0 << 4 >> 4;
    }

    public static int b_pack(int var0, int var1)
    {
        return (var1 & 268435455) | (var0 << 28);
    }
}