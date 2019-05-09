using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Monica Ritz 5-09-2019
/// </summary>
namespace CsvToJson
{
    /// <summary>
    /// Final Json Object 
    /// Each Object contains one Date, Type and Ender objects but can contain multiple Orders
    /// </summary>
    public  class jsonObject
    {
        public jsonObject()
        {
            Orders = new List<Orders>();
        }

        public string date { get; set; }
        public string type { get; set; }
        public List<Orders> Orders { get; set; }
        public End Ender  { get; set; }

    }
  /// <summary>
  /// Orders object contains 
  /// record contains a single Buyer & Timings records
  /// record can contain multiple Line Item records
  /// </summary>
   public  class Orders
    {
        public Orders()
        {
            items = new List<Items>();
        }

        public string date { get; set; }
        public string code { get; set; }
        public string number { get; set; }
        public Buyer buyer { get; set; }
        public List<Items> items { get; set; }
        public Timing timings { get; set; }
       
    }
    /// <summary>
    /// Items Object contains sku and qty
    /// </summary>
   public class Items
    {
        public string sku { get; set; }
        public int qty { get; set; }
    }
    /// <summary>
    /// Buy Object contains Name, Street and Zip as string values
    /// </summary>
   public class Buyer
    {
        public string Name { get; set; }
        public string street { get; set; }
        public string zip { get; set; }
    }
    /// <summary>
    /// Timing Object contains start, stop, gap, offset, pause as int values
    /// </summary>
   public class Timing
    {
        public int start { get; set; }
        public int stop { get; set; }
        public int gap { get; set; }
        public int offset { get; set; }
        public int pause { get; set; }
    }
    /// <summary>
    /// End object contains process, paid, and created as int values
    /// </summary>
   public  class End
    {
        public int process { get; set; }
        public int paid { get; set; }
        public int created { get; set; }
    }

    class ConvertCSVtoJSON
    {
        static void Main(string[] args)
        {
            //gets the current directory and looks for a file named test.csv
            string inputPath = Path.Combine( Directory.GetCurrentDirectory(), "test.csv");
            
            string output = ConvertCvstoJson(inputPath);
            File.WriteAllText( "output.json", output);

        }
        /// <summary>
        /// Converts a flat CSV file into a hierarchical Json file using the first column as a tag
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string ConvertCvstoJson(string Path)
        {
            var csv = new List<string[]>();
           
            var lines = File.ReadAllLines(Path);
             
            foreach(var line in lines)
            {
                //parses the line by the "," value in order to account for a comma inside a value
                // split overloader found here:https://stackoverflow.com/questions/2245442/split-a-string-by-another-string-in-c-sharp
                string[] splitLine = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                splitLine[0] = splitLine[0].Replace("\"", string.Empty);
                splitLine[splitLine.Length-1] = splitLine[splitLine.Length-1].Replace("\"", string.Empty);
                csv.Add(splitLine);
            }

            jsonObject objResult = new jsonObject();
            //used to keep track of the current order 
            int current_order = -1;


            for(int i=0; i<csv.Count; i++)
            {
               //uses the first value of each internal array to assign the right values to the coresponding object
                 switch(csv[i][0])
                {
                    case "F":
                        objResult.date = csv[i][1];
                        objResult.type = csv[i][2];
                        break;

                    case "O":
                        Orders newOrder = new Orders();
                        newOrder.date = csv[i][1];
                        newOrder.code = csv[i][2];
                        newOrder.number = csv[i][3];
                        current_order++;
                        objResult.Orders.Add(newOrder);
                        break;

                    case "B":
                        Buyer newBuyer = new Buyer();
                        newBuyer.Name = csv[i][1];
                        newBuyer.street = csv[i][2];
                        newBuyer.zip = csv[i][3];
                        objResult.Orders[current_order].buyer = newBuyer;
                        break;

                    case "L":
                        Items newItem = new Items();
                        newItem.sku = csv[i][1];
                        newItem.qty = Int32.Parse(csv[i][2]);
                        objResult.Orders[current_order].items.Add(newItem);
                        break;

                    case "T":
                        Timing newTiming = new Timing();
                        newTiming.start = Int32.Parse(csv[i][1]);
                        newTiming.stop = Int32.Parse(csv[i][2]);
                        newTiming.gap = Int32.Parse(csv[i][3]);
                        newTiming.offset = Int32.Parse(csv[i][4]);
                        newTiming.pause = Int32.Parse(csv[i][5]);
                        objResult.Orders[current_order].timings = newTiming;
                        break;

                    case "E":
                        End newEnd = new End();
                        newEnd.process = Int32.Parse(csv[i][1]);
                        newEnd.paid = Int32.Parse(csv[i][2]);
                        newEnd.created = Int32.Parse(csv[i][3]);
                        objResult.Ender = newEnd;
                        break;
                }   
            }
            return JsonConvert.SerializeObject(objResult);
        }
    }
}
