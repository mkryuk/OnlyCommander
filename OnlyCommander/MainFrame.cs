using System;
using System.Text;
using System.Drawing;

namespace OnlyCommander
{
    public static class RectangleExtention
    {
        public static void DrawLayout(this Rectangle rect)
        {            
            for (int j = 0; j < rect.Height; j++)
            {
                Console.Write(String.Format("{0," + (-rect.Width + 1) + "} ", " "));
            } 
        }
        public static void DrawFrame(this Rectangle rect, char symbol)
        {
            //write main frame           
            //write top & bottom borders
            for (int i = rect.Left; i < rect.Right; i++)
            {
                //write top border
                Console.SetCursorPosition(i, rect.Top);
                Console.Write(symbol);

                //write bottom border
                Console.SetCursorPosition(i, rect.Bottom - 1);
                Console.Write(symbol);
            }

            //write left & right borders
            for (int i = rect.Top; i < rect.Bottom; i++)
            {
                //write left border
                Console.SetCursorPosition(rect.Left, i);
                Console.Write(symbol);

                //write middle delimeter
                Console.SetCursorPosition(rect.Left + (rect.Right - rect.Left)/ 2, i);
                Console.Write(symbol);

                //write right border
                Console.SetCursorPosition(rect.Right - 1, i);
                Console.Write(symbol);
            }
            //end writing main frame  
        }
    }

    internal class MainFrame
    {
        public bool Working { get; set; }

        private StringBuilder _line;
        private ConsoleKeyInfo _key;        
        private Rectangle _windowRect;        
        private Panel _leftPanel;
        private Panel _rightPanel;
        private Panel _activePanel;

        public ConsoleColor BackgroundColor;
        public ConsoleColor BackgroundActiveColor;

        //main foreground colors
        public ConsoleColor ForegroundColor;
        public ConsoleColor ForegroundActiveColor;

        //foreground colors for files
        public ConsoleColor ForegroundFileColor;
        public ConsoleColor ForegroundActiveFileColor;

        public Rectangle ClientRect {
            get
            {
                var clientRect = new Rectangle(_windowRect.Location, _windowRect.Size);
                clientRect.Inflate(-1, -1);
                return clientRect;
            }
        }

        public MainFrame()
        {
            Working = true;

            //_needToRedraw = true;
            _line = new StringBuilder();
            _key = new ConsoleKeyInfo();
            _windowRect = new Rectangle(0,0,Console.LargestWindowWidth-1,Console.LargestWindowHeight-1);

            //Standard colors
            BackgroundColor = ConsoleColor.DarkCyan;
            ForegroundColor = ConsoleColor.White;

            //Active colors
            BackgroundActiveColor = ConsoleColor.DarkBlue;
            ForegroundActiveColor = ConsoleColor.Yellow;

            //File colors
            ForegroundFileColor = ConsoleColor.Cyan;
            ForegroundActiveFileColor = ConsoleColor.Yellow;

            //Create left panel
            _leftPanel = new Panel(this,
                new Rectangle(ClientRect.Left + 1, ClientRect.Top, ClientRect.Width/2 - 1, ClientRect.Height));

            //Create right panel
            _rightPanel = new Panel(this,
                new Rectangle(ClientRect.Left + ClientRect.Width/2 + 2, ClientRect.Top, ClientRect.Width/2 - 1,
                    ClientRect.Height));
            _activePanel = _leftPanel;           

            Draw();
        }

        public void DispatchMessage()
        {
            _key = Console.ReadKey(true);

            switch (_key.Key)
            {
                case ConsoleKey.Enter:
                    OnEnterHandler();
                    break;

                case ConsoleKey.UpArrow:
                    OnUpArrowHandler();
                    break;

                case ConsoleKey.DownArrow:
                    OnDownArrowHandler();
                    break;

                case ConsoleKey.Tab:
                    OnTabHandler();
                    break;

                case ConsoleKey.Backspace:
                    OnBackspaceHandler();
                    break;
            }

            if (_key.KeyChar == 'q')
            {
                Working = false;
            }
        }        

        //draw main window and palettes
        private void Draw()
        {
            //Set console size
            Console.WindowHeight = _windowRect.Height+1;
            Console.WindowWidth = _windowRect.Width;            
            
            //Set console buffer size
            Console.BufferHeight = _windowRect.Height+1;
            Console.BufferWidth = _windowRect.Width;

            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            this._windowRect.DrawLayout();
            this._windowRect.DrawFrame('#');
            _leftPanel.Draw();
            _rightPanel.Draw();
            _activePanel.SetCursorPosition(0);
        }        

        //Keyboard Handlers
        private void OnDownArrowHandler()
        {
            _activePanel.OnDownArrowHandler();
        }

        private void OnUpArrowHandler()
        {
            _activePanel.OnUpArrowHandler();
        }

        private void OnEnterHandler()
        {
            _activePanel.OnEnterHandler();
        }

        private void OnBackspaceHandler()
        {
            _activePanel.OnBackspaceHandler();
        }

        private void OnTabHandler()
        {
            _activePanel = _activePanel == _leftPanel ? _rightPanel : _leftPanel;
        }
        
    }    
}
