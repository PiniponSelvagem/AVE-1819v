﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mocky {
    public class MockMethod {

        private readonly Type klass;
        private readonly MethodInfo meth;
        private Dictionary<object[], object> results;
        private object[] args;
        private Delegate del;

        public Delegate Del { get { return del; } }

        public MethodInfo Method { get { return meth; } }

        public MockMethod(Type type, string name) {
            this.klass = type;
            this.meth = type.GetMethod(name);
            if (meth == null)
                throw new ArgumentException("There is no method " + name + " in type " + type);
            this.results = new Dictionary<object[], object>();
        }

        public MockMethod With(params object[] args) {
            if (this.args != null)
                throw new InvalidOperationException("You already called With() !!!!  Cannot call it twice without calling Return() first!");
            ParameterInfo[] argTypes = meth.GetParameters();
            if (argTypes.Length == args.Length) {
                if (AreAllArgumentsCompatible(argTypes, args)) {
                    this.args = args;
                    return this;
                }
            }
            throw new InvalidOperationException("Invalid arguments: " + String.Join(",", args));
        }
        public object Call(params object[] args) {
            foreach (KeyValuePair<object[], object> entry in results) {
                if (KeyEquals(entry.Key, args)) {
                    return entry.Value;
                }
            }

            if (Method.ReturnType.IsPrimitive)
                return Activator.CreateInstance(meth.ReturnType);   //return default value for the return type

            return null;
        }

        public void Then(Action act) {
            CheckAndSaveDelegate(act);;
        }

        public void Then<T1, T2>(Func<T1, T2> func) {
            CheckAndSaveDelegate(func);
        }

        public void Then<T1, T2, T3>(Func<T1, T2, T3> func) {
            CheckAndSaveDelegate(func);
        }

        public void Return(object res) {
            results.Add(args, res);
            this.args = null;
        }

        private bool AreAllArgumentsCompatible(ParameterInfo[] argTypes, object[] args) {
            int i = 0;
            foreach (var p in argTypes) {
                Type a = args[i++].GetType();
                if (!p.ParameterType.IsAssignableFrom(a))
                    return false;
            }
            return true;
        }

        private bool AreAllArgumentsCompatible_Delegate(ParameterInfo[] argTypes, Type funcRet) {
            foreach (var p in argTypes) {
                if (p.ParameterType != funcRet)
                    return false;
            }
            return true;
        }

        private void CheckAndSaveDelegate(Delegate delg) {
            if (del != null) {
                throw new InvalidOperationException("You already called Then() !!!!  Cannot call it twice for same method.");
            }

            ParameterInfo[] funcParam = delg.Method.GetParameters();
            Type funcRet = delg.Method.ReturnType;

            if (AreAllArgumentsCompatible_Delegate(funcParam, funcRet)) {
                del = delg;
            }
            else {
                throw new NotSupportedException("Arguments not compatible with delegate when calling Then().");
            }
        }



        private bool KeyEquals(object[] key, object[] args) {
            if (key.Length != args.Length) {
                return false;
            }

            for (int i=0; i<key.Length; ++i) {
                if (!key[i].Equals(args[i])) {
                    return false;
                }
            }
            return true;
        }
    }
}