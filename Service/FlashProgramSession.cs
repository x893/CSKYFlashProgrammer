using System;
using System.Collections.Generic;

namespace Service
{

    public class FlashProgramSession : ICloneable
    {
        public FlashProgramSession()
        {
            CommandType = CommandType.Write;
            TargetConfig = new TargetConfig();
            TargetFile = new TargetFile();
            TargetObjectArray = new List<TargetObject>();
        }

        public CommandType CommandType { get; set; }

        public TargetConfig TargetConfig { get; protected set; }

        public TargetFile TargetFile { get; protected set; }

        public List<TargetObject> TargetObjectArray { get; set; }

        public object Clone()
        {
            FlashProgramSession flashProgramSession = new FlashProgramSession();
            flashProgramSession.TargetConfig = TargetConfig.Clone() as TargetConfig;
            flashProgramSession.TargetFile = TargetFile.Clone() as TargetFile;
            flashProgramSession.CommandType = CommandType;
            foreach (TargetObject targetObject in TargetObjectArray)
                flashProgramSession.TargetObjectArray.Add(targetObject.Clone() as TargetObject);
            return flashProgramSession;
        }
    }
}
