using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Providers
{
    internal class JsonDataProvider<T> : IDataProvider<T> where T : IEntity
    {
        public string FilePath { get; set; }
        public JsonDataProvider(string filePath)
        {
            FilePath = filePath;
        }
        public List<T> LoadData()
        {
            //read json file
            List<T>? t = new List<T>();
            try
            {
                string fileContent = File.ReadAllText(FilePath);
                t = JsonConvert.DeserializeObject<List<T>>(fileContent) ?? [];
                Console.WriteLine($"Initialized data for {typeof(T).Name}");
            }
            catch (FileNotFoundException fe)
            {
                Console.WriteLine($"No data present for {typeof(T).Name}. File path: {fe.FileName}");
            }
            return t;
        }

        public OperationResult SaveData(T t)
        {
            List<T> values = LoadData();
            values.Add(t);

            WriteToFile(values);

            return new OperationResult { RowsAffected = values.Count, ErrorMessage = "Success" };
        }

        public T FindById(int id)
        {
            List<T> t = LoadData();
            return t.Where(it => it.GetId() == id).FirstOrDefault();
        }

        public List<T> FindByName(string name)
        {
            List<T> t = LoadData();
            return t.Where(it => it.GetName().ToLower().Contains(name)).ToList();
        }

        //e.g update tp add medicine to category
        public OperationResult UpdateColumn(int id, string column, string newValue)
        {
            List<T> values = LoadData();
            T? t = values.Find(t => t.GetId() == id);
            if (t != null)
            {
                Console.WriteLine($"id={id}, column={column}, newv={newValue}");
                Console.ReadKey();
                var p = typeof(T).GetProperty(column);
                Console.WriteLine($"p={p}");
                Console.ReadKey();
                if (p != null)
                {

                    p.SetValue(t, int.Parse(newValue));
                }
            }
            WriteToFile(values);

            return new OperationResult { RowsAffected = 1, ErrorMessage = "Success" };
        }

        public List<T> FindAllByColumn(string column, object value)
        {
            List<T> list = new List<T>();
            var data = LoadData();
            if (data.Count > 0)
                foreach (var t in LoadData())
                {
                    Console.WriteLine("");
                    if ((t.GetType().GetProperty(column).GetValue(t, null) ?? "").Equals(value))
                    {
                        list.Add(t);
                    }
                }
            return list;
        }

        public void SaveAll(List<T> values)
        {
            WriteToFile(values);
        }

        private void WriteToFile(List<T> values)
        {
            try
            {
                string output = JsonConvert.SerializeObject(values, Formatting.Indented);
                File.WriteAllText(FilePath, output);
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            catch (JsonSerializationException je)
            {
                Console.WriteLine(je.Message);
            }
        }

        public List<T> FindByPage(int p, int s, int d)
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}
