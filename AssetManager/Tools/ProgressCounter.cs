using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManager
{
    public class ProgressCounter
    {

        #region "Fields"

        private int _currentTick;
        private int _progBytesMoved;
        private int _progTotalBytes;
        private int _speedBytesMoved;
        private double _speedThroughput;

        private int _startTick;
        #endregion

        #region "Constructors"

        public ProgressCounter()
        {
            _progBytesMoved = 0;
            _progTotalBytes = 0;
            _speedBytesMoved = 0;
            _currentTick = 0;
            _startTick = 0;
            _speedThroughput = 0;
        }

        #endregion

        #region "Properties"

        public int BytesMoved
        {
            get { return _progBytesMoved; }
            set
            {
                _speedBytesMoved += value;
                _progBytesMoved += value;
            }
        }

        public int BytesToTransfer
        {
            get { return _progTotalBytes; }
            set { _progTotalBytes = value; }
        }

        public int Percent
        {
            get
            {
                if (_progTotalBytes > 0)
                {
                    return (int)Math.Round((((float)_progBytesMoved / (float)_progTotalBytes) * 100), 0);
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Throughput
        {
            get { return _speedThroughput; }
        }

        #endregion

        #region "Methods"

        public void ResetProgress()
        {
            _progBytesMoved = 0;
        }

        public void Tick()
        {
            _currentTick = Environment.TickCount;
            if (_startTick > 0)
            {
                if (_speedBytesMoved > 0)
                {
                    double elapTime = _currentTick - _startTick;
                    _speedThroughput = Math.Round((_speedBytesMoved / elapTime) / 1000, 2);
                }
            }
            else
            {
                _startTick = _currentTick;
            }
        }

        #endregion

    }
}