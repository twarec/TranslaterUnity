using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;


namespace ExcelToJava
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table();
            string path = "";
            do {
                Console.WriteLine("Введите путь к фаилу xml");
                path = Console.ReadLine();
                if (!File.Exists(path))
                    Console.WriteLine("Фаил не найден\n/////////////////////////////////////////////////");
                else
                    break;
            } while (true);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            foreach (XmlNode v in xDoc.DocumentElement)
            {
                if (v.Name == "ss:Worksheet")
                {
                    foreach (XmlNode Tablse in v.ChildNodes)
                        if (Tablse.Name == "Table")
                        {
                            foreach (XmlNode Row in Tablse.ChildNodes)
                                if (Row.Name == "Row")
                                {
                                    table.Row.Add(new Row());
                                    Console.WriteLine("---------------------------------------");
                                    foreach (XmlNode Cell in Row.ChildNodes)
                                    {
                                        if (Cell.ChildNodes.Count > 0)
                                        {
                                            Console.WriteLine(Cell.LastChild.InnerText);
                                            table.Row[table.Row.Count - 1].Cell.Add(new Cell() { Data = Cell.LastChild.InnerText });
                                        }
                                    }
                                }
                        }
                }
            }
            Console.WriteLine("---------------------------------------");
            do
            {
                Console.WriteLine("Введите дериктория для сохранения фаилов");
                path = Console.ReadLine();
                if (!Directory.Exists(path))
                    Console.WriteLine("Директория не найден\n//////////////////////////////////");
                else
                    break;
            } while (true);
            foreach(Row row in table.Row)
            {
                string Name = row.Cell[0].Data;
                int count = row.Cell.Count;
                string Text = "%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n--- !u!114 &11400000\nMonoBehaviour:\n  m_ObjectHideFlags: 0\n  m_PrefabParentObject: {fileID: 0}\n  m_PrefabInternal: {fileID: 0}\n  m_GameObject: {fileID: 0}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n  m_Script: {fileID: 11500000, guid: 11b64caea72d63142aae19454fd24090, type: 3}\n  m_Name: " + Name + "\n  m_EditorClassIdentifier: \n  dictionary:\n";
                for (int i = 1; i < count; i++)
                {
                    Text += $"  - key: {table.Row[0].Cell[i].Data}\n    value: {row.Cell[i].Data}\n";
                }
                Console.WriteLine($"----------------------------------\nName: {Name}\n***************************\n{Text}");
                using(FileStream fs = File.Create($"{path}/{Name}.asset"))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(Text);
                    sw.Close();
                }
            }
            Console.ReadLine();
            
        }
    }
    public class Table
    {
        public List<Row> Row = new List<Row>();
    }
    public class Row
    {
        public List<Cell> Cell = new List<ExcelToJava.Cell>();
    }
    public class Cell
    {
        public string Data;
    }
}
