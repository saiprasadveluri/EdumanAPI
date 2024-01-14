using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Models
{
    public class TimetableViewVM
    {
        public List<TimetableViewHourData> DayDataLst { get; set; } = new List<TimetableViewHourData>();
        public List<int> WDaysList { get; set; } = new List<int>();
        public List<int> WHourList { get; set; } = new List<int>();
    }

    public class TimetableViewHourData
    {
        private string[] Days = { "Monday", "Tueseday", "Wed", "Thurs", "Fri", "Sat" };
        public int WNo { get; set; }
        public string DayName { get { return Days[WNo - 1]; } }
        public List<ViewTimeTableEntryVM> HourDataLst { get; set; } = new List<ViewTimeTableEntryVM>();
    }

    public class ViewTimeTableEntryVM
    {
        public int WeekNo { get; set; }
        public int HourNo { get; set; }
        public string SubName { get; set; }
        public string TeacherName { get; set; }
    }
}
