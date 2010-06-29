create database test;
use test;
//create table users (userid int not null default 0,username varchar(32),primary key(userid));
//insert into users (userid,username) values(1,'testa');
//insert into users (userid,username) values(2,'testb');


create table logmsginfo(msgid bigint not null,msgmark bigint not null, msgtype varchar(32) not null,logmsg varchar(128) not null,msgowner varchar(32) not null,msgtime datetime not null, primary key(msgid));
create table logmarkinfo(markid bigint not null,markdesc varchar(64) not null, markowner varchar(32) not null, markstate tinyint not null default 0, marktime datetime not null, primary key(markid));