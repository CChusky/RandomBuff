<h1>BuffRegister</h1>

����ע�����档�Ǿ�̬�ࣨ��ĸо�û�Ľ��ܵĶ����ˣ�

<h2>����</h2>

```csharp
//ע���µ�����
//HookType ��̳��� IBuffHook
public static void RegisterBuff<BuffType, DataType, HookType>(BuffID id);
```

```csharp
//ע���µ�����
public static void RegisterBuff<BuffType, DataType>(BuffID id);
```

<h2>�ӿ�</h2>

```csharp
public interface IBuffHook
{
    //�൱��OnModsInit()
    //�뽫������Ҫ��Hook������Ӧ��
    public void HookOn();
}
```
