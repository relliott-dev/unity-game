using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class SimpleINI
{
    public string commentChar = "#";
    public List<string> categories;
    public List<VarData> variables;
   
    public void Initialize()
    {
        variables = new List<VarData>();
        categories = new List<string>();
    }

    public void ClearData()
    {
        variables = new List<VarData>();
        categories = new List<string>();
    }

    public string GetStringValue(string category, string variable)
    {
        for (var i = 0; i < variables.Count; i++)
        {
            if (variables[i].category == category && variables[i].variable == variable)
            {
                return variables[i].value;
            }
        }

        return "";
    }

    public void AddSetStringValue(string category1, string variable1, string value1)
    {
        bool found = false;
        for (var i = 0; i < variables.Count; i++)
        {
            if (variables[i].category == category1 && variables[i].variable == variable1)
            {
                variables[i].value = value1;
                found = true;
            }
        }

        if (!found)
        {
            VarData temp = new VarData();
            temp.category = category1;
            temp.variable = variable1;
            temp.value = value1;
            variables.Add(temp);
        }
    }

    public int GetVariableCount(string category)
    {
        int counter = 0;
        for (var i = 0; i < variables.Count; i++)
        {
            if (variables[i].category == category)
                counter++;
        }
        return counter;
    }

    public int GetVariableCount()
    {
        return variables.Count;
    }

    public void AddCategory(string category)
    {
        if (categories != null)
        if (!categories.Contains(category))
            categories.Add(category);
    }
    
    public void LoadIniFile(string file, bool clearData)
    {
        try
        {
            if (clearData)
                ClearData();

            var RawIniData = File.ReadAllLines(file, Encoding.UTF8);
            List<string> parseData = new List<string>(RawIniData);

            string currentCategory = "";
            for (var i = 0; i < parseData.Count; i++)
            {
                string cleanData = CleanStr(parseData[i]);
                if (cleanData.StartsWith("["))
                {
                    string resultCat = "";
                    int index1 = cleanData.IndexOf(']');
                    resultCat = cleanData.Substring(1, index1 - 1);

                    AddCategory(resultCat);

                    currentCategory = resultCat;
                }
                else
                {
                    if (cleanData.Contains("=") && !cleanData.StartsWith(commentChar))
                    {
                        int index1 = cleanData.IndexOf('=');
                        if (index1 < 0)
                            continue;

                        if (cleanData.Contains(commentChar))
                        {
                            int commentIndex = cleanData.IndexOf(commentChar);
                            if (commentIndex > index1)
                            {
                                string variableName = cleanData.Substring(0, index1);
                                string value = cleanData.Substring(index1 + 1, commentIndex - index1 - 1);

                                AddSetStringValue(currentCategory, variableName, value);
                            }
                            else
                                Debug.Log("[SimpleIni] Couldnt Parse Line due to bad comment on line: " + i + ". Text: " + cleanData);
                        }
                        else
                        {
                            string variableName = cleanData.Substring(0, index1);
                            string value = cleanData.Substring(index1 + 1, cleanData.Length - index1 - 1);

                            AddSetStringValue(currentCategory, variableName, value);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("[SimpleIni] Read INI File Error: " + ex.Message);
        }
    }

    public string CleanStr(string input)
    {
        return input.TrimEnd().Replace("\r", "").Replace("\n", "");
    }

    public void WriteIniFile(string file)
    {
        try
        {
            using (StreamWriter sw1 = new StreamWriter(file))
            {
                for (var i = 0; i < categories.Count; i++)
                {
                    sw1.WriteLine("["+categories[i]+"]");
                    for (var x = 0; x < variables.Count; x++)
                    {
                        if (variables[x].category == categories[i])
                        {
                            sw1.WriteLine(variables[x].variable + "=" + variables[x].value);
                        }
                    }
                    sw1.WriteLine("");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("[SimpleIni] Write INI File Error: " + ex.Message);
        }
    }
}

public class VarData
{
    public string variable = "";
    public string category = "";
    public string value = "";
}