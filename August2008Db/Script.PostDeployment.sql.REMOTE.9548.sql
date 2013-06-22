﻿USE [August2008Db]
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Language] ON
GO
INSERT INTO [dbo].[Language] ([LanguageId], [DisplayName], [EnglishName], [Culture]) VALUES (1, 'ქართული', 'Georgian', 'ka-GE')
INSERT INTO [dbo].[Language] ([LanguageId], [DisplayName], [EnglishName], [Culture]) VALUES (2, 'English', 'English (US)', 'en-US')
GO
SET IDENTITY_INSERT [dbo].[Language] OFF
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[PhotoType] ON
GO
INSERT INTO [dbo].[PhotoType] ([PhotoTypeId], [DisplayName], [Description]) VALUES (1, 'Thumbnail', 'Thumbnail 250x250')
INSERT INTO [dbo].[PhotoType] ([PhotoTypeId], [DisplayName], [Description]) VALUES (2, 'Fullsize', 'Full-size photo')
GO
SET IDENTITY_INSERT [dbo].[PhotoType] OFF
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[MilitaryGroup] ON
GO
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (1)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (2)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (3)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (4)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (5)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (6)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (7)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (8)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (9)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (10)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (11)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (12)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (13)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (14)
INSERT [dbo].[MilitaryGroup] ([MilitaryGroupId]) VALUES (15)
GO
SET IDENTITY_INSERT [dbo].[MilitaryGroup] OFF
GO
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (1, N'სამხედრო სახმელეთო ძალები', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (2, N'სამხედრო საზღვაო ძალები', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (3, N'სამხედრო საჰაერო ძალები', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (4, N'I ქვეითი ბრიგადა', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (5, N'II ქვეითი ბრიგადა', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (6, N'III ქვეითი ბრიგადა', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (7, N'IV ქვეითი ბრიგადა', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (8, N'V ქვეითი ბრიგადა', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (9, N'საინჟინრო ბრიგადა', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (10, N'ცალკეული ჯავშანსატანკო ბატალიონი', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (11, N'ცალკეული მსუბუქი ქვეითი ბატალიონი', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (12, N'სპეციალური ოპერაციების დაჯგუფება', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (13, N'ჯარების ლოგისტიკური უზრუნველყოფის დეპარტამენტი', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (14, N'ეროვნული გვარდია', NULL, 1)
INSERT [dbo].[MilitaryGroupTranslation] ([MilitaryGroupId], [GroupName], [Description], [LanguageId]) VALUES (15, N'ს/დ დეპარტამენტი, I ოპერატიული სამმართველოს თანამშრომელი', NULL, 1)
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[MilitaryRank] ON
GO
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (1)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (2)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (3)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (4)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (5)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (6)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (7)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (8)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (9)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (10)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (11)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (12)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (13)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (14)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (15)
INSERT [dbo].[MilitaryRank] ([MilitaryRankId]) VALUES (16)
SET IDENTITY_INSERT [dbo].[MilitaryRank] ON
GO
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (1, N'კაპიტან-ლეიტენანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (2, N'კაპრალი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (3, N'კაპიტანი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (4, N'სერჟანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (5, N'ავიაციის უმცროსი სერჟანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (6, N'ვადიანი ს/მ', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (7, N'ლეიტენანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (8, N'უმცროსი სერჟანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (9, N'უფროსი ლეიტენანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (10, N'მაიორი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (11, N'უფროსი სერჟანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (12, N'სამედიცინო სამსახურის სერჟანტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (13, N'პოლკოვნიკი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (14, N'ეს/პირი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (15, N'რეზერვისტი', NULL, 1)
INSERT [dbo].[MilitaryRankTranslation] ([MilitaryRankId], [RankName], [Description], [LanguageId]) VALUES (16, N'ოფიცერი', NULL, 1)
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
