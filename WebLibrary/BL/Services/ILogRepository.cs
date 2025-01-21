using BL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ILogRepository
    {
        void AddLog(string message, int level);
        int GetLogCount();
        IEnumerable<Log> GetByAmount(int number);
    }

    public class LogRepository : ILogRepository
    {
        private readonly WebLibraryContext _context;

        public LogRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public void AddLog(string message, int level)
        {
            _context.Logs.Add(new Log
            {
                Message = message,
                Level = level,
                Timestamp = DateTime.Now
            });

            _context.SaveChanges();
        }

        public IEnumerable<Log> GetByAmount(int n)
        {
            return _context.Logs.Skip(GetLogCount() - n).Take(n).ToList();
        }

        public int GetLogCount() => _context.Logs.Count();
    }
}
