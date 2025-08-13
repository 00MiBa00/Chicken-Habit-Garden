using System;

namespace Datas.Habits
{
    [Serializable]
    public class LastDayHabitData
    {
        public string Ymd;
        public bool IsClosed;
        public HabitsData Habits;
    }
}