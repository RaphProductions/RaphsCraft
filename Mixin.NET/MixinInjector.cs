using Mixin.NET.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mixin.NET;

internal class MixinInjector
{
    private AssemblyDefinition _inputAssembly;
    private AssemblyDefinition _sourceAssembly;

    public MixinInjector(AssemblyDefinition inputAssembly, AssemblyDefinition sourceAssembly)
    {
        _inputAssembly = inputAssembly;
        _sourceAssembly = sourceAssembly;
    }

    private void InjectFromMethod(
        MethodDefinition md, 
        MethodDefinition toInject,
        InjectAt at,
        TypeDefinition target,
        ModuleDefinition mod)
    {
        var copiedMethod = new MethodDefinition(
            md.Name,
            md.Attributes,
            mod.ImportReference(md.ReturnType)
        );

        var processor = copiedMethod.Body.GetILProcessor();
        foreach (var instruction in md.Body.Instructions)
        {
            processor.Append(instruction);  // Copy each instruction
        }

        var toInject_processor = toInject.Body.GetILProcessor();
        var toInject_firstInstruction = toInject.Body.Instructions[0];

        // Create new instructions and insert them at the start
        processor.InsertBefore(toInject_firstInstruction, processor.Create(OpCodes.Ldstr, "Hijacked code."));
        //processor.InsertBefore(toInject_firstInstruction, processor.Create(OpCodes.Call, module.ImportReference(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }))));

    }

    private void InjectFromType(TypeDefinition t, TypeReference target, ModuleDefinition mod)
    {
        TypeDefinition targetDef = _inputAssembly.MainModule.GetType(target.FullName);

        if (targetDef == null)
            throw new Exception("The specified type doesn't exist in the input assembly.");

        foreach (MethodDefinition md in t.GetMethods())
        {
            foreach (var attr in md.CustomAttributes)
            {
                if (attr.AttributeType.FullName == typeof(InjectAttribute).FullName)
                {
                    InjectAt? at = null;
                    string? sourceMethodName = null; 

                    foreach (var field in attr.Fields)
                    {
                        if (field.Name == "_at")
                        {
                            // Get the value of _classType
                            at = (InjectAt)field.Argument.Value;
                        }
                    }

                    if (at != null && sourceMethodName != null)
                    {
                        MethodDefinition sourceMethod = null;

                        foreach (var sm in targetDef.GetMethods())
                        {
                            if (sm.Name == sourceMethodName)
                            {
                                sourceMethod = sm;
                            }
                        }

                        if (sourceMethod == null)
                            throw new Exception("Source method is null");


                        InjectFromMethod(
                            md,
                            sourceMethod,
                            (InjectAt)at,
                            targetDef,
                            mod);
                    }
                }
            }
        }
    }

    public void Inject()
    {
        foreach (var mod in _sourceAssembly.Modules)
        {
            foreach (var type in mod.GetTypes())
            {
                foreach (var attr in type.CustomAttributes)
                {
                    if (attr.AttributeType.FullName == typeof(MixinAttribute).FullName)
                    {
                        foreach (var field in attr.Fields)
                        {
                            if (field.Name == "_classType")
                            {
                                // Get the value of _classType
                                var classTypeReference = (TypeReference)field.Argument.Value;
                                
                                InjectFromType(type, classTypeReference, mod);
                            }
                        }
                    }
                }
            }
        }
    }
}
