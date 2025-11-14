namespace Service
{
    public class WordObject : TargetObject
    {
        public bool Verify { get; set; }
        public uint Start { get; set; }
        public uint Length { get; set; }
        public uint Value { get; set; }
        public ProgramWordRegular Regular { get; set; }

        public WordObject()
        {
            Type = ProgramFileType.Word;
            Verify = false;
            Start = 0U;
            Length = 4096U /*0x1000*/;
            Value = uint.MaxValue;
            Regular = ProgramWordRegular.None;
        }
    }
}
