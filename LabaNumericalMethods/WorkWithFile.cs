
using System.Globalization;

namespace LanaNumericalMethods
{
    public static class WorkWithFile
    { 
        public static string? Path { set; get; }

        public static string? NewPath { set; get; }

        private static void CheckX(double[] X,ref int IER)
        {
            for (var i = 0; i < X.Length - 1; i++)
            {
                if (X[i] != X[i + 1] && !(X[i] > X[i + 1])) continue;
                IER = 2;
                using StreamWriter writer = new StreamWriter(NewPath, false);
                writer.WriteLine($"IER={IER}");
                throw new Exception($"IER={IER}");

            }
        }

        private static void CheckSizeEqualXAndY(double[] X, double[] Y)
        {
            if (X.Length != Y.Length)
            {
                throw new Exception("Размерность x!=Размерности Y");
            }
        }

        private static void CheckSize(int n,ref int IER)
        {
            if (n > 2) return;
            IER = 1;
            using StreamWriter writer = new StreamWriter(NewPath, false);
            writer.WriteLine($"IER={IER}");
            throw new Exception("IER=1");
        }

        private static string FillCollection(string name,double[] collection)
        {
            string coll = name;
            coll += "=";
            foreach(var i in collection)
            {
                coll += '[';
                coll += i.ToString(CultureInfo.CurrentCulture);
                coll += ']';
                coll += ' ';
            }
            return coll;
        }

        public static void ReadFromFile(ref double[] X, ref double[] Y,ref double A,ref double B, ref int n, ref int IER)
        {
            using StreamReader reader = new StreamReader(Path);
            var nLine = reader.ReadLine();
            string aLine = reader.ReadLine();
            aLine = aLine?.Substring(2);
            string bLine = reader.ReadLine();
            bLine = bLine?.Substring(2);
            nLine = nLine?.Substring(2);
            var xLine = reader.ReadLine();
            xLine = xLine?.Substring(2);
            var yLine = reader.ReadLine();
            yLine = yLine?.Substring(2);
            n = int.Parse(nLine)+1;
            CheckSize(n,ref IER);
            A=double.Parse(aLine);
            B=double.Parse(bLine);
            X = xLine?.Split(' ').Select(double.Parse).ToArray();
            CheckX(X, ref IER);
            //CheckAscending(X, ref IER);
            Y = yLine.Split(' ').Select(double.Parse).ToArray();
            CheckSizeEqualXAndY(X,Y);
        }

        public static void FillFile(double[]X,double[] Y,double[] Z,int IER)
        {
            var XX = FillCollection("X", X);
            var YY = FillCollection("Y", Y);
            var ZZ=FillCollection("f''",Z);
            using var writer = new StreamWriter(NewPath, false);
            writer.WriteLine(XX);
            writer.WriteLine(YY);
            writer.WriteLine(ZZ);
            writer.WriteLine($"IER={IER}");
        }

    }
}
