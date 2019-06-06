using System.Data.Entity;

namespace FileManager
{
    public class AppContext : DbContext
    {
        /// <summary>
        /// Prop to interaction with table
        /// </summary>
        public DbSet<DBFile> DBFiles { get; set; }

        /// <summary>
        /// Ctor of context
        /// </summary>
        /// <param name="dbNameOrConnection">Name of connection</param>
        public AppContext(string dbNameOrConnection = "DefaultConnection") :base(dbNameOrConnection) { }
    }
}
