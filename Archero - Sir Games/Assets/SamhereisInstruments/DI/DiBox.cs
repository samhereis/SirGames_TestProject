using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace DI
{
    public static class DIBox
    {
        private static readonly Dictionary<Type, Dictionary<string, object>> _dictionarySingle = new Dictionary<Type, Dictionary<string, object>>();

        public static void Clear()
        {
            _dictionarySingle.Clear();
        }

        public static void Remove<T>(string id = "") where T : class
        {
            if (_dictionarySingle.ContainsKey(typeof(T))) _dictionarySingle[typeof(T)].Remove(id);
            if (_dictionarySingle.Count == 0) _dictionarySingle.Remove(typeof(T));

            Debug.Log($"Removed from DI - Type: {typeof(T)} - Id: '{id}'");
        }

        public static void Remove(Type type, string id = "")
        {
            if (_dictionarySingle.ContainsKey(type)) _dictionarySingle[type].Remove(id);
            if (_dictionarySingle.Count == 0) _dictionarySingle.Remove(type);

            Debug.Log($"Removed from DI - Type: {type} - Id: '{id}'");
        }

        public static T Get<T>(string id = "") where T : class
        {
            if (_dictionarySingle.ContainsKey(typeof(T)) == false) throw new Exception($"DI container does not contain this type  - Type: {typeof(T)}");
            if (_dictionarySingle[typeof(T)].ContainsKey(id) == false) throw new Exception($"The container does not contain under this ID - Type: {typeof(T)} \\ Id: '{id}'");

            return _dictionarySingle[typeof(T)][id] as T;
        }

        public static object Get(Type type, string id = "")
        {
            if (_dictionarySingle.ContainsKey(type) == false) throw new Exception($"DI container does not contain this type  - Type: {type}");
            if (_dictionarySingle[type].ContainsKey(id) == false) throw new Exception($"The container does not contain under this ID - Type: {type} \\ Id: '{id}'");

            return _dictionarySingle[type][id];
        }

        public static void Add<T>(T instance, string id = "")
        {
            if (instance == null) throw new Exception($"Instance is null - type {instance.GetType()}");

            AddToDictionary(instance, id, instance.GetType());
        }

        public static void Add(Object instance, string id = "")
        {
            if (instance == null) throw new Exception($"Instance is null");

            Type typeInstance = instance.GetType();

            AddToDictionary(instance, id, typeInstance);
        }

        public static void InjectDataTo(Object obj)
        {
            if (obj == null) return;

            var listFeild = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => CustomAttributeExtensions.GetCustomAttribute<DIAttribute>((MemberInfo)x) != null);

            foreach (var field in listFeild)
            {
                var att = field.GetCustomAttribute<DIAttribute>();
                try
                {
                    var gottenObj = Get(field.FieldType, att.Id);
                    field.SetValue(obj, gottenObj);
                }
                catch (Exception ex) { Debug.LogError(ex); }
            }

            var listProperty = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.GetCustomAttribute<DIAttribute>() != null);

            foreach (var prop in listProperty)
            {
                var att = prop.GetCustomAttribute<DIAttribute>();
                prop.SetValue(obj, Get(prop.PropertyType, att.Id));
            }

            var listMethodInfo = obj.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (listMethodInfo.Length > 0)
            {
                var methodInit = listMethodInfo.Where(x => x.GetCustomAttribute<DIAttribute>() != null);
                if (methodInit.Count() > 0) methodInit.First().Invoke(obj, new object[0]);
            }
        }

        private static void AddToDictionary(object instance, string id, Type typeInstance)
        {
            if (_dictionarySingle.ContainsKey(typeInstance) && _dictionarySingle[typeInstance].ContainsValue(id) == false)
            {
                _dictionarySingle[typeInstance].Add(id, instance);
            }
            else
            {
                _dictionarySingle.Add(typeInstance, new Dictionary<string, object>());
                _dictionarySingle[typeInstance].Add(id, instance);
            }

            Debug.Log($"Added to DI - Type: {typeInstance}- Id: '{id}'");
        }
    }
}