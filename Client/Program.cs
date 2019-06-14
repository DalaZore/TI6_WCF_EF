using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data.SQLite;
using HelloWorld.Contract;
using HelloWorld.Service;


namespace ServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpointAddress = new EndpointAddress("http://localhost:80/HelloWorld");
            var proxy = ChannelFactory<IService>.CreateChannel(new BasicHttpBinding(), endpointAddress);
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../../Chinook_Sqlite_AutoIncrementPKs.sqlite");
            bool exit = false;
            
            string AlbumName;
            string trackName;
            string artistName;
            string customerID;
            
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("What Operation would you like to Perform?");
                Console.WriteLine("1: Add Albums with Tracks");
                Console.WriteLine("2: Delete Albums");
                Console.WriteLine("3: Search Albums by Name");
                Console.WriteLine("4: Search Tracks by Name");
                Console.WriteLine("5: Search Albums by Artist");
                Console.WriteLine("6: Search Purchased tracks by Customer");
                Console.WriteLine("7: Search Invoices by Customer");
                Console.WriteLine("0: Exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        
                        Console.Clear();
                        Console.WriteLine("Enter the ID of the Artist");
                        string ArtistID = Console.ReadLine();
                        Console.WriteLine("Enter the Name of the Album");
                        AlbumName = Console.ReadLine();
                        Console.WriteLine("How many Tracks are on the Album?");
                        int TrackCount = Convert.ToInt16(Console.ReadLine());
                        string[] tracks = new string[TrackCount];
                        int i = 0;

                        while (TrackCount > i)
                        {
                            Console.WriteLine("Whats the name of Track {0}", i+1);
                            trackName = Console.ReadLine();
                            tracks[i] = trackName;
                            i++;
                        }
                        Console.WriteLine("The Data is being written to the Database");
                        Console.WriteLine("Writing the Album Information");
                        
                        
                        proxy.AddAlbum(ArtistID,AlbumName,TrackCount,tracks);
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("What Album do you want to Delete?");
                        AlbumName = Console.ReadLine();
                        
                        proxy.DeleteAlbum(AlbumName);
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Name an Album");
                        AlbumName = Console.ReadLine();

                        
                        foreach (DataRow album in proxy.SearchAlbumsName(AlbumName))
                        {
                            Console.WriteLine(album.Field<string>("Title"));
                            return;
                        }
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("Name a track");
                        trackName = Console.ReadLine();
                        
                        
                        foreach (DataRow track in proxy.SearchTracksName(trackName))
                        {
                            Console.WriteLine(track.Field<string>("Name"));
                        }
                        
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Enter an Artists Name");
                        artistName = Console.ReadLine();
                        
                        foreach (DataRow album in proxy.SearchAlbumsArtist(artistName))
                        {
                            Console.WriteLine(album.Field<string>("Title"));
                        }
                        
                        
                        break;
                    case "6":
                        Console.Clear();
                        Console.WriteLine("Enter an customer ID");
                        customerID = Console.ReadLine();
                        
                        foreach (DataRow trackeroni in proxy.TracksByCustomer(customerID))
                        {
                            Console.WriteLine(trackeroni.Field<string>("Name"));
                        }
                        
                        break;
                    case "7":
                        Console.Clear();
                        Console.WriteLine("Enter an customer ID");
                        customerID = Console.ReadLine();
                        
                        foreach (DataRow invoiceroni in proxy.InvoicesByCustomer(customerID))
                        {
                            Console.WriteLine(invoiceroni.Field<string>("InvoiceId"));
                        }

                        break;
                    case "0":
                        connection.Close();
                            exit = true;
                        break;
                }
            }
        }
    }
}
