using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileManager
{
    public class Model : INotifyPropertyChanged
    {
        /// <summary>
        /// Name of connection on connectio.config file
        /// </summary>
        private readonly string Conn = "DefaultConnection";
        /// <summary>
        /// DataContext
        /// </summary>
        private AppContext appContext;

        /// <summary>
        /// ctor
        /// </summary>
        public Model()
        {
            appContext = new AppContext(Conn);
            Task t = LoadAsync();
        }
        /// <summary>
        /// Get data context as list
        /// </summary>
        /// <returns></returns>
        public BindingList<DBFile> GetContext()
        {
            return appContext.DBFiles.Local.ToBindingList();
        }
        /// <summary>
        /// Async load data to data context
        /// </summary>
        /// <returns></returns>
        private async Task LoadAsync()
        {
            await appContext.DBFiles.LoadAsync();
        }
        /// <summary>
        /// find file on DB
        /// </summary>
        /// <param name="ID">id of file</param>
        /// <returns></returns>
        private async Task<DBFile> FindObject(int ID)
        {
            return await appContext.DBFiles.FindAsync(ID);
        }
        /// <summary>
        /// Load file from DB
        /// </summary>
        /// <param name="item"></param>
        public void Load(DBFile item)
        {
            try
            {
                if (item == null)
                {
                    throw new NullReferenceException("Item is NULL");
                }
                else
                {
                    Task<DBFile> t = FindObject(item.ID);
                    t.Wait();

                    DBFile file = t.Result;

                    SaveFileDialog sfd = new SaveFileDialog();

                    //string format = RegStr(@"[.]\w+$", file.Name);

                    sfd.Filter += "File (*" + file.Extension + ") | *" + file.Extension;
                    sfd.FileName = file.Name;

                    if (sfd.ShowDialog() == true)
                    {
                        File.WriteAllBytes(sfd.FileName, file.Data);
                    }
                }
            }
            catch (Exception) { }
        }
        /// <summary>
        /// Add new file to DB
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private async Task SaveObject(DBFile f)
        {
            appContext.DBFiles.Add(f);
            await appContext.SaveChangesAsync();
        }
        /// <summary>
        /// reg func helper
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private string RegStr(string pattern, string str)
        {
            Regex r = new Regex(pattern);
            Match m = r.Match(str);

            return m.ToString();
        }
        /// <summary>
        /// Save file into DB
        /// </summary>
        public void Save()
        {
            OpenFileDialog fod = new OpenFileDialog();
            try
            {
                if (fod.ShowDialog() == true)
                {
                    string name = RegStr(@"\\.+", fod.FileName);
                    string extension = RegStr(@"\..+", fod.FileName);

                    name = name.Remove(name.Length - extension.Length, extension.Length).Trim('\\');

                    using (var fs = new FileStream(fod.FileName, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, (int)fs.Length);

                        SaveObject(new DBFile
                        {
                            Data = buffer,
                            Name = name,
                            Extension = extension
                        }).Wait();
                    }
                }
            }
            catch (Exception) { }
        }
        /// <summary>
        /// Handler of prop changing
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Props changing function
        /// </summary>
        /// <param name="prop"></param>
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
