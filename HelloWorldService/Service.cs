using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using HelloWorld.Contract;

namespace HelloWorld.Service
{
        public class Service : IService
        {
            
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../../Chinook_Sqlite_AutoIncrementPKs.sqlite");
            public void connect()
            {
                SQLiteConnection connection = new SQLiteConnection("Data Source=../../../Chinook_Sqlite_AutoIncrementPKs.sqlite");
            }

            public bool disconnect()
            {
               connection.Close();
                return true;
            }
            
             
        public void AddAlbum(string ArtistID,string AlbumName,int TrackCount,string[] tracks)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../../Chinook_Sqlite_AutoIncrementPKs.sqlite");
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Album",connection );
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            DataTable albums = dataSet.Tables[0];
            //ADD DATA TO DB
            DataRow newAlbum = albums.NewRow();
            
            newAlbum["Title"] = AlbumName;
            newAlbum["ArtistId"] = ArtistID;

            albums.Rows.Add(newAlbum);
            SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(dataAdapter);
            dataAdapter.Update(dataSet);
            
            dataAdapter = new SQLiteDataAdapter("SELECT * FROM Track", connection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            DataTable dataTableTracks = dataSet.Tables[0];

            int i = 0;
            foreach (string track in tracks)
            {
                DataRow newTrack = dataTableTracks.NewRow();
                newTrack["Name"] = track;
                newTrack["MediaTypeId"] = 1;
                newTrack["Milliseconds"] = 1;   
                newTrack["UnitPrice"] = 1.05;   
                dataTableTracks.Rows.Add(newTrack);
                commandBuilder = new SQLiteCommandBuilder(dataAdapter);
                dataAdapter.Update(dataSet);
                i++;
            }           
//            commandBuilder = new SQLiteCommandBuilder(dataAdapter);
//            dataAdapter.Update(dataSet);
        }
            
        public void DeleteAlbum(string AlbumName)
        {
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Album", connection);
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet);

            DataTable albums = dataSet.Tables[0];
            

            IEnumerable<DataRow> albumsQuery =
                from album in albums.AsEnumerable().AsParallel()
                where album["Title"].ToString() == AlbumName
                select album;

            DataRow albumRow = albumsQuery.ToArray()[0];

            albumRow.Delete();

            SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(dataAdapter);

            dataAdapter.Update(dataSet);
        }
        
        public IEnumerable<DataRow> SearchAlbumsName(string AlbumName)
        {
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Album", connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            DataTable albums = dataSet.Tables[0];
            
            
            IEnumerable<DataRow> albumsQuery =
                from album in albums.AsEnumerable().AsParallel()
                where album[1].ToString() == AlbumName
                select album;

            IEnumerable<DataRow> albumsArray = albumsQuery.ToArray();
  
            return albumsArray;
        }
        public IEnumerable<DataRow> SearchTracksName(string trackName)
        {
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Track", connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            DataTable tracks = dataSet.Tables[0];
            
            
            IEnumerable<DataRow> tracksQuery =
                from track in tracks.AsEnumerable().AsParallel()
                where track[1].ToString() == trackName
                select track;

            IEnumerable<DataRow> tracksArray = tracksQuery.ToArray();
            return tracksArray;

        }
        
        public IEnumerable<DataRow> SearchAlbumsArtist(string artistName)
        {
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Album", connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            DataTable albums = dataSet.Tables[0];
            
            dataAdapter = new SQLiteDataAdapter("SELECT * FROM Artist", connection);
            DataSet dataSetArtist = new DataSet();
            dataAdapter.Fill(dataSetArtist);
            DataTable artist = dataSetArtist.Tables[0];

            
            IEnumerable<DataRow> artistQuery =
                from artists in artist.AsEnumerable().AsParallel()
                where artists[1].ToString() == artistName
                select artists;

            DataRow[] artistArray = artistQuery.ToArray();
            DataRow artistId = artistArray[0];
            
            
            IEnumerable<DataRow> albumsQuery =
                from album in albums.AsEnumerable().AsParallel()
                where album[2].ToString()==artistId[0].ToString()
                orderby album[1]
                select album;

            IEnumerable<DataRow> albumsArray = albumsQuery.ToArray();
            return albumsArray;

        }
        
        public IEnumerable<DataRow> TracksByCustomer(string customerID)
        {
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Invoice", connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            DataTable invoice = dataSet.Tables[0];
                
            
            dataAdapter = new SQLiteDataAdapter("SELECT * FROM Customer", connection);
            DataSet dataSetArtist = new DataSet();
            dataAdapter.Fill(dataSetArtist);
            DataTable customer = dataSetArtist.Tables[0];
            
            dataAdapter = new SQLiteDataAdapter("SELECT * FROM InvoiceLine", connection);
            DataSet dataSetInvoiceLine = new DataSet();
            dataAdapter.Fill(dataSetInvoiceLine);
            DataTable invoiceLine = dataSetInvoiceLine.Tables[0];
            
            dataAdapter = new SQLiteDataAdapter("SELECT * FROM Track", connection);
            DataSet dataSetTrack = new DataSet();
            dataAdapter.Fill(dataSetTrack);
            DataTable track = dataSetTrack.Tables[0];

            
            
            IEnumerable<DataRow> customerQuery =
                from customers in customer.AsEnumerable().AsParallel()
                where customers[0].ToString() == customerID
                select customers;

            DataRow[] customerArray = customerQuery.ToArray();
            DataRow customerId = customerArray[0];
            
            
            IEnumerable<DataRow> invoicesQuery =
                from invoices in invoice.AsEnumerable().AsParallel()
                where invoices[1].ToString()==customerId[0].ToString()
                orderby invoices[0]
                select invoices;

            DataRow[] invoicesArray = invoicesQuery.ToArray();
            DataRow invoiceId = invoicesArray[0];

            
            IEnumerable<DataRow> invoiceLineQuery =
                from invoiceLines in invoiceLine.AsEnumerable().AsParallel()
                where invoiceLines[1].ToString()==invoiceId[0].ToString()
                orderby invoiceLines[0]
                select invoiceLines;
            
            DataRow[] invoiceLineArray = invoiceLineQuery.ToArray();
            DataRow trackId = invoiceLineArray[0];
            
            IEnumerable<DataRow> trackQuery = 
                from tracks in track.AsEnumerable().AsParallel()
                where tracks[1].ToString()==trackId[2].ToString()
                orderby tracks[0]
                select tracks;
            
            IEnumerable<DataRow> trackArray = trackQuery.ToArray();

            return trackArray;
        }
        
        public IEnumerable<DataRow> InvoicesByCustomer(string customerID)
        {
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Invoice", connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            DataTable invoice = dataSet.Tables[0];
            
            dataAdapter = new SQLiteDataAdapter("SELECT * FROM Customer", connection);
            DataSet dataSetArtist = new DataSet();
            dataAdapter.Fill(dataSetArtist);
            DataTable customer = dataSetArtist.Tables[0];

            
            
            IEnumerable<DataRow> customerQuery =
                from customers in customer.AsEnumerable().AsParallel()
                where customers[0].ToString() == customerID
                select customers;

            DataRow[] customerArray = customerQuery.ToArray();
            DataRow customerId = customerArray[0];
            
            
            IEnumerable<DataRow> invoicesQuery =
                from invoices in invoice.AsEnumerable().AsParallel()
                where invoices[1].ToString()==customerId[0].ToString()
                orderby invoices[0]
                select invoices;

            IEnumerable<DataRow> invoicesArray = invoicesQuery.ToArray();

            return invoicesArray;
            
        }
        }
}


