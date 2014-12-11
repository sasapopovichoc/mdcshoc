create table users(userId int identity, username nvarchar(100), password nvarchar(100), email nvarchar(100))
create clustered index index_users_userid on users(userid)

insert into users values ('sasapopo', 'password1', 'sasapopo@microsoft.com')
insert into users values ('administrator', 'adminpassword1', null)
select * from users

create table messages(messageid int identity, fromUserId int, toUserId int, messageText nvarchar(max), messageTime datetime)
create clustered index index_messages_messageid on messages(messageId)

insert into messages values (null, null, 'Hello world', getutcdate())
insert into messages values (null, null, 'second message', getutcdate())
select * from messages

create table eventLog(eventId int identity, eventTime datetime, eventDescription nvarchar(max))
create clustered index index_eventlog_eventid on eventLog(eventid)

insert into eventLog values (getutcdate(), 'test event')
select * from eventLog