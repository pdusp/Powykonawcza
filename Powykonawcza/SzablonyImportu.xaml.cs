﻿using Newtonsoft.Json;
using Powykonawcza.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            populateSzablon();
            gr1.ItemsSource = GridItems;
        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }

        private ObservableCollection<SzablonItem> populateSzablon()
        {
            IFormatter formatter = new BinaryFormatter();
            //
            try
            {
                GridItems.Clear();
                Stream streamIn = new FileStream(@"SzablonImportu.dat", FileMode.Open, FileAccess.Read);
                GridItems = (ObservableCollection<SzablonItem>)formatter.Deserialize(streamIn);
                streamIn.Close();
                return GridItems;
            }
            catch
            {
                GridItems.Add(new SzablonItem(  "pkt",   true, "numer Punktu" ));
                GridItems.Add(new SzablonItem(  "x",   true,   "współrzędna X" ));
                GridItems.Add(new SzablonItem(   "y", true,  "współrzędna y" ));
                GridItems.Add(new SzablonItem(   "z", true,  "wysokość h" ));
                GridItems.Add(new SzablonItem( "data", false, "data pomiaru" ));
                GridItems.Add(new SzablonItem( "kod",  false, "" ));
                GridItems.Add(new SzablonItem( "mn",  false,  "" ));
                GridItems.Add(new SzablonItem( "mh",  false,  "" ));
                GridItems.Add(new SzablonItem( "mp",  false,  "" ));
                GridItems.Add(new SzablonItem(  "e",  false,  "" ));
                GridItems.Add(new SzablonItem( "sat",  false,  "" ));
                GridItems.Add(new SzablonItem( "pdop",  false, "" ));
                GridItems.Add(new SzablonItem( "wys_tyczki", false, "" ));
                GridItems.Add(new SzablonItem( "typ", false,  "" ));
            }
            //
            return GridItems;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void SaveSzablon()
        {
            //  IFormatter formatter = new BinaryFormatter();
            IFormatter formatterOut = new BinaryFormatter();
            Stream stream = new FileStream(@"SzablonImportu.dat", FileMode.Create, FileAccess.Write);
            formatterOut.Serialize(stream, GridItems);
            stream.Close();
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
                    if (i < GridItems.Count - 1)
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
            gr1.Columns[0].IsReadOnly = true;
            gr1.Columns[2].IsReadOnly = true;
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

            for (int i = 0; i < GridItems.Count; i++)
            {
                string nazwa = GridItems[i].nazwa;
                //foreach (var item in gr1.Items)
                //{
                //    if ((String)item == nazwa)
                //    {

                //        break;
                //    }

                //}

                //foreach (var dr in gr1.rows)
                {
                     
                   // if (dr ==nazwa)
                    {
                       // l[i] = new SzablonItem(dr[0].ToString(), (bool)dr[1], dr[2].ToString());
                    }
                     
                    Console.Write(Environment.NewLine);
                }



                SaveSzablon();
            }


        }
    }
}
