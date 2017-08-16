--username: admin
--password: pass2app

INSERT INTO users (
            IsActive, UserName, NormalizedUserName, 
            Email, NormalizedEmail, PasswordHash ,PhoneNumber, FirstName, LastName)
    SELECT  true, 'admin', 'ADMIN', 
            'admin@admin.com', 'ADMIN@ADMIN.COM', 'AQAAAAEAACcQAAAAEJqD048Zo3HHPE9azvxJS4qBFlI05HmPhSI+tc0Qc4SE71rCIv0Xyv5CdR2MnIz8ow==', '0000000000', 'Admin', 'Admin';

INSERT INTO roles (Name, NormalizedName)
	SELECT 'Admin', 'ADMIN';

INSERT INTO roles (Name, NormalizedName)
	SELECT 'Curator', 'CURATOR';

INSERT INTO roles (Name, NormalizedName)
	SELECT 'Client', 'CLIENT';

INSERT INTO userroles(UserId, RoleId)
	SELECT 1, 1;

INSERT INTO userroles(UserId, RoleId)
	SELECT 1, 2;

INSERT INTO userroles(UserId, RoleId)
	SELECT 1, 3;