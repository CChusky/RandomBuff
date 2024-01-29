﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RandomBuff.Core.Buff;
using UnityEngine;

namespace RandomBuff.Core.SaveData
{

    /// <summary>
    /// Buff静态数据的保存 仅跟随saveSlot变化而更换
    /// 公开部分
    /// </summary>
    public partial class BuffConfigManager
    {
        public static BuffConfigManager Instance { get; private set; }

        internal static void LoadConfig(string config, string formatVersion)
        {
            Instance = new BuffConfigManager(config, formatVersion);
        }

        /// <summary>
        /// 序列化保存配置信息
        /// </summary>
        /// <returns></returns>
        internal string ToStringData()
        {
            StringBuilder builder = new();
            foreach (var buffConfig in allConfigs)
            {
                builder.Append(buffConfig.Key);
                builder.Append(BuffIdSplit);
                foreach (var paraConfig in buffConfig.Value)
                {

                    builder.Append(paraConfig.Key);
                    builder.Append(ParameterIdSplit);
                    builder.Append(paraConfig.Value);
                    builder.Append(ParameterSplit);
                }

                builder.Append(BuffSplit);
            }
            return builder.ToString();
        }


        /// <summary>
        /// 尝试获取对应config
        /// 如果不存在返回false
        /// 存在则保存到loadedConfigs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffName"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryGet(BuffID buffName, string key, Type type, out object data)
        {
            if (allLoadedConfigs.ContainsKey(buffName.value) && allLoadedConfigs[buffName.value].ContainsKey(key))
            {
                data = allLoadedConfigs[buffName.value][key];
                return true;
            }

            if (allConfigs.ContainsKey(buffName.value))
            {
                try
                {
                    data = JsonConvert.DeserializeObject(allConfigs[buffName.value][key], type);
                    AddLoadedConfig(buffName.value, key, data);
                    return true;
                }
                catch (Exception e)
                {
                    BuffPlugin.LogException(e);
                    BuffPlugin.LogError($"Corrupted Buff Config Data At : {buffName}:{key}:{allConfigs[buffName.value][key]}");
                    data = default;
                    return false;
                }
            }

            if (staticDatas.ContainsKey(buffName) &&
                staticDatas[buffName].customParameters.ContainsKey(key))
            {
                try
                {
                    data = JsonConvert.DeserializeObject(staticDatas[buffName].customParameters[key], type);
                    AddLoadedConfig(buffName.value, key, data);
                    return true;
                }
                catch (Exception e)
                {
                    BuffPlugin.LogException(e);
                    BuffPlugin.LogError($"Corrupted Buff Config Data At : {buffName}:{key}:{staticDatas[buffName].customParameters[key]}");
                    data = default;
                    return false;
                }

            }

            BuffPlugin.LogError($"Can't Find Config Data For : {buffName}:{key}");
            data = default;
            return false;
        }

        public bool TryGet<T>(BuffID buffName, string key, out T data)
        {
            if (TryGet(buffName, key, typeof(T), out var obj))
            {
                data = (T)obj;
                return true;
            }
            data = default;
            return false;
        }

        /// <summary>
        /// 设置Config和loadedConfig
        /// 为了速度优化 不会进行保存
        ///
        /// 务必手动调用BuffFile.SaveFile()!!!!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffName"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        internal void Set<T>(BuffID buffName, string key,T obj)
        {
            if(!allLoadedConfigs.ContainsKey(buffName.value))
                allLoadedConfigs.Add(buffName.value,new ());

            if (!allLoadedConfigs[buffName.value].ContainsKey(key))
                allLoadedConfigs[buffName.value].Add(key, obj);
            else
                allLoadedConfigs[buffName.value][key] = obj;

            string strData = "";
            try
            {
               strData = JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                BuffPlugin.LogException(e);
                BuffPlugin.LogError($"{buffName}:{key} data serialize failed !");
                return;
            }

            if(!allConfigs.ContainsKey(buffName.value))
                allConfigs.Add(buffName.value, new ());

            if (!allConfigs[buffName.value].ContainsKey(key))
                allConfigs[buffName.value].Add(key, strData);
            else
                allConfigs[buffName.value][key] = strData;
        }

        private readonly Dictionary<string, Dictionary<string, string>> allConfigs = new();
        private readonly Dictionary<string, Dictionary<string, object>> allLoadedConfigs = new();

    }

