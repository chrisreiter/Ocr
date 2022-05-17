using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NSOCR_NameSpace; //Add NSOCR namespace from "NSOCR.cs"


namespace Sample
{
    public class CheckedListItem<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
 
        private bool isChecked;
        private T item;
 
        public CheckedListItem()
        {}
 
        public CheckedListItem(T item, bool isChecked)
        {
            this.item = item;
            this.isChecked = isChecked;
        }
 
        public T Item
        {
            get { return item; }
            set
            {
                item = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Item"));
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
    }

    public class Language
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window1 fmMain;
        public ObservableCollection<CheckedListItem<Language>> Languages { get; set; }
        public ObservableCollection<CheckedListItem<Language>> LanguagesAsian { get; set; }

        public Window4()
        {
            InitializeComponent();

            Languages = new ObservableCollection<CheckedListItem<Language>>();

            Languages.Add(new CheckedListItem<Language>(new Language() {Name = "Bulgarian" }, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name = "Catalan" }, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name = "Croatian" }, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name = "Czech" }, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Danish"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Dutch"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="English"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Estonian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Finnish"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="French"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="German"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Hungarian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Indonesian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Italian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Latvian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Lithuanian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Norwegian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Polish"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Portuguese"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Romanian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Russian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Slovak"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Slovenian"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name="Spanish"}, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name = "Swedish" }, false));
            Languages.Add(new CheckedListItem<Language>(new Language() {Name = "Turkish" }, false));

            LanguagesAsian = new ObservableCollection<CheckedListItem<Language>>();

            LanguagesAsian.Add(new CheckedListItem<Language>(new Language() { Name = "Arabic" }, false));
            LanguagesAsian.Add(new CheckedListItem<Language>(new Language() { Name = "Chinese_Simplified" }, false));
            LanguagesAsian.Add(new CheckedListItem<Language>(new Language() { Name = "Chinese_Traditional" }, false));
            LanguagesAsian.Add(new CheckedListItem<Language>(new Language() { Name = "Japanese" }, false));
            LanguagesAsian.Add(new CheckedListItem<Language>(new Language() { Name = "Korean" }, false));

            DataContext = this;

        }

        public void LoadLanguages()
        {
            int i;
            string val;
            for (i = 0; i < Languages.Count; i++)
            {
                if (fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + Languages[i].Item.Name, out val) < TNSOCR.ERROR_FIRST)
                    Languages[i].IsChecked = (val == "1");
                else
                    Languages[i].IsChecked = false;
            }
            for (i = 0; i < LanguagesAsian.Count; i++)
            {
                if (fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + LanguagesAsian[i].Item.Name, out val) < TNSOCR.ERROR_FIRST)
                    LanguagesAsian[i].IsChecked = (val == "1");
                else
                    LanguagesAsian[i].IsChecked = false;
            }
        }

        public bool ApplyLanguages()
        {
            int i;
            bool ch1 = false;
            for (i = 0; i < Languages.Count; i++)
                if (Languages[i].IsChecked)
                {
                    ch1 = true;
                    break;
                }
            int ch2 = 0;
            for (i = 0; i < LanguagesAsian.Count; i++)
                if (LanguagesAsian[i].IsChecked)
                    ch2++;

            if (!ch1 && (ch2 == 0))
            {
                System.Windows.MessageBox.Show("Please select at least one language for recognition.");
                return false;
            }
            if (ch1 && (ch2 > 0))
            {
                System.Windows.MessageBox.Show("Using both main and asian languages in same zone is not supported.");
                return false;
            }
            if (ch2 > 1)
            {
                System.Windows.MessageBox.Show("Using two or more asian languages in same zone is not supported currently.");
                return false;
            }

            for (i = 0; i < Languages.Count; i++)
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + Languages[i].Item.Name, Languages[i].IsChecked ? "1" : "0");
            for (i = 0; i < LanguagesAsian.Count; i++)
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + LanguagesAsian[i].Item.Name, LanguagesAsian[i].IsChecked ? "1" : "0");

            return true;
        }

        private void bkCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLanguages();
        }

        private void bkOK_Click(object sender, RoutedEventArgs e)
        {
            if (ApplyLanguages())
                Close();
        }



    }
}
