﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace TreeViewMenu
{
    public class MainWindowViewModel : ViewModelBase
    {

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnItemSelected(object selectedItem)
        {
            switch (selectedItem.ToString())
            {
                case "AltCoinDecrypter":
                    SelectedItem = new View_AltCoinDecrypter();
                    break;
                case "Steam":
                    SelectedItem = new View_Ach_Steam();
                    break;
                default:
                    SelectedItem = new View__Empty();
                    break;
            } 
        }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ICommand ItemSelectedCommand { get; }

        private object _SelectedItem;
        public object SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }

        public ObservableCollection<MenuItem> MenuItems { get; } = new ObservableCollection<MenuItem>();

        public MainWindowViewModel()
        {
            ItemSelectedCommand = new RelayCommand<object>(OnItemSelected);
        }
    }

    public class MenuItem : ObservableObject, IEnumerable<MenuItem>
    {
        public string Title { get; }

        public PackIconKind? Icon { get; }

        public IList<MenuItem> Children { get; } = new List<MenuItem>();

        private MenuItem Parent { get; set; }

        private bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (Set(ref _IsSelected, value))
                {
                    if (value)
                    {
                        IsExpanded = !IsExpanded;
                    }
                    else
                    {
                        for (MenuItem current = this;
                            current != null;
                            current = current.Parent)
                        {
                            current.IsExpanded = current.IsSelectionWithin;
                        }
                    }
                }
            }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get => _IsExpanded;
            set
            {
                if (Set(ref _IsExpanded, value))
                {
                    if (!value)
                    {
                        IsSelected = false;
                    }
                }
            }
        }

        public MenuItem(string title, PackIconKind icon)
            :this(title)
        {
            Icon = icon;
        }

        public MenuItem(string title)
        {
            Title = title;
        }

        private bool IsSelectionWithin => IsSelected || Children.Any(x => x.IsSelectionWithin);

        public void Add(MenuItem child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public IEnumerator<MenuItem> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}