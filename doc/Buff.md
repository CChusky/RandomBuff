<h1>Buff&lt;TData&gt;</h1>
���ֻ��ڴ�����ʵ��������ʵ�ʵ������߼���
���ڵ��ֻؽ���ʱ���١�

<h2>����</h2>

```csharp
//��������򰴼�������ʱ����
//���Կɴ�����������Ч��Triggerable == true��
public abstract bool Trigger(RainWorldGame game);
```

```csharp
//����ĸ��·�������RainWorldGame.Updateͬ��
public abstract void Update(RainWorldGame game);
```

```csharp
// ��������ٷ�������������ʵ�����Ƴ���ʱ������
// ע�⣺��ǰ�ֻؽ���ʱ�����ȫ����Buff����
public abstract void Destroy();
```

<h2>����</h2>

```csharp
//�������ݶ�Ӧ��BuffID
public abstract BuffID ID { get; }
```

```csharp
//�����Ӧ������
public TData Data { get; }
```

```csharp
// ���Ϊtrue���������ڽ���ʱ�Զ��Ƴ�������
public bool NeedDeletion { get; set; }
```
