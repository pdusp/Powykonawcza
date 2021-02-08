using Powykonawcza.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Powykonawcza
{
    /// <summary>
    /// Interaction logic for SzablonyImportu.xaml
    /// </summary>
    public partial class SzablonyImportu : Window
    {
        private List<SzablonItem> l = new List<SzablonItem>();
        public SzablonyImportu()
        {
            InitializeComponent();
            populateSzablon();
            gr1.ItemsSource = l;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            
        }

        private List<SzablonItem> populateSzablon()
        {
           
            l.Add(new SzablonItem() {nazwa="pkt", import= true, Inne="numer Punktu" });
            l.Add(new SzablonItem() {nazwa = "x", import = true, Inne = "współrzędna X" });
            l.Add(new SzablonItem() { nazwa = "y", import = true, Inne = "współrzędna Y" });
            l.Add(new SzablonItem() { nazwa = "z", import = true, Inne = "wysokość H" });
            l.Add(new SzablonItem() { nazwa = "data", import = false, Inne = "data pomiaru" });
            l.Add(new SzablonItem() { nazwa = "kod", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "mn", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "mh", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "mp", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "e", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "sat", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "pdop", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "wys_tyczki", import = false, Inne = "" });
            l.Add(new SzablonItem() { nazwa = "typ", import = false, Inne = "" });

            return l;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

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
            var toDeleteFromBindedList = (SzablonItem)item.SelectedCells[0].Item;

            //Remove the toDeleteFromBindedList object from your ObservableCollection
           // yourObservableCollection.Remove(toDeleteFromBindedList);

        }

        private void MenuItem_Clickdown(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
