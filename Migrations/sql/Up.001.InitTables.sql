DROP TABLE  `user`;

CREATE TABLE IF NOT EXISTS `organization` (
    `organizationId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    `INN` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`organizationId`)
);

CREATE TABLE IF NOT EXISTS `user` (
    `userId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `firstName` VARCHAR(255) NOT NULL,
    `lastName` VARCHAR(255) NOT NULL,
    `login` VARCHAR(255) NOT NULL,
    `password` VARCHAR(255) NOT NULL,
    `organizationId` INT(10) UNSIGNED NOT NULL,
    `jobTitle` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`userId`),
    UNIQUE KEY (`login`),
    CONSTRAINT `fk_user_organization_userId`
        FOREIGN KEY (`organizationId`)
        REFERENCES `organization` (`organizationId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

