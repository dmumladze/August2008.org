BEGIN TRY
	BEGIN TRANSACTION;
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	SET IDENTITY_INSERT[dbo].[Language] ON
	INSERT INTO [dbo].[Language] ([LanguageId], [DisplayName], [EnglishName], [Culture]) VALUES (1, 'ქართული (Georgian)', 'Georgian', 'ka-GE')
	INSERT INTO [dbo].[Language] ([LanguageId], [DisplayName], [EnglishName], [Culture]) VALUES (2, 'English (USA)', 'English (US)', 'en-US')
	SET IDENTITY_INSERT[dbo].[Language] OFF
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	SET IDENTITY_INSERT[dbo].[MilitaryGroup] ON
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (1)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (2)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (3)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (4)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (5)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (6)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (7)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (8)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (9)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (10)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (11)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (12)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (13)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (14)
	INSERT INTO [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (15)
	SET IDENTITY_INSERT[dbo].[MilitaryGroup] OFF
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (1, N'სამხედრო სახმელეთო ძალები', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (2, N'სამხედრო საზღვაო ძალები', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (3, N'სამხედრო საჰაერო ძალები', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (4, N'I ქვეითი ბრიგადა', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (5, N'II ქვეითი ბრიგადა', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (6, N'III ქვეითი ბრიგადა', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (7, N'IV ქვეითი ბრიგადა', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (8, N'V ქვეითი ბრიგადა', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (9, N'საინჟინრო ბრიგადა', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (10, N'ცალკეული ჯავშანსატანკო ბატალიონი', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (11, N'ცალკეული მსუბუქი ქვეითი ბატალიონი', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (12, N'სპეციალური ოპერაციების დაჯგუფება', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (13, N'ჯარების ლოგისტიკური უზრუნველყოფის დეპარტამენტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (14, N'ეროვნული გვარდია', NULL, 1)
	INSERT INTO [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (15, N'ს/დ დეპარტამენტი, I ოპერატიული სამმართველოს თანამშრომელი', NULL, 1)
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	SET IDENTITY_INSERT[dbo].[MilitaryRank] ON
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (1)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (2)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (3)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (4)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (5)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (6)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (7)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (8)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (9)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (10)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (11)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (12)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (13)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (14)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (15)
	INSERT INTO [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (16)
	SET IDENTITY_INSERT[dbo].[MilitaryRank] OFF
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (1, N'კაპიტან-ლეიტენანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (2, N'კაპრალი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (3, N'კაპიტანი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (4, N'სერჟანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (5, N'ავიაციის უმცროსი სერჟანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (6, N'ვადიანი ს/მ', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (7, N'ლეიტენანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (8, N'უმცროსი სერჟანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (9, N'უფროსი ლეიტენანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (10, N'მაიორი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (11, N'უფროსი სერჟანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (12, N'სამედიცინო სამსახურის სერჟანტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (13, N'პოლკოვნიკი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (14, N'ეს/პირი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (15, N'რეზერვისტი', NULL, 1)
	INSERT INTO [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (16, N'ოფიცერი', NULL, 1)
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	SET IDENTITY_INSERT[dbo].[MilitaryAward] ON
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (1)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (2)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (3)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (4)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (5)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (6)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (7)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (8)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (9)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (10)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (11)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (12)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (13)
	INSERT INTO [dbo].[MilitaryAward] ([MilitaryAwardId]) VALUES (14)
	SET IDENTITY_INSERT[dbo].[MilitaryAward] OFF
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (1, N'ეროვნული გმირის ორდენი', N'ეროვნული გმირის ორდენი, 
	საქართველოს უმაღლესი სახელმწიფო ჯილდო, რომელიც საქართველოს ეროვნული გმირის წოდებასთან ერთად ენიჭება პირს საქართველოსთვის განსაკუთრებული, გამორჩეული გმირული 
	ქმედებისათვის. დაწესდა 2004 წ. 24 ივნისს. ფულადი პრემია - შრომის ანაზღაურების 500 მინიმალური ოდენობით.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (2, N'წმინდა გიორგის სახელობის გამარჯვების ორდენი', N'წმინდა გიორგის 
	სახელობის გამარჯვების ორდენი, საქართველოს სახელმწიფო ჯილდო, ორდენი, რომელიც ენიჭება პირს საქართველოსთვის მოპოვებულ გამარჯვებებში შეტანილი განსაკუთრებული წვლილისათვის.
	შემოღებულია 2004 წლის 24 ივნისის საქართველოს პრეზიდენტის ბრძანების შესაბამისად. ფულადი პრემია - შრომის ანაზღაურების 300 მინიმალური ოდენობით.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (3, N'ბრწყინვალების საპრეზიდენტო ორდენი', N'ბრწყინვალების საპრეზიდენტო 
	ორდენი ენიჭება კულტურის, განათლების, მეცნიერების, ხელოვნების, სპორტისა და სხვა დარგის გამოჩენილ მოღვაწეებს საზოგადოებრივი ცხოვრების შესაბამის სფეროში გამორჩეული მიღწევებისათვის 
	და საქართველოს წინაშე აღმატებული ღვაწლისათვის.ფულადი პრემია - ფულადი პრემია 1.000 ლარი.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (4, N'დავით აღმაშენებლის ორდენი', N'დავით აღმაშენებელის ორდენი, საქართველოს 
	სახელმწიფო ჯილდო, ორდენი, რომლითაც ჯილდოვდებიან სამოქალაქო, სამხედრო და სასულიერო პირები, სახელმწიფო მოღვაწეები საქართველოს ერის წინაშე ღვაწლის, საქართველოს სახელმწიფოებრივი 
	დამოუკიდებლობისათვის ბრძოლისა და აღორძინების, საზოგადოების კონსოლიდაციის, დემოკრატიის დამკვიდრების საქმეში შეტანილი პირადი წვლილისათვის. ორდენი დამტკიცებულია 1992 წლის 24 
	დეკემბრის პარლამენტის დადგენილებით. ფულადი პრემია 200 ლარი.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (5, N'თამარ მეფის ორდენი', N'თამარ მეფის ორდენით ჯილდოვდებიან ქალები ქვეყნისა 
	და ერის წინაშე განსაკუთრებული ღვაწლისათვის, გამორჩეული სახელმწიფოებრივი ან/და საზოგადოებრივი მოღვაწეობისათვის. ფულადი პრემია - ფულადი პრემია 4.000 ლარი.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (6, N'წმინდა ნიკოლოზის ორდენი', N'წმინდა ნიკოლოზის ორდენით ჯილდოვდებიან  
	პირები ქველმოქმედებისათვის, ქვეყნისა და ხალხის უანგარო სამსახურისათვის.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (7, N'ოქროს საწმისის ორდენი', N'ოქროს საწმისის ორდენით ჯილდოვდებიან უცხოელი 
	მოქალაქეები და მოქალაქეობის არ მქონე პირები, რომელთაც განსაკუთრებული წვლილი შეიტანეს საქართველოს აღმშენებლობაში, მისი ეროვნული უშიშროების ინტერესების, სუვერენიტეტისა და  
	ტერიტორიული მთლიანობის დაცვაში, დემოკრატიული და თავისუფალი საზოგადოების ჩამოყალიბებაში, უცხოეთის სახელმწიფოებთან და საერთაშორისო ორგანიზაციებთან ორმხრივად სასარგებლო 
	ურთიერთობის დამყარებაში, საზღვარგარეთ საქართველოს მოქალაქეების უფლებების დაცვაში, ქართული კულტურის პოპულარიზაციაში, ქართული მეცნიერებისა და ხელოვნების განვითარებაში.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (8, N'ვახტანგ გორგასლის ორდენი - I ხარისხი', N'ვახტანგ გორგასლის ორდენით 
	ჯილდოვდებიან საქართველოს სამხედრო მოსამსახურენი, რომლებმაც სამშობლოს დაცვისა და ერთიანობისათვის ბრძოლაში გამოიჩინეს გმირული თავდადება და მამაცობა, უზრუნველყვეს სამხედრო 
	ნაწილებისა და ქვედანაყოფების წარმატება. I ხარისხის ორდენით ჯილდოვდებიან ერისა და სამშობლოს წინაშე განსაკუთრებული დამსახურების, თავდადებისა და თავგანწირვისათვის.დაჯილდოებული 
	პირი სარგებლობს საზოგადოებრივი ტრანსპორტით სახელმწიფოს ხარჯზე მგზავრობის უფლებით და სავალდებულო სამედიცინო დაზღვევით.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (9, N'ვახტანგ გორგასლის ორდენი - II ხარისხი', N'ვახტანგ გორგასლის ორდენით  
	ჯილდოვდებიან საქართველოს სამხედრო მოსამსახურენი, რომლებმაც სამშობლოს დაცვისა და ერთიანობისათვის ბრძოლაში გამოიჩინეს გმირული თავდადება და მამაცობა, უზრუნველყვეს სამხედრო 
	ნაწილებისა და ქვედანაყოფების წარმატება. II ხარისხის ორდენით ჯილდოვდებიან სამხდრო ხელმძღვანელობის მიერ მიცემული საბრძოლო დავალების წარმატებით შესრულებისათვის.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (10, N'ვახტანგ გორგასლის ორდენი - III ხარისხი', N'ვახტანგ გორგასლის ორდენით 
	ჯილდოვდებიან საქართველოს სამხედრო მოსამსახურენი, რომლებმაც სამშობლოს დაცვისა და ერთიანობისათვის ბრძოლაში გამოიჩინეს გმირული თავდადება და მამაცობა, უზრუნველყვეს სამხედრო 
	ნაწილებისა და ქვედანაყოფების წარმატება. III ხარისხის ორდენით ჯილდოვდებიან სამსახურებრივი მოვალეობის შესრულების დროს გამოჩენილი მაღალი საბრძოლო მომზადებისთვის.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (11, N'ღირსების ორდენი', N'ღირსების ორდენით ჯილდოვდებიან მოქალაქენი 
	დამოუკიდებელი ქართული სახელმწიფოს მშენებლობაში - მმართველობის, თავდაცვის, კანონიერების და მართლწესრიგის განმტკიცების, სახალხო მეურნეობის, ჯანმრთელობის დაცვის, მეცნიერების, 
	განათლების, კულტურის, ლიტერატურის, ხელოვნების დარგებში, სპორტული მიღწევებისათვის შეტანილი განსაკუთრებული პიროვნული წვლილისათვის, გმირობისა და თავდადებისათვის. ფულადი პრემია 
	- შრომის ანაზღაურების 30 მინიმალური ოდენობით.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (12, N'მხედრული მამაცობის მედალი', N'მხედრული მამაცობის მედლით 
	ჯილდოვდებიან საქართველოს სამხედრო მოსამსახურეები, პოლიციის მუშაკები მამულის დაცვისას პიროვნული მამაცობისა და მხედრული ვალის შესრულებისათვის, ვაჟკაცობისა და გაბედული 
	მოქმედებისათვის. მედლით "მხედრული მამაცობისათვის" სამჯერ დაჯილდოებულ პირს გადაეცემა პრეზიდენტის სამახსოვრო საჩუქარი. ფულადი პრემია - შრომის ანაზღაურების 30 მინიმალური 
	ოდენობით.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (13, N'საბრძოლო დამსახურების მედალი', N'საბრძოლო დამსახურების მედლით 
	ჯილდოვდებიან საქართველოს სამხედრო მოსამსახურენი და სამოქალაქო პირნი სამშობლოს დაცვისა და ერთიანობის უზრუნველყოფაში განსაკუთრებული პიროვნული დამსახურებისათვის.', 1)
	INSERT INTO [dbo].[MilitaryAwardTranslation] ([MilitaryAwardId], [AwardName], [Description], [LanguageId]) VALUES (14, N'ღირსების მედალი', N'ღირსების მედლით ჯილდოვდებიან საქართველოს 
	მოქალაქენი საქართველოს აღორძინების საქმეში შეტანილი განსაკუთრებული წვლილისა და ღირსეული სამსახურისათვის.', 1)
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	SET IDENTITY_INSERT[dbo].[Role] ON
	INSERT INTO [dbo].[Role] (RoleId, Name, Description) VALUES (1, 'Reader', 'Regular user with minimal provilages')
	INSERT INTO [dbo].[Role] (RoleId, Name, Description) VALUES (2, 'Writer', 'User with ability to perform set of operation such as creating an aricle etc..')
	INSERT INTO [dbo].[Role] (RoleId, Name, Description) VALUES (3, 'Admin', 'Administrator')
	SET IDENTITY_INSERT[dbo].[Role] OFF
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	
	SET IDENTITY_INSERT[dbo].[DonationProvider] ON
	INSERT INTO [dbo].[DonationProvider] (DonationProviderId, ProviderName) VALUES (1, 'PayPal')	
	SET IDENTITY_INSERT[dbo].[DonationProvider] OFF
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 
	BEGIN
		ROLLBACK TRANSACTION;
		PRINT ERROR_MESSAGE();
	END;
END CATCH