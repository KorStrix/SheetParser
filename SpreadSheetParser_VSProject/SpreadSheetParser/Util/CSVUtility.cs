#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2019-10-18 오전 9:44:28
 *	개요 : 
 *	
 *	원본 CSV Serializer를 래핑한 코드입니다.
 *	원본 링크 : https://github.com/sinbad/UnityCsvUtil
 *	유니티 종속성을 제거하였습니다.
   ============================================ */
#endregion Header

using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.ComponentModel;

public static class CSVUtility
{
    public static List<T> FromCSVText_List<T>(byte[] arrCSVByte, Action<string> OnError)
    where T : new()
    {
        using (MemoryStream pStream = new MemoryStream(arrCSVByte))
        {
            using (StreamReader pReader = new StreamReader(pStream))
            {
                return Sinbad.CsvUtil.LoadObjects<T>(pReader, OnError);
            }
        }
    }

    public static List<T> FromCSVFile_List<T>(string strFileName, Action<string> OnError)
        where T : new()
    {
        return Sinbad.CsvUtil.LoadObjects<T>(strFileName, OnError);
    }

    public static void FromCSVFile_List<T>(string strFileName, out List<T> list, Action<string> OnError)
        where T : new()
    {
        list = Sinbad.CsvUtil.LoadObjects<T>(strFileName, OnError);
    }

    public static Dictionary<TKey, TValue> FromCSVFile_Dictionary<TKey, TValue>(string strFileName, Func<TValue, TKey> OnGetKey, Action<string> OnError)
        where TValue : new()
    {
        Dictionary<TKey, TValue> mapReturn = new Dictionary<TKey, TValue>();
        List<TValue> list = Sinbad.CsvUtil.LoadObjects<TValue>(strFileName, OnError);
        for(int i = 0; i < list.Count; i++)
        {
            TValue pValue = list[i];
            TKey pKey = OnGetKey(pValue);

            if(mapReturn.ContainsKey(pKey))
            {
                OnError?.Invoke(string.Format("CSVParser {0}<{1}, {2}> - Key({3}) is Overlap value : {4}, {5}",
                    nameof(FromCSVFile_Dictionary), typeof(TKey).Name, typeof(TValue).Name, pKey, mapReturn[pKey], pValue));

                continue;
            }

            mapReturn.Add(pKey, pValue);
        }

        return mapReturn;
    }

    public static Dictionary<TKey, List<TValue>> FromCSVFile_Dictionary_ValueList<TKey, TValue>(string strFileName, Func<TValue, TKey> OnGetKey, Action<string> OnError)
        where TValue : new()
    {
        Dictionary<TKey, List<TValue>> mapReturn = new Dictionary<TKey, List<TValue>>();
        List<TValue> list = Sinbad.CsvUtil.LoadObjects<TValue>(strFileName, OnError);
        for (int i = 0; i < list.Count; i++)
        {
            TValue pValue = list[i];
            TKey pKey = OnGetKey(pValue);

            if (mapReturn.ContainsKey(pKey) == false)
                mapReturn.Add(pKey, new List<TValue>());

            mapReturn[pKey].Add(pValue);
        }

        return mapReturn;
    }



    public static void FromCSVFile<T>(string strFileName, ref T destObject)
    {
        Sinbad.CsvUtil.LoadObject(strFileName, ref destObject);
    }
}

#region CSVUtility_OriginalCode

namespace Sinbad
{

    // This class uses Reflection and Linq so it's not the fastest thing in the
    // world; however I only use it in development builds where we want to allow
    // game data to be easily tweaked so this isn't an issue; I would recommend
    // you do the same.
    public static class CsvUtil
    {

        // Quote semicolons too since some apps e.g. Numbers don't like them
        static char[] quotedChars = new char[] { ',', ';', '\n' };


