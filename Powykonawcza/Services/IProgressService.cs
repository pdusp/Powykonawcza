namespace Powykonawcza.Services
{
    public interface IProgressService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="percent">0..100</param>
        void ReportProgress(double percent);
    }
}