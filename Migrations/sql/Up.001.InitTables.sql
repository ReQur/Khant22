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

CREATE TABLE IF NOT EXISTS `vehicle` (
    `vehicleId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `organizationId` INT(10) UNSIGNED NOT NULL,
    `vehicleNumber` VARCHAR(255) NOT NULL,
    `model` VARCHAR(255) NOT NULL,
    `code` VARCHAR(255) NOT NULL,
    `vehicleType` VARCHAR(255) NOT NULL,
    `serviceType` VARCHAR(255) NOT NULL,
    `vehicleTypeExt` VARCHAR(255) NOT NULL,
    `vehicleChars` VARCHAR(255) NOT NULL,
    `country` ENUM('Russia', 'Unknown', 'Import'),
    `fuelType` ENUM('Diesel', 'Unknown', 'Petrol'),
    `subOrganization` boolean,
    `ownershipType` ENUM('Contract', 'Rent', 'leasing', 'Unknown'),
    CONSTRAINT `fk_vehicle_organization_organizationId`
        FOREIGN KEY (`organizationId`)
        REFERENCES `organization` (`organizationId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    PRIMARY KEY (`vehicleId`)
);


CREATE TABLE IF NOT EXISTS `request` (
    `requestId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `requestDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `status` ENUM('Send', 'precessing', 'rejected', 'accepted'),
    `userId` INT(10) UNSIGNED NOT NULL,
    `vehicleId` INT(10) UNSIGNED NOT NULL,
    `organizationId` INT(10) UNSIGNED NOT NULL,
    CONSTRAINT `fk_request_vehicle_vehicleId`
        FOREIGN KEY (`vehicleId`)
        REFERENCES `vehicle` (`vehicleId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_request_user_userId`
        FOREIGN KEY (`userId`)
        REFERENCES `user` (`userId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_request_organization_organizationId`
        FOREIGN KEY (`organizationId`)
        REFERENCES `organization` (`organizationId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    PRIMARY KEY (`requestId`)
);
