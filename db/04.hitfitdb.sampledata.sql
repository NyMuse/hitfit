--username: admin
--password: pass2app

INSERT INTO public."Users"(
            "Id", "IsAdministrator", "IsActive", "UserName", "NormalizedUserName", 
            "Email", "NormalizedEmail", "PasswordHash" ,"PhoneNumber", "FirstName", "LastName")
    VALUES (1, true, true, 'admin', 'ADMIN', 
            'admin@admin.com', 'ADMIN@ADMIN.COM', 'AQAAAAEAACcQAAAAEJqD048Zo3HHPE9azvxJS4qBFlI05HmPhSI+tc0Qc4SE71rCIv0Xyv5CdR2MnIz8ow==', '0000000000', 'Admin', 'Admin');