        // Load a CSV into a list of struct/classes from a file where each line = 1 object
        // First line of the CSV must be a header containing property names
        // Can optionally include any other columns headed with #foo, which are ignored
        // E.g. you can include a #Description column to provide notes which are ignored
        // This method throws file exceptions if file is not found
        // Field names are matched case-insensitive for convenience
        // @param filename File to load
        // @param strict If true, log errors if a line doesn't have enough
        //   fields as per the header. If false, ignores and just fills what it can
        public static List<T> LoadObjects<T>(string filename, Action<string> OnError) where T : new()
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                using (StreamReader rdr = new StreamReader(stream))
                {
                    return LoadObjects<T>(rdr, OnError);
                }
            }
        }

        // Load a CSV into a list of struct/classes from a stream where each line = 1 object
        // First line of the CSV must be a header containing property names
        // Can optionally include any other columns headed with #foo, which are ignored
        // E.g. you can include a #Description column to provide notes which are ignored
        // Field names are matched case-insensitive for convenience
        // @param rdr Input reader
        // @param strict If true, log errors if a line doesn't have enough
        //   fields as per the header. If false, ignores and just fills what it can
        public static List<T> LoadObjects<T>(TextReader rdr, Action<string> OnError) where T : new()
        {
            List<T> ret = new List<T>();
            string header = rdr.ReadLine();
            Dictionary<string, int> fieldDefs = ParseHeader(header);
            FieldInfo[] fi = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo[] pi = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            bool isValueType = typeof(T).IsValueType;
            string line;
            while ((line = rdr.ReadLine()) != null)
            {
                T obj = new T();
                // box manually to avoid issues with structs
                object boxed = obj;
                if (ParseLineToObject(line, fieldDefs, fi, pi, boxed, OnError))
                {
                    // unbox value types
                    if (isValueType)
                        obj = (T)boxed;
                    ret.Add(obj);
                }
            }
            return ret;
        }

        // Load a CSV file containing fields for a single object from a file
        // No header is required, but it can be present with '#' prefix
        // First column is property name, second is value
        // You can optionally include other columns for descriptions etc, these are ignored
        // If you want to include a header, make sure the first line starts with '#'
        // then it will be ignored (as will any lines that start that way)
        // This method throws file exceptions if file is not found
        // Field names are matched case-insensitive for convenience
        public static void LoadObject<T>(string filename, ref T destObject)
        {
            using (FileStream stream = File.Open(filename, FileMode.Open))
            {
                using (StreamReader rdr = new StreamReader(stream))
                {
                    LoadObject<T>(rdr, ref destObject, null);
                }
            }
        }

        // Load a CSV file containing fields for a single object from a stream
        // No header is required, but it can be present with '#' prefix
        // First column is property name, second is value
        // You can optionally include other columns for descriptions etc, these are ignored
        // Field names are matched case-insensitive for convenience
        public static void LoadObject<T>(TextReader rdr, ref T destObject, Action<string> OnError)
        {
            FieldInfo[] fi = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo[] pi = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            // prevent auto-boxing causing problems with structs
            object nonValueObject = destObject;
            string line;
            while ((line = rdr.ReadLine()) != null)
            {
                // Ignore optional header lines
                if (line.StartsWith("#"))
                    continue;

                string[] vals = EnumerateCsvLine(line).ToArray();
                if (vals.Length >= 2)
                {
                    SetField(RemoveSpaces(vals[0].Trim()), vals[1], fi, pi, nonValueObject);
                }
                else
                {
                    OnError?.Invoke(string.Format("CsvUtil: ignoring line '{0}': not enough fields", line));
                }
            }
            if (typeof(T).IsValueType)
            {
                // unbox
                destObject = (T)nonValueObject;
            }
        }

        // Save a single object to a CSV file
        // Will write 1 line per field, first column is name, second is value
        // This method throws exceptions if unable to write
        public static void SaveObject<T>(T obj, string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.Create))
            {
                using (StreamWriter wtr = new StreamWriter(stream))
                {
                    SaveObject<T>(obj, wtr);
                }
            }
        }

        // Save a single object to a CSV stream
        // Will write 1 line per field, first column is name, second is value
        // This method throws exceptions if unable to write
        public static void SaveObject<T>(T obj, TextWriter w)
        {
            FieldInfo[] fi = typeof(T).GetFields();
            bool firstLine = true;
            foreach (FieldInfo f in fi)
            {
                // Good CSV files don't have a trailing newline so only add here
                if (firstLine)
                    firstLine = false;
                else
                    w.Write(Environment.NewLine);

                w.Write(f.Name);
                w.Write(",");
                string val = f.GetValue(obj).ToString();
                // Quote if necessary
                if (val.IndexOfAny(quotedChars) != -1)
                {
                    val = string.Format("\"{0}\"", val);
                }
                w.Write(val);
            }
        }

        // Save multiple objects to a CSV file
        // Writes a header line with field names, followed by one line per
        // object with each field value in each column
        // This method throws exceptions if unable to write
        public static void SaveObjects<T>(IEnumerable<T> objs, string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.Create))
            {
                using (StreamWriter wtr = new StreamWriter(stream))
                {
                    SaveObjects<T>(objs, wtr);
                }
            }
        }

        // Save multiple objects to a CSV stream
        // Writes a header line with field names, followed by one line per
        // object with each field value in each column
        // This method throws exceptions if unable to write
        public static void SaveObjects<T>(IEnumerable<T> objs, TextWriter w)
        {
            FieldInfo[] fi = typeof(T).GetFields();
            WriteHeader<T>(fi, w);

            bool firstLine = true;
            foreach (T obj in objs)
            {
                // Good CSV files don't have a trailing newline so only add here
                if (firstLine)
                    firstLine = false;
                else
                    w.Write(Environment.NewLine);

                WriteObjectToLine(obj, fi, w);

            }
        }

        private static void WriteHeader<T>(FieldInfo[] fi, TextWriter w)
        {
            bool firstCol = true;
            foreach (FieldInfo f in fi)
            {
                // Good CSV files don't have a trailing comma so only add here
                if (firstCol)
                    firstCol = false;
                else
                    w.Write(",");

                w.Write(f.Name);
            }
            w.Write(Environment.NewLine);
        }

        private static void WriteObjectToLine<T>(T obj, FieldInfo[] fi, TextWriter w)
        {
            bool firstCol = true;
            foreach (FieldInfo f in fi)
            {
                // Good CSV files don't have a trailing comma so only add here
                if (firstCol)
                    firstCol = false;
                else
                    w.Write(",");

                string val = f.GetValue(obj).ToString();
                // Quote if necessary
                if (val.IndexOfAny(quotedChars) != -1)
                {
                    val = string.Format("\"{0}\"", val);
                }
                w.Write(val);
            }
        }

        // Parse the header line and return a mapping of field names to column
        // indexes. Columns which have a '#' prefix are ignored.
        private static Dictionary<string, int> ParseHeader(string header)
        {
            Dictionary<string, int> headers = new Dictionary<string, int>();
            int n = 0;
            foreach (string field in EnumerateCsvLine(header))
            {
                string trimmed = field.Trim();
                if (!trimmed.StartsWith("#"))
                {
                    trimmed = RemoveSpaces(trimmed);
                    headers[trimmed] = n;
                }
                ++n;
            }
            return headers;
        }

        // Parse an object line based on the header, return true if any fields matched
        private static bool ParseLineToObject(string line, Dictionary<string, int> fieldDefs, FieldInfo[] fi, PropertyInfo[] pi, object destObject, Action<string> OnError)
        {

            string[] values = EnumerateCsvLine(line).ToArray();
            bool setAny = false;
            foreach (string field in fieldDefs.Keys)
            {
                int index = fieldDefs[field];
                if (index < values.Length)
                {
                    string val = values[index];
                    setAny = SetField(field, val, fi, pi, destObject) || setAny;
                }
                else if (OnError != null)
                {
                    OnError.Invoke(string.Format("CsvUtil: error parsing line '{0}': not enough fields", line));
                }
            }
            return setAny;
        }

        private static bool SetField(string fieldName, string val, FieldInfo[] fi, PropertyInfo[] pi, object destObject)
        {
            bool result = false;
            foreach (PropertyInfo p in pi)
            {
                // Case insensitive comparison
                if (string.Compare(fieldName, p.Name, true) == 0)
                {
                    // Might need to parse the string into the property type
                    object typedVal = p.PropertyType == typeof(string) ? val : ParseString(val, p.PropertyType);
                    p.SetValue(destObject, typedVal, null);
                    result = true;
                    break;
                }
            }
            foreach (FieldInfo f in fi)
            {
                // Case insensitive comparison
                if (string.Compare(fieldName, f.Name, true) == 0)
                {
                    // Might need to parse the string into the field type
                    object typedVal = f.FieldType == typeof(string) ? val : ParseString(val, f.FieldType);
                    f.SetValue(destObject, typedVal);
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static object ParseString(string strValue, Type t)
        {
            TypeConverter cv = TypeDescriptor.GetConverter(t);
            return cv.ConvertFromInvariantString(strValue);
        }

        private static IEnumerable<string> EnumerateCsvLine(string line)
        {
            // Regex taken from http://wiki.unity3d.com/index.php?title=CSVReader
            foreach (Match m in Regex.Matches(line,
                @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                RegexOptions.ExplicitCapture))
            {
                yield return m.Groups[1].Value;
            }
        }

        private static string RemoveSpaces(string strValue)
        {
            return Regex.Replace(strValue, @"\s", string.Empty);
        }
    }
}
#endregion