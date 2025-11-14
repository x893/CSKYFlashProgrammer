namespace Service
{
    public class HexObject : ElfObject
    {
        public HexObject()
        {
            Type = ProgramFileType.Hex;
            Verify = false;
            FilePath = "";
        }
    }
}
