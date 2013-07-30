using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeWork1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Menu();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Для завершения программы нажмите любую клавишу...");
            Console.ReadKey();
        }

        private static Stream GetFileStream(string fileName, FileMode mode)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (File.Exists(path))
            {
                return new FileStream(path, mode);
            }

            var asm = Assembly.GetExecutingAssembly();
            var names = asm.GetManifestResourceNames();
            path = Assembly.GetExecutingAssembly().GetName().Name + ".Resource." + fileName;
            if (!names.Contains(path))
            {
                Debugger.Break();
                throw new ApplicationException("Отсутствует Embedded Resource: " + fileName);
            }
            return asm.GetManifestResourceStream(path);
        }

        private static void SaveWithLinqToXml(Cluster cluster)
        {
            const string fileName = @"ClusterWithLinq.xml"; //Directory.GetCurrentDirectory() + @"\clusterWithLinq.xml";

            var doc = new XDocument();
            var xElementRoot = new XElement("Cluster");
            var elementMachinery = new XElement("Machinery");

            foreach (var machine in cluster.Machinery)
            {
                var elementMachine = new XElement("Machine");

                var elementProcessorName = new XElement("ProcessorName") {Value = machine.ProcessorName};
                elementMachine.Add(elementProcessorName);

                var elementCoresNumber = new XElement("CoresNumber") {Value = machine.CoresNumber};
                elementMachine.Add(elementCoresNumber);

                var elementCpuSpeed = new XElement("CpuSpeed") {Value = machine.CpuSpeed};
                elementMachine.Add(elementCpuSpeed);

                var elementRam = new XElement("Ram") {Value = machine.Ram};
                elementMachine.Add(elementRam);

                var elementHdd = new XElement("Hdd") {Value = machine.Hdd};
                elementMachine.Add(elementHdd);

                var elementVideoSystemType = new XElement("VideoSystemType") {Value = machine.VideoSystemType};
                elementMachine.Add(elementVideoSystemType);

                elementMachinery.Add(elementMachine);
            }

            xElementRoot.Add(elementMachinery);
            doc.Add(xElementRoot);

            var outputFileStream = GetFileStream(fileName, FileMode.Create);//new FileStream(fileName, FileMode.Create);
            doc.Save(outputFileStream);
            outputFileStream.Close();
        }

        private static Cluster LoadWithLinqToXml()
        {
            const string fileName = "ClusterWithLinq.xml"; //Directory.GetCurrentDirectory() + @"\clusterWithLinq.xml";
            var inputFileStream = GetFileStream(fileName, FileMode.Open);//new FileStream(fileName, FileMode.Open);

            var xDoc = XDocument.Load(inputFileStream);

            var xElement = xDoc.Element("Cluster");
            if (xElement == null)
            {
                throw new Exception("XML-документ содержит ошибку.");
            }

            var element = xElement.Element("Machinery");
            if (element == null)
            {
                throw new Exception("XML-документ содержит ошибку.");
            }

            var query = from machine in element.Elements("Machine")
                select new
                {
                    processorName = machine.Element("ProcessorName") != null ? machine.Element("ProcessorName").Value : "",
                    coresNumber = machine.Element("CoresNumber") != null ? machine.Element("CoresNumber").Value : "",
                    cpuSpeed = machine.Element("CpuSpeed") != null ? machine.Element("CpuSpeed").Value : "",
                    ram = machine.Element("Ram") != null ? machine.Element("Ram").Value : "",
                    hdd = machine.Element("Hdd") != null ? machine.Element("Hdd").Value : "",
                    videoSystemType = machine.Element("VideoSystemType") != null ? machine.Element("VideoSystemType").Value : ""
                };

            var listOfMachine = query.Select(machineParams => 
                new Machine(
                    machineParams.processorName, 
                    machineParams.coresNumber, 
                    machineParams.cpuSpeed, 
                    machineParams.ram, 
                    machineParams.hdd, 
                    machineParams.videoSystemType
                )).ToList();
            var cluster = new Cluster(listOfMachine);

            inputFileStream.Close();

            return cluster;
        }

        private static void SaveWithStream(Cluster cluster)
        {
            var xmlser = new XmlSerializer(typeof(Cluster));
            const string fileName = @"ClusterWithStream.xml"; //Directory.GetCurrentDirectory() + @"\clusterWithStream.xml";
            var outputFileStream = GetFileStream(fileName, FileMode.Create);//new FileStream(fileName, FileMode.Create);
            xmlser.Serialize(outputFileStream, cluster);
            outputFileStream.Close();
        }

        private static Cluster LoadWithStream()
        {
            var xmlser = new XmlSerializer(typeof(Cluster));
            const string fileName = @"ClusterWithStream.xml"; //Directory.GetCurrentDirectory() + @"\clusterWithStream.xml";
            var inputFileStream = GetFileStream(fileName, FileMode.Open);//new FileStream(fileName, FileMode.Open);
            var cluster = (Cluster)xmlser.Deserialize(inputFileStream);
            inputFileStream.Close();

            return cluster;
        }

        private static Cluster InputCluster()
        {
            var listOfMachine = new List<Machine>();
            Console.WriteLine("Введите последовательно информацию о компьютерах в кластере.");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Добавить компьютер в кластер? (y/n)");
                var command = Console.ReadLine();
                if (command == "n")
                {
                    break;
                }
                if (command == "y")
                {
                    var machine = InputMachine();
                    listOfMachine.Add(machine);
                    Console.WriteLine("Компьютер добавлен в кластер!");
                }
                Console.ReadKey();
            }
            Console.Clear();
            if (listOfMachine.Count == 0)
            {
                throw new ApplicationException("Кластер пустой, работа прервана.");
            }

            var newCluster = new Cluster(listOfMachine);
            return newCluster;
        }

        private static Machine InputMachine()
        {
            var processor = InputMachineParameter("Введите модель процессора: ");
            var cores = InputMachineParameter("Введите количество ядер: ");
            var speed = InputMachineParameter("Введите частоту ядра процессора: ");
            var ram = InputMachineParameter("Введите тип и количество ОЗУ: ");
            var hdd = InputMachineParameter("Введите тип и количество ПЗУ: ");
            var video = InputMachineParameter("Введите тип видеосистемы: ");

            var machine = new Machine(processor, cores, speed, ram, hdd, video);
            return machine;
        }

        private static string InputMachineParameter(string message)
        {
            Console.WriteLine(message);
            var param = Console.ReadLine();
            return param;
        }
    
        private static void PrintClusterInfo(Cluster cluster)
        {
            Console.Clear();
            foreach (var machine in cluster.Machinery)
            {
                Console.WriteLine("Параметры компьютера: ");
                Console.WriteLine("Модель процессора: " + machine.ProcessorName);
                Console.WriteLine("Количество ядер: " + machine.CoresNumber);
                Console.WriteLine("Частоту ядра процессора: " + machine.CpuSpeed);
                Console.WriteLine("Тип и количество ОЗУ: " + machine.Ram);
                Console.WriteLine("Тип и количество ПЗУ: " + machine.Hdd);
                Console.WriteLine("Тип видеосистемы: " + machine.VideoSystemType);
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
                Console.ReadKey();
            }
        }
    
        private static void Menu()
        {
            Console.Clear();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Создать и сохранить информацию о кластере в файл (потоки).");
                Console.WriteLine("2 - Создать и сохранить информацию о кластере в файл (LINQ to XML).");
                Console.WriteLine();
                Console.WriteLine("3 - Сравнить файлы.");
                Console.WriteLine();
                Console.WriteLine("0 - Выход");

                var command = Console.ReadLine();
                Cluster clusterWithStream;
                Cluster clusterWithLinq;
                switch (command)
                {
                    case "1" :
                        clusterWithStream = InputCluster();
                        PrintClusterInfo(clusterWithStream);
                        SaveWithStream(clusterWithStream);
                        break;
                    case "2":
                        clusterWithLinq = InputCluster();
                        PrintClusterInfo(clusterWithLinq);
                        SaveWithLinqToXml(clusterWithLinq);
                        break;
                    case "3":
                        clusterWithStream = LoadWithStream();
                        clusterWithLinq = LoadWithLinqToXml();
                        CompareClusters(clusterWithStream, clusterWithLinq);
                        break;
                    case "0":
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Неверная комманда! Нажмите любую клавишу чтобы продолжить...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void CompareClusters(Cluster clusterWithStream, Cluster clusterWithLinq)
        {
            if (clusterWithStream == null)
            {
                throw new Exception("Кластер не создан, невозможно сравнить.");
            }

            if (clusterWithLinq == null)
            {
                throw new Exception("Кластер не создан, невозможно сравнить.");
            }

            if (clusterWithStream.Machinery.Count == 0)
            {
                throw new Exception("Кластер пуст, невозможно сравнить.");
            }

            if (clusterWithLinq.Machinery.Count == 0)
            {
                throw new Exception("Кластер пуст, невозможно сравнить.");
            }

            Console.WriteLine(clusterWithStream.Equals(clusterWithLinq) 
                ? "Файлы одинаковы!" 
                : "Файлы различны!");

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
    }

    [TestClass]
    public class UnitTest
    {

    }
}
