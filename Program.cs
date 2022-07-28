﻿using Newtonsoft.Json.Linq;
using FreneticUtilities.FreneticExtensions;
using System.Text;

#region file grab
string file = args.Length > 0 ? args[0] : "./ReflectionMappingsInfo.java";
if (!File.Exists(file))
{
    Console.WriteLine("Missing ReflectionMappingsInfo.java file");
    return;
}
string fileContent = File.ReadAllText(file);
string[] fileLines = fileContent.Replace("\r", "").Split('\n');
string packageLine = fileLines.First(l => l.StartsWith("package "));
string classLine = fileLines.First(l => l.StartsWith("public class "));
List<(string, List<string>)> requiredLabels = new();
List<string> curSet = null;
foreach (string line in fileLines)
{
    if (!line.StartsWith("    "))
    {
        continue;
    }
    string cleanLine = line.Trim();
    if (cleanLine.StartsWith("// net.minecraft."))
    {
        curSet = new();
        requiredLabels.Add((cleanLine.After("// "), curSet));
    }
    else if (cleanLine.StartsWith("//"))
    {
        continue;
    }
    else if (cleanLine.StartsWith("public static String "))
    {
        curSet.Add(cleanLine.After("public static String ").Before(' '));
    }
}
#endregion

#region download and gather
const string ManifestUrl = "https://piston-meta.mojang.com/mc/game/version_manifest.json";
HttpClient client = new();

Console.WriteLine("Downloading...");
string manifestText = client.GetAsync(ManifestUrl).Result.Content.ReadAsStringAsync().Result;
JObject manifestObj = JObject.Parse(manifestText);

string latest = manifestObj["latest"]["release"].ToString();
Console.WriteLine($"Latest version = {latest}");
Console.Write("Which version to use? ");
string version = Console.ReadLine();

if (manifestObj["versions"].ToArray().FirstOrDefault(t => t["id"].ToString() == version) is not JObject versionObj)
{
    Console.WriteLine($"Failed: version {version} does not exist.");
    return;
}

Console.WriteLine($"Found version {version}, downloading version data...");
string versionData = client.GetAsync(versionObj["url"].ToString()).Result.Content.ReadAsStringAsync().Result;
JObject versionDataObj = JObject.Parse(versionData);
string mappingURL = versionDataObj["downloads"]["server_mappings"]["url"].ToString();

Console.WriteLine("Have mappings path, downloading...");
string mappingsData = client.GetAsync(mappingURL).Result.Content.ReadAsStringAsync().Result;

Console.WriteLine("Parsing...");
Dictionary<string, Dictionary<string, string>> mappings = new();
Dictionary<string, string> currentClass = null;
foreach (string line in mappingsData.Replace("\r", "").Split('\n'))
{
    if (line.StartsWith("#"))
    {
        continue;
    }
    if (!line.StartsWith("    "))
    {
        currentClass = new Dictionary<string, string>();
        mappings.Add(line.Before(" -> "), currentClass);
    }
    else
    {
        string name = line.Trim().After(' ').BeforeAndAfter(" -> ", out string after);
        if (name.Contains('('))
        {
            name = name.Before('(') + "_method";
        }
        currentClass[name] = after;
    }
}
#endregion

#region process
File.Delete(file + ".bak");
File.Copy(file, file + ".bak");
StringBuilder output = new();
output.Append($"{packageLine}\n\n{classLine}\n\n    // Content generated by ReflectionMappingsGenerator - https://github.com/DenizenScript/ReflectionMappingsGenerator\n");
int success = 0, fail = 0;
foreach ((string className, List<string> set) in requiredLabels)
{
    output.Append($"\n    // {className}\n");
    foreach (string target in set)
    {
        string id = target.After('_');
        if (mappings.TryGetValue(className, out var mappingSet) && mappingSet.TryGetValue(id, out string mapped))
        {
            output.Append($"    public static String {target} = \"{mapped}\";\n");
            success++;
        }
        else
        {
            output.Append($"    public static String {target} = ERROR_UNKNOWN_TARGET;\n");
            fail++;
        }
    }
}
output.Append("\n}\n");
File.WriteAllText(file, output.ToString().Replace("\n", Environment.NewLine));
Console.WriteLine($"Done! {success} gathered and {fail} failed");
#endregion
