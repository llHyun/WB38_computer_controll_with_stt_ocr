//Macro.cs

namespace Client3.Member
{
    internal class Macro
    {
        public string Cmd { get; set; }

        public string Key { get; set; }

        public Macro(string cmd, string key)
        {
            Cmd = cmd;
            Key = key;
        }
    }
}
