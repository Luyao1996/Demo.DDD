namespace DDD.Demo.Domain;

public class OptionBuilder<T>
{
    private T _t { get; set; }
    private Dictionary<string, Func<T, Task>> _dicOptionFuncs { get; set; } = new();
    private Dictionary<string, Func<T, Task>> _preDicOptionFuncs { get; set; } = new();

    private List<string> _waitForRemoveOptionKeys = new();

    public OptionBuilder(T t)
    {
        this._t = t;
    }

    public OptionBuilder<T> AddOption(Func<T, Task> func, string key = "")
    {
        if (func == null)
            return this;

        if (string.IsNullOrEmpty(key))
        {
            key = Guid.NewGuid().ToString();
        }

        _dicOptionFuncs.Add(key, func);
        return this;
    }

    public OptionBuilder<T> AddPreOption(Func<T, Task> func, string key = "")
    {
        if (func == null)
            return this;

        if (string.IsNullOrEmpty(key))
        {
            key = Guid.NewGuid().ToString();
        }

        _preDicOptionFuncs.Add(key, func);
        return this;
    }

    /// <summary>
    /// 移除选项 懒移除
    /// </summary>
    /// <param name="key"></param>
    public void RemoveOption(string key)
    {
        _waitForRemoveOptionKeys.Add(key);
    }

    public async Task<T> BuildAsync(Func<string, Task> beforeExec = null, Func<string, Task> afterExec = null)
    {
        if (_preDicOptionFuncs != null && _preDicOptionFuncs.Any())
        {
            foreach (var funcKV in _preDicOptionFuncs)
            {
                await funcKV.Value(_t);
            }
        }

        foreach (var key in _waitForRemoveOptionKeys)
        {
            if (_dicOptionFuncs.ContainsKey(key))
                _dicOptionFuncs.Remove(key);
        }

        foreach (var funcKV in _dicOptionFuncs)
        {
            if (beforeExec != null)
                await beforeExec(funcKV.Key);

            await funcKV.Value(_t);

            if (afterExec != null)
                await afterExec(funcKV.Key);
        }

        return _t;
    }
}