INSERT INTO organization (name, INN) values
    ('ООО \"Газпромнефть-Хантос\"', '123123123'),
    ('ООО \"НефтеСпецТранс\"', '123123123'),
    ('ООО \"Черногоравтотранс\"', '1231231234');

insert into user (firstName, lastName, login, password, organizationId, jobTitle)
values ('Иван', 'Иванов', 'user1', 'password1', 1, 'Сотрудник 1'),
       ('Василий', 'Васильев', 'user2', 'password2', 2, 'Сотрудник 2'),
       ('Пётр', 'Петров', 'user3', 'password3', 3, 'Сотрудник 3');

insert into request (status, userId, vehicleId, organizationId, comment)
values ('send', 2,1, 2, ''),
       ('precessing', 2,10, 2, 'Обрабатываем вашу заявку'),
       ('rejected', 2,100, 2, 'Заявка отклонена по причине.....'),
       ('accepted', 2,15, 2, ''),
       ('send', 3,150, 3, ''),
       ('precessing', 3,113, 3, 'Обрабатываем вашу заявку'),
       ('rejected', 3,121, 3, 'Заявка отклонена по причине.....'),
       ('accepted', 3,777, 3, '');