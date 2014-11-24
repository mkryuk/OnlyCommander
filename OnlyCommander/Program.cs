namespace OnlyCommander
{
    class Program
    {
        private static void Main(string[] args)
        {
            var mainFrame = new MainFrame();
            while (mainFrame.Working)
            {
                mainFrame.DispatchMessage();
            }
        }
    }   
}
