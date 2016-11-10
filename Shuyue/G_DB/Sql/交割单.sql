drop table delivery_order
create table delivery_order(
	Id int identity(1,1) primary key,
	SecurityCode varchar(100),--证券代码
	SecurityName nvarchar(100),--证券名称
	Volume int,--成交数量
	TransactionPrice decimal(18,5),--交易均价
	TransactionAmount decimal(18,5),--交易金额
	CounterFee decimal(18,5),--手续费
	StampDuty decimal(18,5),--印花税
	OtherExpenses decimal(18,5),--其他费用
	OccurrenceAmount decimal(18,5),--发生金额
	Remark nvarchar(500),--备注
	Operation nvarchar(500),--操作
	OperationType int,--操作类型
	ClosingDdate varchar(50),--成交日期
)