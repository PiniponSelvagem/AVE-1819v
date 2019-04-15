using Mocky;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

public class Generator {
    private static readonly Dictionary<Type, Type> calculators =
        new Dictionary<Type, Type>();

    public static Type For(Type type) {
        if (!calculators.ContainsKey(type)) {
            calculators[type] = GenerateFor(type);
        }
        return calculators[type];
    }

    private static Type GenerateFor(Type type) {
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
        CreateContructor(type, typeBuilder);

        // Generate methods
        ImplementMethods(typeBuilder, type); //for this type
        Type[] typeInterfaces = type.GetInterfaces();
        for (int i = 0; i<typeInterfaces.Length; ++i) {
            ImplementMethods(typeBuilder, typeInterfaces[i]);
        }

        // Create type 
        Type retType = typeBuilder.CreateTypeInfo().AsType();

        // Save the assembly
        asmBuilder.Save(DLL_NAME);

        return retType;
    }


    private static void CreateAssemblyBuilderAndTypeBuilder(string ASM_NAME, string MOD_NAME, string TYP_NAME, string DLL_NAME,
                                        out AssemblyBuilder asmBuilder, out TypeBuilder typBuilder) {
        // Define assembly
        asmBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(ASM_NAME),
                AssemblyBuilderAccess.RunAndSave
        );

        // Define module in assembly
        ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);

        // Define type in module
        typBuilder = modBuilder.DefineType(TYP_NAME);
    }

    private static void CreateContructor(Type type, TypeBuilder typBuilder) {
        FieldBuilder fieldBuilder =
            typBuilder.DefineField(
                "obj",
                typeof(MockMethod[]),
                FieldAttributes.Private
            );

        ConstructorBuilder ctorBuilder =
            typBuilder.DefineConstructor(
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

    private static void ImplementMethods(TypeBuilder typBuilder, Type type) {
        MethodInfo[] methods = type.GetMethods(
            BindingFlags.Public       |
            BindingFlags.Instance     |
            BindingFlags.DeclaredOnly
        );

        ConstructorInfo ctorNIE = typeof(NotImplementedException).GetConstructor(Type.EmptyTypes);

        for (int i=0; i<methods.Length; ++i) {
            ParameterInfo[] pInfo = methods[i].GetParameters();
            Type[] parms = new Type[pInfo.Length];
            for (int j=0; j<parms.Length; ++j) {
                parms[j] = pInfo[j].ParameterType;
            }

            MethodBuilder metBuilder =
            typBuilder.DefineMethod(
                methods[i].Name,
                MethodAttributes.Public    |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot   |
                MethodAttributes.Virtual   |
                MethodAttributes.Final,
                methods[i].ReturnType,
                parms
            );

            ILGenerator il = metBuilder.GetILGenerator();
            LocalBuilder tobj = il.DeclareLocal(type);

            il.Emit(OpCodes.Newobj, ctorNIE);
            il.Emit(OpCodes.Throw);
        }
    }
}
