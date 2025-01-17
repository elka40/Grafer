﻿using Grafer.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Grafer.CustomControls
{
    /// <summary>
    /// Interaction logic for EquationInput.xaml
    /// </summary>
    public partial class EquationInput : TextBox
    {
        public EquationInput()
        {
            InitializeComponent();
            shortcuts = new Dictionary<string, string>();
            LoadShortcuts();
        }

        private Dictionary<string, string> shortcuts;
        //Invalidní sekce.
        public (int SelectionStart, int SelectionLength, int MessageID) InvalidSection { get; private set; } = (0, 0, -1);

        //Jestli je rovnice v pořádku.
        public bool IsEquationValid
        {
            get
            {
                bool isValid = EquationCheck.IsEquationValid(Text);

                if (!isValid)
                {
                    InvalidSection = EquationCheck.InvalidSection;
                }

                return isValid;
            }
        }

        //Při vkládání textu z klávesnice. 
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            RelationInputCheck(e);
            CloseBracket(e.Text);
        }

        //Omezení možných znaků.
        private static void RelationInputCheck(TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "[0-9 x + * / ( ) ^ √ , s i n c o s t a g ° π |]") && e.Text != "-")
            {
                e.Handled = true;
            }
        }

        //Dokončení závorky.
        private void CloseBracket(string input)
        {
            if (SelectionStart == Text.Length)
            {
                if (input == "(")
                {
                    Text += ')';
                    SelectionStart = Text.Length - 1;
                }

                if (input == "|")
                {
                    Text += "|";
                    SelectionStart = Text.Length - 1;
                }
            }
        }

        //Vyvolávní zkratky z klávesnice PreviewKeyDown event.
        private void ShortcutInput(object sender, KeyEventArgs e)
        {
            string modifier = Keyboard.Modifiers.ToString();
            string shortcut = modifier + e.Key.ToString();

            if (shortcuts.ContainsKey(shortcut))
            {
                InsertShortcut(shortcuts[modifier + e.Key.ToString()]);
            }
        }

        //Vložení zkratky do textového pole.
        public void InsertShortcut(string shortcut)
        {
            string addition = (shortcut != "π" && shortcut != "°" && shortcut != "|") ? "()" : "";

            int selectionStart = SelectionStart;

            Text = Text.Insert(SelectionStart, shortcut + addition);

            SelectionStart = selectionStart + shortcut.Length + addition.Length + ((shortcut != "π" && shortcut != "°" && shortcut != "|") ? -1 : 0);
        }

        private void LoadShortcuts()
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Data\\Shortcuts.csv";
            shortcuts = File.ReadAllLines(path).Skip(1).ToArray().ToDictionary();
        }
    }
}
