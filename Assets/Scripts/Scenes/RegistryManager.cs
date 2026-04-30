using System.Collections.Generic;
using UnityEngine;

public class RegistryManager : MonoBehaviour
{
    public static RegistryManager Instance { get; private set; }

    [System.Serializable]
    public class RegistryItem
    {
        public string key;
        public GameObject value;
    }

    public List<RegistryItem> initialItems = new List<RegistryItem>();
    
    // 使用 GameObject 作為基底存儲
    private Dictionary<string, GameObject> _registry = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        // 初始化
        foreach (var item in initialItems)
        {
            Register(item.key, item.value);
        }
    }

    // 註冊方法（支援動態加入）
    public void Register(string key, GameObject obj)
    {
        if (!_registry.ContainsKey(key))
            _registry.Add(key, obj);
        else
            Debug.LogWarning($"[Registry] Key '{key}' 已經存在！");
    }

    // 核心功能：泛型獲取
    // 使用方式：var btn = RegistryManager.Instance.Get<Button>("SubmitBtn");
    public T Get<T>(string key) where T : Component
    {
        if (_registry.TryGetValue(key, out GameObject obj))
        {
            return obj.GetComponent<T>();
        }
        Debug.LogError($"[Registry] 找不到 Key: {key}");
        return null;
    }

    // 直接獲取 GameObject
    public GameObject GetGameObject(string key)
    {
        if (_registry.TryGetValue(key, out GameObject obj)) return obj;
        return null;
    }
}