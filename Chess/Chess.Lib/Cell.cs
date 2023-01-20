namespace Chess.Lib
{
    public struct Cell
    {
        public Cell() { }
        public int Number { get; set; } = 0;
        public string Coordinate { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
    }
}