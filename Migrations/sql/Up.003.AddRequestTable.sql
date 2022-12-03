CREATE TABLE IF NOT EXISTS `request` (
    `requestId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `requestDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `status` ENUM('Send', 'precessing', 'rejected', 'accepted'),
    `userId` INT(10) UNSIGNED NOT NULL,
    `vehicleId` INT(10) UNSIGNED NOT NULL,
    `organizationId` INT(10) UNSIGNED NOT NULL,
    `comment` VARCHAR(255),
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