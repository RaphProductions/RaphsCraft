using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Runtime.Loader;

namespace RaphsCraft;

class Program
{
    static void Main()
    {
        new RaphsCraft.Client.Game();
    }

    /*
     * using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

class Program
{
    static void Main()
    {
        // Paths for the original and injected assemblies
        string assemblyPath = "path/to/ExternalAssemblies/RaphsCraft.Client.dll";  // Adjust this path
        string outputPath = "path/to/output/RaphsCraft.Client_Injected.dll";       // Path for the modified assembly
        
        // Load the assembly you want to modify (injected assembly)
        var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);
        var module = assembly.MainModule;

        // Find the target class to modify (e.g., "RaphsCraft.Client.Game")
        var targetClass = module.GetType("RaphsCraft.Client.Game");

        // Find the source class and method to copy (e.g., copying from "RaphsCraft.Client.AnotherClass")
        var sourceClass = module.GetType("RaphsCraft.Client.AnotherClass");
        var sourceMethod = sourceClass.Methods.First(m => m.Name == "MethodToCopy");

        // Create a new method definition with the same name, parameters, and return type
        var copiedMethod = new MethodDefinition(
            sourceMethod.Name,
            MethodAttributes.Public | MethodAttributes.Static,  // Make the method static (or change as needed)
            module.ImportReference(sourceMethod.ReturnType)
        );

        // Copy the IL instructions from the source method to the new method
        var processor = copiedMethod.Body.GetILProcessor();
        foreach (var instruction in sourceMethod.Body.Instructions)
        {
            processor.Append(instruction);  // Copy each instruction
        }

        // Add the copied method to the target class
        targetClass.Methods.Add(copiedMethod);

        // Save the modified assembly to a new file
        assembly.Write(outputPath);

        // Optionally, load and invoke the new method dynamically
        Assembly loadedAssembly = Assembly.LoadFrom(outputPath);
        Type targetType = loadedAssembly.GetType("RaphsCraft.Client.Game");
        var method = targetType.GetMethod("MethodToCopy");
        method.Invoke(null, null);  // Assuming the method is static
    }
}

     * */
}
