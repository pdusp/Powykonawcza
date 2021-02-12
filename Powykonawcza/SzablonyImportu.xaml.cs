﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using Powykonawcza.DAL;
using Powykonawcza.Model.Szablon;

namespace Powykonawcza
{
    /// <summary>
    /// Interaction logic for SzablonyImportu.xaml<!--AutoGeneratedColumns="gr1_AutoGeneratedColumns" -->
    /// </summary>
    public partial class SzablonyImportu : Window
    {
        public ObservableCollection<SzablonItem> GridItems = new ObservableCollection<SzablonItem>();
        public SzablonyImportu()
        {
            InitializeComponent();
            gr1.ItemsSource = populateSzablon();
        }
         

        public IEnumerable<SzablonItem> CurrentTemplate()
        {
            for (int i = 0; i < GridItems.Count; i++)
            {
                if (GridItems[i].import == true)
                    yield return GridItems[i];
            }
        }

        private ObservableCollection<SzablonItem> populateSzablon()
        {
            IFormatter formatter = new BinaryFormatter();
            //
            try
            {
                var GridItems = JsonUtils.LoadJsonFile<ObservableCollection<SzablonItem>>(@"SzablonImportu.dat");
                if (GridItems is null || GridItems.Count==0) { throw new ArgumentException("GridItems cannot be null"); }
                return GridItems;
            }
            catch
            {
                GridItems.Add(new SzablonItem("pkt", true, "numer Punktu", "string"));
                GridItems.Add(new SzablonItem("x", true, "współrzędna X", "numeric"));
                GridItems.Add(new SzablonItem("y", true, "współrzędna y", "numeric"));
                GridItems.Add(new SzablonItem("h", true, "wysokość h", "numeric"));
                GridItems.Add(new SzablonItem("data", false, "data pomiaru", "date"));
                GridItems.Add(new SzablonItem("kod", false, "", "string"));
                GridItems.Add(new SzablonItem("mn", false, "", "string"));
                GridItems.Add(new SzablonItem("mh", false, "", "numeric"));
                GridItems.Add(new SzablonItem("mp", false, "", "numeric"));
                GridItems.Add(new SzablonItem("e", false, "", "integer"));
                GridItems.Add(new SzablonItem("sat", false, "", "integer"));
                GridItems.Add(new SzablonItem("pdop", false, "", "numeric"));
                GridItems.Add(new SzablonItem("wys_tyczki", false, "", "numeric"));
                GridItems.Add(new SzablonItem("typ", false, "", "string"));
            }
            //
            return GridItems;
        }

 

        private void SaveSzablon()
        {
            JsonUtils.SaveJson(@"SzablonImportu.dat", GridItems);
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
            for (int i = 0; i < GridItems.Count; i++)
            {
                if (GridItems[i].nazwa == itm.nazwa)
                {
                    if (i > 0)
                    {
                        var szp = GridItems[i - 1];
                        var szk = GridItems[i];
                        GridItems[i] = szp;
                        GridItems[i - 1] = szk;
                        gr1.ItemsSource = null;
                        gr1.ItemsSource = GridItems;
                    }
                    break;
                }
            }
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
            for (int i = 0; i < GridItems.Count; i++)
            {
                if (GridItems[i].nazwa == itm.nazwa)
                {
                    if (i < GridItems.Count-1)
                    {
                        var szp = GridItems[i + 1];
                        var szk = GridItems[i];
                        GridItems[i] = szp;
                        GridItems[i + 1] = szk;
                        gr1.ItemsSource = null;
                        gr1.ItemsSource = GridItems;
                    }
                    break;
                }
            }
        }

        private void gr1_AutoGeneratedColumns(object sender, EventArgs e)
        {
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }


        private void clik_zapisz(object sender, RoutedEventArgs e)
        {
            var rows = GetDataGridRows(gr1);
            var itemsSource = gr1.ItemsSource as IEnumerable;
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
        }
    }
}
