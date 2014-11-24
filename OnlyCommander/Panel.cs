using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace OnlyCommander
{
    internal class Panel
    {
        private MainFrame _parentFrame;
        private Rectangle _windowRect;
        private List<Item> _items;
        private StringBuilder _currentPath;
        private int _cursorPosition;
        private int _topShift;

        private readonly ConsoleColor _backgroundColor;
        private readonly ConsoleColor _backgroundActiveColor;
        private readonly ConsoleColor _foregroundColor;
        private readonly ConsoleColor _foregroundActiveColor;
        public Panel(MainFrame parentFrame, Rectangle windowRect)
        {
            _parentFrame = parentFrame;
            _windowRect = windowRect;
            _items = new List<Item>();
            _currentPath = new StringBuilder(Directory.GetLogicalDrives()[0]);
            _cursorPosition = 0;
            _topShift = 0;

            //Standard colors
            _backgroundColor = _parentFrame.BackgroundColor;
            _foregroundColor = _parentFrame.ForegroundColor;

            //Active colors
            _backgroundActiveColor = _parentFrame.BackgroundActiveColor;
            _foregroundActiveColor = _parentFrame.ForegroundActiveColor;

            FillItems();
        }

        private void FillItems()
        {
            _items.Clear();
            foreach (var directory in Directory.GetDirectories(_currentPath.ToString()))
            {
                _items.Add(new Item(Path.GetFileName(directory), PType.Directory));
            }
            foreach (var file in Directory.GetFiles(_currentPath.ToString()))
            {
                _items.Add(new Item(Path.GetFileName(file), PType.File));
            }
        }

        public void Draw()
        {
            //this._windowRect.DrawFrame('?');            
            for (int i = 0; i < _windowRect.Height; i++)
            {
                if (i < _items.Count)
                {
                    DrawItem(i, false);
                }
                else
                    DrawEmptyString(i);
            }
        }

        private void DrawItem(int position, bool isActive)
        {
            if (position < _items.Count && _items.Count != 0)
            {
                Console.SetCursorPosition(_windowRect.Left, _windowRect.Top + position);
                Console.BackgroundColor = isActive ? _backgroundActiveColor : _backgroundColor;
                Console.ForegroundColor = isActive ? _foregroundActiveColor : _foregroundColor;
                Console.Write(String.Format("{0," + (-_windowRect.Width + 1) + "} ", _items[position + _topShift].Path));
            }
        }

        private void DrawEmptyString(int position)
        {
            Console.SetCursorPosition(_windowRect.Left, _windowRect.Top + position);
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _foregroundColor;
            Console.Write(String.Format("{0," + (-_windowRect.Width + 1) + "} ", " "));
        }



        public void SetCursorPosition(int i)
        {
            _cursorPosition = i;
            DrawItem(i, true);
        }

        public void OnDownArrowHandler()
        {
            if (_cursorPosition < _windowRect.Height - 1 && _cursorPosition < _items.Count - 1)
            {
                DrawItem(_cursorPosition, false);
                SetCursorPosition(++_cursorPosition);
            }
            else if (_cursorPosition + _topShift < _items.Count - 1)
            {
                _topShift++;
                Draw();
                SetCursorPosition(_cursorPosition);
            }
        }

        public void OnUpArrowHandler()
        {
            if (_cursorPosition > 0)
            {
                DrawItem(_cursorPosition, false);
                SetCursorPosition(--_cursorPosition);
            }
            else if (_topShift > 0)
            {
                _topShift--;
                Draw();
                SetCursorPosition(_cursorPosition);
            }
        }

        public void OnEnterHandler()
        {
            if (_items[_cursorPosition].Type == PType.Directory)
            {
                string fullPath = Path.Combine(_currentPath.ToString(), _items[_cursorPosition].Path);
                _currentPath = new StringBuilder(fullPath);
                FillItems();
                Draw();
                SetCursorPosition(0);
            }
        }

        public void OnBackspaceHandler()
        {
            if (Directory.GetParent(_currentPath.ToString()) != null)
            {
                string fullPath = Directory.GetParent(_currentPath.ToString()).ToString();
                _currentPath = new StringBuilder(fullPath);
                FillItems();
                _topShift = 0;
                Draw();
                SetCursorPosition(0);
            }
        }
    }
}