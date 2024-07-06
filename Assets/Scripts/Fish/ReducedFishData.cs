using System;
using Enums;

namespace Fish
{
    public class ReducedFishData
    {
        public ReducedFishData(FishData data, FishSize size)
        {
            Data = data;
            Size = size;
        }

        public readonly FishData Data;
        public readonly FishSize Size;
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ReducedFishData other = (ReducedFishData)obj;
            return Data == other.Data && Size == other.Size;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data, Size);
        }
    }
}