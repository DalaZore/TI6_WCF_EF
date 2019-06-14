using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data.SQLite;
using System.Text;

namespace HelloWorld.Contract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        
        [OperationContract]
        void AddAlbum(string ArtistID,string AlbumName,int TrackCount,string[] tracks);

        [OperationContract]
        void DeleteAlbum(string AlbumName);

        [OperationContract]
        IEnumerable<DataRow> SearchAlbumsName(string AlbumName);

        [OperationContract]
        IEnumerable<DataRow> SearchTracksName(string trackName);

        [OperationContract]
        IEnumerable<DataRow> SearchAlbumsArtist(string artistName);

        [OperationContract]
        IEnumerable<DataRow> TracksByCustomer(string customerID);

        [OperationContract]
        IEnumerable<DataRow> InvoicesByCustomer(string customerID);

    }

  
}
