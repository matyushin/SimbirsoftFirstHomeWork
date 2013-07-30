using System;
using System.Globalization;

namespace HomeWork1
{
    public class Machine : IEquatable<Machine>
    {
        public string ProcessorName { get; set; }

        public string CoresNumber { get; set; }

        public string CpuSpeed { get; set; }

        public string Ram { get; set; }

        public string Hdd { get; set; }

        public string VideoSystemType { get; set; }

        public Machine()
        {
            ProcessorName = "";
            CoresNumber = "";
            CpuSpeed = "";
            Ram = "";
            Hdd = "";
            VideoSystemType = ""; 
        }

        public Machine(string processor, string cores, string speed, string ram, string hdd, string video)
        {
            ProcessorName = processor.ToString(CultureInfo.InvariantCulture);
            CoresNumber = cores.ToString(CultureInfo.InvariantCulture);
            CpuSpeed = speed.ToString(CultureInfo.InvariantCulture);
            this.Ram = ram.ToString(CultureInfo.InvariantCulture);
            this.Hdd = hdd.ToString(CultureInfo.InvariantCulture);
            VideoSystemType = video.ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(Machine otherMachine)
        {
            if (ReferenceEquals(null, otherMachine)) return false;
            if (ReferenceEquals(this, otherMachine)) return true;
            return (
                    string.Equals(ProcessorName, otherMachine.ProcessorName) 
                    && string.Equals(CoresNumber, otherMachine.CoresNumber) 
                    && string.Equals(CpuSpeed, otherMachine.CpuSpeed) 
                    && string.Equals(Ram, otherMachine.Ram) 
                    && string.Equals(Hdd, otherMachine.Hdd) 
                    && string.Equals(VideoSystemType, otherMachine.VideoSystemType)
                );
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Machine)) return false;
            return Equals((Machine) obj);
        }

        #region Останки инкапсуляции
        /*

        void SetProcessorName(string processor)
        {
            processorName = processor;
        }

        void SetCoresNumber(string cores)
        {
            coresNumber = cores;
        }

        void SetCpuSpeed(string speed)
        {
            cpuSpeed = speed;
        }

        void SetRam(string ram)
        {
            this.ram = ram;
        }

        void SetHdd(string hdd)
        {
            this.hdd = hdd;
        }
        void SetVideoSystemType(string video)
        {
            videoSystemType = video;
        }

        string GetProcessorName()
        {
            return processorName;
        }

        string GetCoresNumber()
        {
            return coresNumber;
        }

        string GetCpuSpeed()
        {
            return cpuSpeed;
        }

        string GetRam()
        {
            return ram;
        }

        string GetHdd()
        {
            return hdd;
        }

        string GetVideoSystemType()
        {
            return videoSystemType;
        }*/
        #endregion
    }
}