    /// <summary>
    /// 内部实例部分
    /// </summary>
    public partial class BuffConfigManager
    {

        private BuffConfigManager(string config, string formatVersion)
        {
            foreach (var buffSingle in Regex.Split(config, BuffSplit)
                         .Where(i => !string.IsNullOrEmpty(i)))
            {
                var buffSplit = Regex.Split(buffSingle, BuffIdSplit);
                if (buffSplit.Length != 2)
                {
                    BuffPlugin.LogError($"Corrupted Config Data At: {buffSingle}");
                    continue;
                }

                if (allConfigs.ContainsKey(buffSplit[0]))
                    BuffPlugin.LogWarning($" Redefine buff name in config:{buffSplit[0]}");
                else
                    allConfigs.Add(buffSplit[0], new());

                var buffConfigs = allConfigs[buffSplit[0]];

                foreach (var paraSingle in Regex.Split(buffSplit[1], ParameterSplit)
                             .Where(i => !string.IsNullOrEmpty(i)))
                {
                    var paraSplit = Regex.Split(paraSingle, ParameterIdSplit);
                    if (paraSplit.Length != 2)
                    {
                        BuffPlugin.LogError($"Corrupted Config Data At: {buffSplit[0]}:{paraSingle}");
                        continue;
                    }

                    if (buffConfigs.ContainsKey(paraSplit[0]))
                    {
                        BuffPlugin.LogWarning($"Redefine BuffConfig Name: {buffSplit[0]}:{paraSplit[0]}, Ignore {paraSplit[1]}");
                        continue;
                    }

                    buffConfigs.Add(paraSplit[0], paraSplit[1]);
                }

            }
        }

        
        /// <summary>
        /// 添加已读取config
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        private void AddLoadedConfig(string id, string key, object data)
        {
            if (!allLoadedConfigs.ContainsKey(id))
                allLoadedConfigs.Add(id,new ());

            if (!allLoadedConfigs[id].ContainsKey(key))
                allLoadedConfigs[id].Add(key, data);
            else
                allLoadedConfigs[id][key] = data;
        }

        private const string BuffSplit = "<BuB>";
        private const string BuffIdSplit = "<BuBI>";

        private const string ParameterSplit = "<BuC>";
        private const string ParameterIdSplit = "<BuCI>";
    }

    /// <summary>
    /// 静态部分
    /// </summary>
    public partial class BuffConfigManager
    {
        private static Dictionary<BuffID, BuffStaticData> staticDatas = new();

        internal static bool ContainsId(BuffID id)
            => staticDatas.ContainsKey(id);
        internal static bool ContainsProperty(BuffID id, string key)
            => staticDatas.ContainsKey(id) && staticDatas[id].customParameters.ContainsKey(key);

        internal static BuffStaticData GetStaticData(BuffID id)
            => staticDatas[id];

        internal static Dictionary<BuffType,List<BuffID>> buffTypeTable = new ();

        static BuffConfigManager()
        {
            buffTypeTable.Add(BuffType.Duality, new List<BuffID>());
            buffTypeTable.Add(BuffType.Negative, new List<BuffID>());
            buffTypeTable.Add(BuffType.Positive, new List<BuffID>());
        }

        /// <summary>
        /// 读取static data
        /// post init时调用
        /// 文件格式 mod根目录/buffassets/卡牌名/卡牌资源
        /// </summary>
        internal static void InitBuffStaticData()
        {
            BuffPlugin.Log("Loading All Buff Static Data!");
            foreach (var mod in ModManager.ActiveMods)
            {
                string path = mod.path + Path.DirectorySeparatorChar + "buffassets";
                if (!Directory.Exists(path))
                    continue;

                LoadInDirectory(new DirectoryInfo(path), new DirectoryInfo(mod.path).FullName);
            }
        }


        private static void LoadInDirectory(DirectoryInfo info, string rootPath)
        {
            foreach (var dir in info.GetDirectories())
            {
                LoadInDirectory(info, dir.FullName);
            }

            foreach (var file in info.GetFiles("*.json"))
            {
                if (BuffStaticData.TryLoadStaticData(file,
                        info.FullName.Replace(rootPath, ""), out var data))
                {
                    staticDatas.Add(data.BuffID, data);
                    buffTypeTable[data.BuffType].Add(data.BuffID);
                }
            }
        }
    }
}
