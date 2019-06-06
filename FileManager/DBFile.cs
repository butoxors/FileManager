using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    public class DBFile : INotifyPropertyChanged
    {
        /// <summary>
        /// fields
        /// </summary>
        private int id;
        private string name;
        private byte[] data;
        private string extension;
        /// <summary>
        /// DB Fields
        /// </summary>
        public int ID { get { return id; } set { this.id = value; OnPropertyChanged("ID"); } }
        public string Name { get { return name; } set { this.name = value; OnPropertyChanged("Name"); } }
        public byte[] Data { get { return data; } set { this.data = value; OnPropertyChanged("Data"); } }
        public string Extension { get { return extension; } set { this.extension = value; OnPropertyChanged("Extension"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
