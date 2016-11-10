drop table delivery_order
create table delivery_order(
	Id int identity(1,1) primary key,
	SecurityCode varchar(100),--֤ȯ����
	SecurityName nvarchar(100),--֤ȯ����
	Volume int,--�ɽ�����
	TransactionPrice decimal(18,5),--���׾���
	TransactionAmount decimal(18,5),--���׽��
	CounterFee decimal(18,5),--������
	StampDuty decimal(18,5),--ӡ��˰
	OtherExpenses decimal(18,5),--��������
	OccurrenceAmount decimal(18,5),--�������
	Remark nvarchar(500),--��ע
	Operation nvarchar(500),--����
	OperationType int,--��������
	ClosingDdate varchar(50),--�ɽ�����
)