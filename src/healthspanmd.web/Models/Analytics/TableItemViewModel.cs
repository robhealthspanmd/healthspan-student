using System;

namespace healthspanmd.web.Models.Analytics
{
    public class TableItemViewModel
    {
        public DateTime TrackingDate { get; set; }
        public int? Value1 { get; set; }
        public int? Value2 { get; set; }
        public int Goal1 { get; set; }
        public int Goal2 { get; set; }
        public bool BiggerIsBetter { get; set; }

        public bool MeetsGoal
        {
            get
            {
                if (BiggerIsBetter)
                {
                    if (Value1 >= Goal1 && Value2 >= Goal2)
                    {
                        return true;
                    }
                }
                else
                {
                    if (Value1 <= Goal1 && Value2 <= Goal2)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
