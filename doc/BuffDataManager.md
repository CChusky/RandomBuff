<h1>BuffDataManager</h1>

��������������ݡ�Ϊ����ģʽ��������ʹ��BuffDataManager.Instance.xxxx��

<h2>����</h2>

```csharp
//��ȡBuffData
//����������򷵻� null
public BuffData GetBuffData(BuffID id);
```

```csharp
//��ȡȫ�����õ�Buff ID
public List<BuffID> GetAllBuffIds();
```

<h2>����</h2>

```csharp
//��ȡBuffDataManager��ʵ��
public static BuffDataManager Instance { get; };
```