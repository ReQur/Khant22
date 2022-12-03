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

CREATE TABLE IF NOT EXISTS `request` (
    `requestId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `requestDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `status` ENUM('Send', 'precessing', 'rejected', 'accepted'),
    `startDate`TIMESTAMP,
    `endDate`TIMESTAMP,
    `userId` INT(10) UNSIGNED NOT NULL,
    `organizationId` INT(10) UNSIGNED NOT NULL,
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

CREATE TABLE IF NOT EXISTS `vehicleType` (
    `vehicleTypeId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`vehicleTypeId`)
);
CREATE TABLE IF NOT EXISTS `vehicleTypeExt` (
    `vehicleTypeExtId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`vehicleTypeExtId`)
);

CREATE TABLE IF NOT EXISTS `serviceType` (
    `serviceTypeId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`serviceTypeId`)
);

CREATE TABLE IF NOT EXISTS `vehicle` (
    `vehicleId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `organizationId` INT(10) UNSIGNED NOT NULL,
    `vehicleNumber` VARCHAR(255) NOT NULL,
    `model` VARCHAR(255) NOT NULL,
    `code` VARCHAR(255) NOT NULL,
    `vehicleTypeId` INT(10) UNSIGNED NOT NULL,
    `serviceTypeId` INT(10) UNSIGNED NOT NULL,
    `vehicleTypeExtId` INT(10) UNSIGNED NOT NULL,
    `vehicleChars` VARCHAR(255) NOT NULL,
    `country` ENUM('Russia', 'Unknown', 'Import'),
    `fuelType` ENUM('Diesel', 'Unknown', 'Petrol'),
    `subOrganization` ENUM('Yes', 'No'),
    `ownershipType` ENUM('Contract', 'Rent', 'leasing', 'Unknown'),
    CONSTRAINT `fk_vehicle_organization_organizationId`
        FOREIGN KEY (`organizationId`)
        REFERENCES `organization` (`organizationId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_vehicle_serviceType_serviceTypeId`
        FOREIGN KEY (`serviceTypeId`)
        REFERENCES `serviceType` (`serviceTypeId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_vehicle_vehicleTypeExt_vehicleTypeExtId`
        FOREIGN KEY (`vehicleTypeExtId`)
        REFERENCES `vehicleTypeExt` (`vehicleTypeExtId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_vehicle_vehicleType_vehicleTypeId`
        FOREIGN KEY (`vehicleTypeId`)
        REFERENCES `vehicleType` (`vehicleTypeId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    PRIMARY KEY (`vehicleId`)
);

CREATE TABLE IF NOT EXISTS `vehicleRequest` (
    `vehicleRequestId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `vehicleId` INT(10) UNSIGNED NOT NULL,
    `requestId` INT(10) UNSIGNED NOT NULL,
    PRIMARY KEY (`vehicleRequestId`),
    CONSTRAINT `fk_serviceType_request_requestId`
        FOREIGN KEY (`requestId`)
        REFERENCES `request` (`requestId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_serviceType_vehicle_vehicleId`
        FOREIGN KEY (`vehicleId`)
        REFERENCES `vehicle` (`vehicleId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);