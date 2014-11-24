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

        //main foreground colors
        private readonly ConsoleColor _foregroundColor;
        private readonly ConsoleColor _foregroundActiveColor;

        //foreground colors for files
        private readonly ConsoleColor _foregroundFileColor;
        private readonly ConsoleColor _foregroundFileActiveColor;
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

            //Foreground file colors
            _foregroundFileColor = _parentFrame.ForegroundFileColor;
            _foregroundFileActiveColor = _parentFrame.ForegroundActiveFileColor;

            FillItems();
        }

        private void FillItems()
        {            
            try
            {
                var directories = Directory.GetDirectories(_currentPath.ToString());
                var files = Directory.GetFiles(_currentPath.ToString());                
                _items.Clear();
                foreach (var directory in directories)
                {
                    _items.Add(new Item(Path.GetFileName(directory), PType.Directory));
                }
                foreach (var file in files)
                {
                    _items.Add(new Item(Path.GetFileName(file), PType.File));
                }
            }
            catch (UnauthorizedAccessException exception)
            {
                throw;
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
                    //Rewrite panel with empty strings 
                    //if items in current directory less than in previous one
                    DrawEmptyString(i);
            }
        }

        private void DrawItem(int position, bool isActive)
        {            
            if (position < _items.Count && _items.Count != 0)
            {
                var currentItem = _items[position + _topShift];
                Console.SetCursorPosition(_windowRect.Left, _windowRect.Top + position);
                if (currentItem.Type == PType.Directory)
                {
                    Console.BackgroundColor = isActive ? _backgroundActiveColor : _backgroundColor;
                    Console.ForegroundColor = isActive ? _foregroundActiveColor : _foregroundColor;
                }
                else
                {
                    Console.BackgroundColor = isActive ? _backgroundActiveColor : _backgroundColor;
                    Console.ForegroundColor = isActive ? _foregroundFileActiveColor : _foregroundFileColor;
                }
                Console.Write("{0," + (-_windowRect.Width + 1) + "} ", currentItem.Path);
            }
        }

        private void DrawEmptyString(int position)
        {
            Console.SetCursorPosition(_windowRect.Left, _windowRect.Top + position);
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _foregroundColor;
            Console.Write("{0," + (-_windowRect.Width + 1) + "} ", " ");
        }



        public void SetCursorPosition(int i)
        {
            _cursorPosition = i;
            DrawItem(i, true);
        }

        public void OnDownArrowHandler()
        {
            //if we are fits in window
            if (_cursorPosition < _windowRect.Height - 1 && _cursorPosition < _items.Count - 1)
            {
                DrawItem(_cursorPosition, false);
                SetCursorPosition(++_cursorPosition);
            }
            //if we should show files that are not fits the window
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
                try
                {
                    FillItems();
                    Draw();
                    SetCursorPosition(0);
                }
                catch (UnauthorizedAccessException exception)
                {
                    //change the current path back                
                    _currentPath = new StringBuilder(Directory.GetParent(_currentPath.ToString()).ToString());
                    return;
                }                
            }
        }

        public void OnBackspaceHandler()
        {
            if (Directory.GetParent(_currentPath.ToString()) != null)
            {
                var fullPath = Directory.GetParent(_currentPath.ToString()).ToString();
                _currentPath = new StringBuilder(fullPath);
                FillItems();
                _topShift = 0;
                Draw();
                SetCursorPosition(0);
            }
        }

        public void Disable()
        {
            DrawItem(_cursorPosition,false);
        }

        public void Enable()
        {
            SetCursorPosition(_cursorPosition);
        }
    }
}