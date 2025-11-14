using Newtonsoft.Json;
using System.IO;

namespace Service
{
    public class SessionMgr
    {
        private static SessionMgr _instance;

        public SessionMgr()
        {
            if (!File.Exists(PathMgr.SessionFile))
                return;
            Session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(PathMgr.SessionFile));
        }

        public static SessionMgr Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SessionMgr();
                return _instance;
            }
        }

        public Session Session { get; protected set; } = new Session();

        public void SaveSession()
        {
            File.WriteAllText(PathMgr.SessionFile, JsonConvert.SerializeObject(Session, Formatting.Indented));
        }
    }
}
