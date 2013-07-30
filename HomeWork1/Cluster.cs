using System;
using System.Collections.Generic;

namespace HomeWork1
{
    public class Cluster : IEquatable<Cluster>
    {
        public List<Machine> Machinery;

        public Cluster()
        {
            Machinery = new List<Machine>();
        }

        public Cluster(List<Machine> curCluster)
        {
            Machinery = curCluster;
        }

        public bool Equals(Cluster otherCluster)
        {
            var flag = false;
            foreach (var machine in Machinery)
            {
                flag = false;
                foreach (var machine1 in otherCluster.Machinery)
                {
                    flag = machine.Equals(machine1);
                    if (flag)
                    {
                        break;
                    }
                }
                if (!flag)
                {
                    return false;
                }
            }
            return flag && (Machinery.Count == otherCluster.Machinery.Count);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Cluster)) return false;
            return Equals((Cluster)obj);
        }
    }
}
