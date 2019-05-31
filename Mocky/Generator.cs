
//#define DEBUG //uncomment to save the DLLs generated

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Mocky {
    public class Generator {

        private readonly Type klass;

        private Boolean isGenerated = false;
        private Type genType;

        public Generator(Type klass) {
            this.klass = klass;
        }

        public void Reset() {
            isGenerated = false;
            genType = null;
        }

        public Type GenWith(Dictionary<string, MockMethod> mockMethods) {
            if (!isGenerated) {
                genType = GenerateWith(mockMethods);
            }
            return genType;
        }




        private Type GenerateWith(Dictionary<string, MockMethod> mockMethods) {
            string TheName = "Mock" + klass.Name;

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
            typeBuilder.AddInterfaceImplementation(klass);
            FieldBuilder fieldBuilder =
            typeBuilder.DefineField(
                "obj",
                typeof(MockMethod[]),
                FieldAttributes.Private
            );
            CreateContructor(typeBuilder, fieldBuilder);

            // Generate methods
            ImplementMethods(klass, typeBuilder, fieldBuilder, mockMethods); //for this type
            Type[] typeInterfaces = klass.GetInterfaces();
            for (int i = 0; i<typeInterfaces.Length; ++i) {
                ImplementMethods(typeInterfaces[i], typeBuilder, fieldBuilder, null);
            }

            // Create type 
            Type retType = typeBuilder.CreateTypeInfo().AsType();

            // Save the assembly
            #if (DEBUG)
            asmBuilder.Save(DLL_NAME);
            #endif

            return retType;
        }


        private void CreateAssemblyBuilderAndTypeBuilder(string ASM_NAME, string MOD_NAME, string TYP_NAME, string DLL_NAME,
                                            out AssemblyBuilder asmBuilder, out TypeBuilder typBuilder) {
            // Define assembly
            asmBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName(ASM_NAME),
                    #if (DEBUG)
                    AssemblyBuilderAccess.RunAndSave
                    #else
                    AssemblyBuilderAccess.Run
                    #endif
        );

            // Define module in assembly
            ModuleBuilder modBuilder;
            #if (DEBUG)
            modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);
            #else
            modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME);    //asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);
            #endif

            // Define type in module
            typBuilder = modBuilder.DefineType(TYP_NAME);
        }

        private void CreateContructor(TypeBuilder typeBuilder, FieldBuilder fieldBuilder) {
            ConstructorBuilder ctorBuilder =
            typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new Type[1] { typeof(MockMethod[]) }
            );

            ILGenerator il = ctorBuilder.GetILGenerator();
            LocalBuilder tobj = il.DeclareLocal(klass);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(Object).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fieldBuilder);
            il.Emit(OpCodes.Ret);
        }

        private void ImplementMethods(Type type, TypeBuilder typeBuilder, FieldBuilder fieldBuilder, Dictionary<string, MockMethod> mockMethods) {
            MethodInfo[] methods = type.GetMethods(
            BindingFlags.Public       |
            BindingFlags.Instance     |
            BindingFlags.DeclaredOnly
        );

            for (int i = 0; i<methods.Length; ++i) {
                MethodInfo method = methods[i];
                EmitMethod(typeBuilder, fieldBuilder, type, method, mockMethods);
            }
        }

        private void EmitMethod(TypeBuilder typeBuilder, FieldBuilder fieldBuilder, Type type, MethodInfo method,
                                            Dictionary<string, MockMethod> mockMethods) {
            ParameterInfo[] pInfo = method.GetParameters();
            Type[] parms = new Type[pInfo.Length];
            for (int j = 0; j<parms.Length; ++j) {
                parms[j] = pInfo[j].ParameterType;
            }

            Type returnType = method.ReturnType;

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

            int methodIdx = GetIndexOfKey(mockMethods, method.Name);
            if (mockMethods==null || methodIdx<0) {
                ConstructorInfo ctorNIE = typeof(NotImplementedException).GetConstructor(Type.EmptyTypes);
                il.Emit(OpCodes.Newobj, ctorNIE);
                il.Emit(OpCodes.Throw);
            }
            else {
                EmitCallMockMethod(il, parms, returnType, fieldBuilder, methodIdx);
            }
        }

        ////////////////////////

        private void EmitCallMockMethod(ILGenerator il, Type[] parms, Type returnType, FieldBuilder fieldBuilder, int methodIdx) {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldBuilder);
            il.Emit(OpCodes.Ldc_I4, methodIdx);       //pos in array
            il.Emit(OpCodes.Ldelem_Ref);
            il.Emit(OpCodes.Ldc_I4, parms.Length);
            il.Emit(OpCodes.Newarr, typeof(Object));

            for (int i = 0; i<parms.Length; ++i) {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldarg, i+1);
                il.Emit(OpCodes.Box, parms[i]);
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.EmitCall(OpCodes.Callvirt, typeof(MockMethod).GetMethod("Call"), new Type[] { typeof(Object[]) });        //callvirt instance object[Mocky] Mocky.MockMethod::Call(object[])

            if (returnType.IsPrimitive)
                il.Emit(OpCodes.Unbox_Any, returnType);
            else
                il.Emit(OpCodes.Castclass, returnType);

            il.Emit(OpCodes.Ret);
        }

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
}
