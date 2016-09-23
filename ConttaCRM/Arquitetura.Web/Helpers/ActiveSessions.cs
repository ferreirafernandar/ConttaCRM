using System.Collections.Generic;

namespace Arquitetura.Web.Helpers
{
    public static class ActiveSessions
    {
        private static List<string> _sessionInfo;
        private static readonly object padlock = new object();

        public static List<string> Sessions
        {
            get
            {
                lock (padlock)
                {
                    if (_sessionInfo == null)
                    {
                        _sessionInfo = new List<string>();
                    }
                    return _sessionInfo;
                }
            }
        }

        public static int Count
        {
            get
            {
                lock (padlock)
                {
                    if (_sessionInfo == null)
                    {
                        _sessionInfo = new List<string>();
                    }
                    return _sessionInfo.Count;
                }
            }
        }
    }
}