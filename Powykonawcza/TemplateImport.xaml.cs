﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Powykonawcza.DAL;
using Powykonawcza.Model.Szablon;

namespace Powykonawcza
{
    /// <summary>
    ///     Interaction logic for SzablonyImportu.xaml<!--AutoGeneratedColumns="gr1_AutoGeneratedColumns" -->
    /// </summary>
    public partial class SzablonyImportu : Window
    {
        public SzablonyImportu()
        {
            InitializeComponent();
            gr1.ItemsSource             =  PopulateSzablon();
            GridItems.CollectionChanged += GridItems_CollectionChanged;
        }

        public IEnumerable<SzablonItem> CurrentTemplate()
        {
            var rows        = GetDataGridRows(gr1);
            var itemsSource = gr1.ItemsSource;

            for (var i = 0; i < GridItems.Count; i++)
                if (GridItems[i].Import)
                    yield return GridItems[i];
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }


        private void clik_zapisz(object sender, RoutedEventArgs e)
        {
            var rows        = GetDataGridRows(gr1);
            var itemsSource = gr1.ItemsSource;
            //
            foreach (var itm in GridItems)
                if ((itm.Name == "point" || itm.Name == "x" || itm.Name == "y" || itm.Name == "h") &&
                    itm.Import == false)
                {
                    MessageBox.Show("Pola pkt,X,Y,Z są zawsze wymagane do importu");
                    return;
                }

            //
            try
            {
                SaveSzablon();
                MessageBox.Show("Zapis wykonany");
            }
            catch
            {
                MessageBox.Show("Problem z zapisem pliku szablonu");
            }

            btnzapisz.IsEnabled = false;
            Close();
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // if (MessageBox.Show("Close?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            Close();
        }

        private void gr1_AutoGeneratedColumns(object sender, EventArgs e)
        {
        }

        private void gr1_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            btnzapisz.IsEnabled = true;
        }

        private void gr1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }

        private void GridItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            btnzapisz.IsEnabled = true;
        }

        private void MenuItem_Clickdown(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var itm = (SzablonItem)item.SelectedCells[0].Item;

            //int lp=l.Find((x => x.nazwa.Contains(itm.nazwa))).;
            for (var i = 0; i < GridItems.Count; i++)
                if (GridItems[i].Name == itm.Name)
                {
                    if (i < GridItems.Count - 1)
                    {
                        var szp = GridItems[i + 1];
                        var szk = GridItems[i];
                        GridItems[i]     = szp;
                        GridItems[i + 1] = szk;
                        gr1.ItemsSource  = null;
                        gr1.ItemsSource  = GridItems;
                    }

                    break;
                }

            btnzapisz.IsEnabled = true;
        }


        private void MenuItem_Clickup(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var itm = (SzablonItem)item.SelectedCells[0].Item;

            //int lp=l.Find((x => x.nazwa.Contains(itm.nazwa))).;
            for (var i = 0; i < GridItems.Count; i++)
                if (GridItems[i].Name == itm.Name)
                {
                    if (i > 0)
                    {
                        var szp = GridItems[i - 1];
                        var szk = GridItems[i];
                        GridItems[i]     = szp;
                        GridItems[i - 1] = szk;
                        gr1.ItemsSource  = null;
                        gr1.ItemsSource  = GridItems;
                    }

                    break;
                }

            btnzapisz.IsEnabled = true;
        }

        private ObservableCollection<SzablonItem> PopulateSzablon()
        {
            IFormatter formatter = new BinaryFormatter();
            //
            try
            {
                var items = JsonUtils.LoadJsonFile<ObservableCollection<SzablonItem>>(@"TempImport.dat");
                if (items is null || items.Count == 0) throw new ArgumentException("GridItems cannot be null");
                GridItems = items;
                return items;
            }
            catch
            {
                GridItems.Add(new SzablonItem("point", true, "numer Punktu", "string"));
                GridItems.Add(new SzablonItem("x", true, "współrzędna X", "numeric"));
                GridItems.Add(new SzablonItem("y", true, "współrzędna y", "numeric"));
                GridItems.Add(new SzablonItem("h", true, "wysokość h", "numeric"));
                GridItems.Add(new SzablonItem("date", false, "data pomiaru", "date"));
                GridItems.Add(new SzablonItem("code", false, "", "string"));
                GridItems.Add(new SzablonItem("mn", false, "", "string"));
                GridItems.Add(new SzablonItem("mh", false, "", "numeric"));
                GridItems.Add(new SzablonItem("mp", false, "", "numeric"));
                GridItems.Add(new SzablonItem("e", false, "", "integer"));
                GridItems.Add(new SzablonItem("sat", false, "", "integer"));
                GridItems.Add(new SzablonItem("pdop", false, "", "numeric"));
                GridItems.Add(new SzablonItem("heightPole", false, "", "numeric"));
                GridItems.Add(new SzablonItem("type", false, "", "string"));
            }

            //
            return GridItems;
        }


        private void SaveSzablon()
        {
            JsonUtils.SaveJson(@"TempImport.dat", GridItems);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var tmpItems = GridItems;
        }

        public ObservableCollection<SzablonItem> GridItems = new ObservableCollection<SzablonItem>();

        public List<SzablonItem> ListImport = null;
    }
}