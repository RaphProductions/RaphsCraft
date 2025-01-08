using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaphsCraft.Client.Resource;

public enum ResourceType
{
    Shaders,
    Textures,
    Lang
}

public class ResourceManager
{
    static string AssetsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets");

    public static byte[] Get(ResourceType rt, string namesp, string path)
    {
        string path2 = Path.Combine(AssetsFolder, namesp, rt.ToString(), path);
        return File.ReadAllBytes(path2);
    }

    public static string GetString(ResourceType rt, string namesp, string path)
    {
        string path2 = Path.Combine(AssetsFolder, namesp, rt.ToString().ToLower(), path);
        return File.ReadAllText(path2);
    }
    public static string GetPath(ResourceType rt, string namesp, string path)
    {
        string path2 = Path.Combine(AssetsFolder, namesp, rt.ToString().ToLower(), path);
        return path2;
    }
}
