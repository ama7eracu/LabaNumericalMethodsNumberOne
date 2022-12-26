using System;
namespace LanaNumericalMethods
{
    public class Algorithm
    {
        private int _N;
        private readonly double[] _x;
        private readonly double[] _y;
        private double[] _X;
        private double _A;
        private double _B;
        private double[] _h;
        private int IER;
        private double[] _F;
        private double[] _tau;
        private double[] _z;
        private double[] _a;
        private double[] _b;
        private double[] _c;
        private double[] _d;
        private double[] _Bdiff;
        private double _b0;

        public Algorithm(string path, string? newPath)
        {
            WorkWithFile.Path = path;

            WorkWithFile.NewPath = newPath;

            WorkWithFile.ReadFromFile(ref _x, ref _y, ref _A, ref _B, ref _N, ref IER);

            Initialization();

            CalcH();

            CalcF();

            CalcCoeffMatrix();

            CalcX();

            CalcTau();

            CalcZ();

            CalcD();

            CalcBdiff();
            
            WorkWithFile.FillFile(_x, _y,_z, IER);
        }

        private void Initialization()
        {
            _h = new double[_N - 1];
            _F = new double[_N];
            _X = new double[_N];
            _tau = new double[_N];
            _z = new double[_N];
            _a = new double[_N - 1];
            _b = new double[_N - 1];
            _c = new double[_N];
            _Bdiff = new double[_N - 1];

        }

        private void CalcH()
        {
            
            for (var i = 0; i < _h.Length; i++)
            {
                _h[i] = _x[i+1] - _x[i];
                
            }

        }

        private void CalcF()
        {

            _F[0] = _A;
            // F[N - 1] = (3 / h[N - 2]) * (B - (y[N - 1] - y[N - 2]) / h[N - 2]);
            _F[_N - 1] = _B;
            for (int i = 1; i < _N - 1; i++)
            {
                _F[i] = 6 * (((_y[i + 1] - _y[i]) / _h[i]) - ((_y[i] - _y[i - 1]) / _h[i-1]));
                
            }

        }

        private void CalcCoeffMatrix()
        {
            _c[0] = 1;
            _c[_N - 1] = 1;
            _b[0] = 0;
            _a[_N - 2] = 0;
            for (int i = 1; i < _N - 1; i++)
            {
                _c[i] = 2 * (_h[i-1] + _h[i]);
                
            }
            for (int i = 1; i < _N - 1; i++)
            {
                _b[i] = _h[i];
            }
            for (int i = 0; i < _N - 2; i++)
            {
                _a[i] = _h[i];
            }


        }

        private void CalcX()
        {
            _X[0] = -((_b[0]) / _c[0]);
            for (int i = 1; i < _N - 1; i++)
            {
                _X[i] = -_b[i] / (_c[i] + _a[i - 1] * _X[i - 1]);
            }
            _X[_N - 1] = -(_a[_N - 2] / _c[_N - 1]);
        }

        private void CalcTau()
        {
            _tau[0] = _F[0] / _c[0];
            for (int i = 1; i < _N - 1; i++)
            {
                _tau[i] = (_F[i] - (_a[i - 1] * _tau[i - 1])) / (_c[i] + _a[i - 1] * _X[i - 1]);
            }
            _tau[_N - 1] = (_F[_N - 1] / _c[_N - 1]);
        }

        private void CalcZ()
        {
            _z[_N - 1] = (_tau[_N - 1] + _X[_N - 1] * _tau[_N - 2]) / (1 - _X[_N - 1] * _X[_N - 2]);
            for (int i = _N - 2; i > -1; i--)
            {
                _z[i] = _tau[i] + _X[i] * _z[i + 1];
            }
        }

        private void CalcD()
        {
            _d = new double[_N - 1];
            int dIter = 1;
            for (int i = 0; i < _N - 1; i++)
            {
                _d[i] = (_z[i + 1] - _z[i]) / _h[i];
                dIter++;
            }
        }

        private void CalcBdiff()
        {
            for (int i = 0; i < _N - 1; i++)
            {
                _Bdiff[i] = (((_h[i] / 2) * _z[i + 1]) - ((_h[i] * _h[i]) / 6) * _d[i] + (_y[i + 1] - _y[i]) / _h[i]);

            }
            _b0 = _Bdiff[0] - (_h[0] * _z[1]) + ((_h[0] * _h[0]) / 2 * _d[1]);
        }

        
    }
}

