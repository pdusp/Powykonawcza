using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powykonawcza.Services
{
    public interface IImport
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="percent">0..100</param>
        void ReportProgress(double percent);
    }
}
