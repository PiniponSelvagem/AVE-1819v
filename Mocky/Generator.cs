using Mocky;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

public class Generator {
    private readonly Dictionary<Type, Type> calculators = new Dictionary<Type, Type>();

    public Generator() {
    }

    public Type For(Type type, Dictionary<string, MockMethod> ms) {
        if (!calculators.ContainsKey(type)) {
            calculators[type] = GenerateFor(type, ms);
        }
        return calculators[type];
    }

    private Type GenerateFor(Type type, Dictionary<string, MockMethod> ms) {
        string TheName = "Mock" + type.Name;

        string ASM_NAME = TheName;
        string MOD_NAME = TheName;
        string TYP_NAME = TheName;
        string DLL_NAME = TheName + ".dll";
        AssemblyBuilder asmBuilder;
        TypeBuilder typeBuilder;
        CreateAssemblyBuilderAndTypeBuilder(
            ASM_NAME,
            MOD_NAME,
            TYP_NAME,
            DLL_NAME,
            out asmBuilder, out typeBuilder);

        // Make the type implement the interfaces
        typeBuilder.AddInterfaceImplementation(type);
        FieldBuilder fieldBuilder =
            typeBuilder.DefineField(
                "obj",
                typeof(MockMethod[]),
                FieldAttributes.Private
            );
        CreateContructor(type, typeBuilder, fieldBuilder);

        // Generate methods
        ImplementMethods(typeBuilder, fieldBuilder, type, ms); //for this type
        Type[] typeInterfaces = type.GetInterfaces();
        for (int i = 0; i<typeInterfaces.Length; ++i) {
            ImplementMethods(typeBuilder, fieldBuilder, typeInterfaces[i], null);
        }

        // Create type 
        Type retType = typeBuilder.CreateTypeInfo().AsType();

        // Save the assembly
        //asmBuilder.Save(DLL_NAME);

        return retType;
    }


    private void CreateAssemblyBuilderAndTypeBuilder(string ASM_NAME, string MOD_NAME, string TYP_NAME, string DLL_NAME,
                                        out AssemblyBuilder asmBuilder, out TypeBuilder typBuilder) {
        // Define assembly
        asmBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(ASM_NAME),
                //AssemblyBuilderAccess.RunAndSave
                AssemblyBuilderAccess.Run
        );

        // Define module in assembly
        ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME);    //asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);

        // Define type in module
        typBuilder = modBuilder.DefineType(TYP_NAME);
    }

    private void CreateContructor(Type type, TypeBuilder typeBuilder, FieldBuilder fieldBuilder) {
        ConstructorBuilder ctorBuilder =
            typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new Type[1] { typeof(MockMethod[]) }
            );

        ILGenerator il = ctorBuilder.GetILGenerator();
        LocalBuilder tobj = il.DeclareLocal(type);

        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Call, typeof(Object).GetConstructor(Type.EmptyTypes));
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Stfld, fieldBuilder);
        il.Emit(OpCodes.Ret);
    }

    private void ImplementMethods(TypeBuilder typeBuilder, FieldBuilder fieldBuilder, Type type, Dictionary<string, MockMethod> ms) {
        MethodInfo[] methods = type.GetMethods(
            BindingFlags.Public       |
            BindingFlags.Instance     |
            BindingFlags.DeclaredOnly
        );

        ConstructorInfo ctorNIE = typeof(NotImplementedException).GetConstructor(Type.EmptyTypes);

        for (int i=0; i<methods.Length; ++i) {
            MethodInfo method = methods[i];
            EmitMethod(typeBuilder, fieldBuilder, type, method, ctorNIE, ms);
        }
    }

    private void EmitMethod(TypeBuilder typeBuilder, FieldBuilder fieldBuilder, Type type, MethodInfo method,
                                        ConstructorInfo ctorNIE, Dictionary<string, MockMethod> ms) {
        ParameterInfo[] pInfo = method.GetParameters();
        Type[] parms = new Type[pInfo.Length];
        for (int j = 0; j<parms.Length; ++j) {
            parms[j] = pInfo[j].ParameterType;
        }

        MethodBuilder metBuilder =
            typeBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public    |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot   |
                MethodAttributes.Virtual   |
                MethodAttributes.Final,
                method.ReturnType,
                parms
            );

        ILGenerator il = metBuilder.GetILGenerator();
        LocalBuilder tobj = il.DeclareLocal(type);

        int methodIdx = GetIndexOfKey(ms, method.Name);
        if (ms==null || methodIdx<0) {
            il.Emit(OpCodes.Newobj, ctorNIE);
            il.Emit(OpCodes.Throw);
        }
        else {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldBuilder);
            il.Emit(OpCodes.Ldc_I4, methodIdx);                 //pos array
            il.Emit(OpCodes.Ldelem_Ref);
            il.Emit(OpCodes.Ldc_I4_2);
            il.Emit(OpCodes.Newarr, typeof(Object));
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Box, typeof(Int32));
            il.Emit(OpCodes.Stelem_Ref);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Box, typeof(Int32));
            il.Emit(OpCodes.Stelem_Ref);
            il.EmitCall(OpCodes.Callvirt, typeof(MockMethod).GetMethod("Call"), new Type[] { typeof(Object[]) });        //callvirt instance object[Mocky] Mocky.MockMethod::Call(object[])
            il.Emit(OpCodes.Unbox_Any, typeof(Int32));
            il.Emit(OpCodes.Ret);
        }
    }


    ////////////////////////

    private int GetIndexOfKey(Dictionary<string, MockMethod> tempDict, String key) {
        int index = -1;
        if (tempDict != null) {
            foreach (String value in tempDict.Keys) {
                ++index;
                if (key == value)
                    return index;
            }
        }
        return -1;
    }
}
