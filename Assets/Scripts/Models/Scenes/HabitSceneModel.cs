using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Datas.Habits;

namespace Models.Scenes
{
    public class HabitSceneModel
    {
        private const string FILE_TODAY   = "habits_today.json";
        private const string FILE_LASTDAY = "habits_last_day.json";
        private const string KEY_STREAK   = "HabitSceneModel.Streak";
        private const string LastSnapshotKey = "HabitSceneModel.LastSnapshotYmd";

        private static string PathToday   => Path.Combine(Application.persistentDataPath, FILE_TODAY);
        private static string PathLastDay => Path.Combine(Application.persistentDataPath, FILE_LASTDAY);

        public int UnlockedElementsCount => Streak / 2; 
        public int Streak { get; private set; }
        public string TodayYmd => DateTime.Now.Date.ToString("yyyy-MM-dd");
        public HabitsData Today { get; private set; } = new HabitsData();

        public struct InitResult
        {
            public bool DayRolled;
            public bool LastDayClosed;
            public int  NewStreak;
            public int  DeltaDays;
        }
        
        public InitResult Init()
        {
            LoadStreak();
            LoadTodayOrCreate();
            var last = LoadLastDay();

            var result = new InitResult
            {
                DayRolled     = false,
                LastDayClosed = false,
                NewStreak     = Streak,
                DeltaDays     = 0
            };

            if (last == null) return result;

            if (!TryParseYmd(last.Ymd, out var lastDate)) return result;

            var delta = ComputeDeltaDays(lastDate, DateTime.Now.Date);
            result.DeltaDays = delta;

            if (!HasDayRolled(delta)) return result;

            result.DayRolled     = true;
            result.LastDayClosed = last.IsClosed;

            ApplyStreakRule(delta, last.IsClosed);
            result.NewStreak = Streak;            
            StartNewTodayFromExistingNames();     
            SaveToday();                          
            SaveStreak();                         

            return result;
        }
        
        public int TodayTotalCount =>
            Today?.Habits?.Count ?? 0;

        public int TodayCompletedCount =>
            Today?.Habits?.Count(h => h.IsСompleted) ?? 0;
        
        public bool CloseTodayAndMakeSnapshot()
        {
            try
            {
                bool closed = AreAllCompleted(Today);
                var snapshot = new LastDayHabitData
                {
                    Ymd      = TodayYmd,
                    IsClosed = closed,
                    Habits   = new HabitsData {
                        Habits = Today.Habits
                            .Select(h => new HabitData { Name = h.Name, IsСompleted = h.IsСompleted })
                            .ToList()
                    }
                };
                Save(PathLastDay, snapshot);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[HabitSceneModel] CloseTodayAndMakeSnapshot failed: {e}");
                return false;
            }
        }
        
        public bool TrySnapshotToday()
        {
            string lastYmd = PlayerPrefs.GetString(LastSnapshotKey, "");

            if (lastYmd == TodayYmd) return false;
            
            bool ok = CloseTodayAndMakeSnapshot();
            if (ok)
            {
                PlayerPrefs.SetString(LastSnapshotKey, TodayYmd);
                PlayerPrefs.Save();
            }
            return ok;
        }
        
        public bool AddHabit(string name)
        {
            name = Normalize(name);
            if (string.IsNullOrEmpty(name)) return false;
            if (Today.Habits.Any(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase)))
                return false;

            Today.Habits.Add(new HabitData { Name = name, IsСompleted = false });
            SaveToday();
            return true;
        }

        public void ClearHabits(bool clearLastDaySnapshot = true)
        {
            if (Today?.Habits != null)
                Today.Habits.Clear();

            SaveToday();
            
            if (clearLastDaySnapshot)
            {
                try
                {
                    if (File.Exists(PathLastDay)) File.Delete(PathLastDay);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[HabitSceneModel] Failed to delete last day snapshot: {e}");
                }
            }
        }

        public bool SetCompletedByIndex(int index, bool completed)
        {
            if (Today?.Habits == null) return false;
            if (index < 0 || index >= Today.Habits.Count) return false;

            Today.Habits[index].IsСompleted = completed;
            SaveToday();
            return true;
        }

        public bool IsCompleted(string name) => FindByName(name)?.IsСompleted == true;

        public void ResetStreak()
        {
            Streak = 0;
            SaveStreak();
        }
        
        private void LoadStreak() => Streak = PlayerPrefs.GetInt(KEY_STREAK, 10);

        private void SaveStreak()
        {
            PlayerPrefs.SetInt(KEY_STREAK, Streak);
            PlayerPrefs.Save();
        }

        private void LoadTodayOrCreate()
        {
            var loaded = Load<HabitsData>(PathToday, () => new HabitsData());
            Today = loaded ?? new HabitsData();
            if (Today.Habits == null) Today.Habits = new List<HabitData>();
        }

        private LastDayHabitData LoadLastDay() => Load<LastDayHabitData>(PathLastDay, () => null);

        private static int ComputeDeltaDays(DateTime last, DateTime today) => (today - last).Days;

        private static bool HasDayRolled(int delta) => delta >= 1;

        private void ApplyStreakRule(int delta, bool lastClosed)
        {
            Streak = (delta == 1 && lastClosed) ? (Streak + 1) : 0;
        }

        private void StartNewTodayFromExistingNames()
        {
            var names = Today.Habits.Select(h => h.Name).ToList();
            Today = new HabitsData
            {
                Habits = names.Select(n => new HabitData { Name = n, IsСompleted = false }).ToList()
            };
        }

        private static bool AreAllCompleted(HabitsData data)
        {
            return data != null
                   && data.Habits != null
                   && data.Habits.Count > 0
                   && data.Habits.All(h => h.IsСompleted);
        }
        
        private HabitData FindByName(string name)
        {
            name = Normalize(name);
            return Today.Habits.FirstOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private void SaveToday() => Save(PathToday, Today);

        private static T Load<T>(string path, Func<T> @default)
        {
            try
            {
                if (!File.Exists(path)) return @default();
                var json = File.ReadAllText(path);
                if (string.IsNullOrEmpty(json)) return @default();
                var obj = JsonUtility.FromJson<T>(json);
                return obj == null ? @default() : obj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[HabitSceneModel] Load<{typeof(T).Name}> failed: {e}");
                return @default();
            }
        }

        private static void Save<T>(string path, T data)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);
#if UNITY_EDITOR
                Debug.Log($"[HabitSceneModel] Saved {typeof(T).Name} -> {path}");
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"[HabitSceneModel] Save<{typeof(T).Name}> failed: {e}");
            }
        }

        private static bool TryParseYmd(string ymd, out DateTime date)
        {
            date = DateTime.MinValue;
            if (string.IsNullOrEmpty(ymd)) return false;
            return DateTime.TryParse(ymd, out date) ? (date = date.Date) == date : false;
        }

        private static string Normalize(string s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();
    }
}