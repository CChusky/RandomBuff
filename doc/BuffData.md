
<h1>BuffData</h1>

������������ͣ�����浵�����ݣ�Ҳ���Ի�ȡ�������ԡ�
�������ŵ��ֿ�ʼ�������ظ����������ڸ����浵��������Ϸʱ�򴴽���

<h2>����</h2>


```csharp
//�ֻؽ���ʱ����
public virtual void CycleEnd(); 
```


```csharp
//�ظ�ѡȡʱ�ᱻ����
public virtual void Stack(); 
```

```csharp
//���浵���ݶ�ȡ�����
//�����������ݳ�ʼ��
public abstract void DataLoaded(bool newData); 
```

```csharp
//��ȡ�������ԣ��������������ݵķ�ʽ��ȡ��
public T GetConfig<T>(string name);
```

<h2>����</h2>

```csharp
//�������ݶ�Ӧ��BuffID
public abstract BuffID ID { get; }
```

�浵����
```csharp
[JsonProperty]
public AnyType saveInWorld; //����һ����Ҫ���浽è����Ϸ�浵������
```

��������
```csharp
[CustomStaticConfig]
public AnyType saveInSlot { get; } //����һ����������
//������Բ�����è�浵�仯���仯
//����һ�������Ľ���(����mod����)����������

///ע�� �� ����ʹ�� {get;} ����ʽ������
```

