namespace OnlyCommander
{
    enum PType 
    {
        File = 1,
        Directory = 2
    }
    class Item
    {
        public string Path { get; set; }
        public PType Type { get; set; }

        public Item(string path = null, PType type = default(PType))
        {
            Path = path;
            Type = type;
        }
    }
}
