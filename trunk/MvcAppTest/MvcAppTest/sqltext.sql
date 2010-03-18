create database test;
use test;
create table users (userid int not null default 0,username varchar(64),primary key(userid));
insert into users (userid,username) values(1,'testa');
insert into users (userid,username) values(2,'testb');


//create table loginfo (logid int identity(1,1) not null, typeid varchar(64) foreign key  references logtype(typeid) on delete  set null,logmsg varchar(128),logtime datetime not null default getdate(), primary key(logid));
create table loginfo (logid varchar(64) not null, typeid varchar(32) not null,logmsg varchar(128) not null,logowner varchar(32) not null,logtime datetime not null default getdate(), primary key(logid));
insert into loginfo(typeid, logmsg) values('852d8622-1d00-4f98-833f-b7eab4520d1b','debug');
