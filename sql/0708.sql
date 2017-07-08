

create table OfficialAccount
(
Id int primary key identity(1,1),
website nvarchar(100),
hidwebsite varchar(100),
[type] nvarchar(100),
[column] nvarchar(100),
value nvarchar(1000),
createtime datetime default getdate(),
updatetime datetime default getdate()
)
go

select top 10 * from OfficialAccount order by createtime desc

