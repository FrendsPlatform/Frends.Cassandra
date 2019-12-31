using System.ComponentModel;

namespace Frends.Community.CassandraDB.Helpers
{
    public class DatabaseConnection
    {

        /// <summary>
        /// The address of the CassandraDB server
        /// </summary>
        [DisplayName("Server Address")]
        [DefaultValue("")]
        public string Nodes { get; set; }

        /// <summary>
        /// The port used to connect to the CassandraDB server
        /// </summary>
        [DisplayName("Server Port")]
        [DefaultValue("")]
        public string ServerPort { get; set; }

        /// <summary>
        /// The name of the CassandraDB schema to perform the operation on
        /// </summary>
        [DisplayName("Schema Name")]
        [DefaultValue("")]
        public string SchemaName { get; set; }

        /// <summary>
        /// The username to use when connecting to CassandraDB
        /// </summary>
        [DisplayName("Username")]
        public string UserName { get; set; }

        /// <summary>
        /// The password to use when connecting to CassandraDB
        /// </summary>
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
