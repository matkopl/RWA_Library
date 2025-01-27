--create database WebLibrary
--go

--use WebLibrary
--go

-- 1-N
create table Genre (
    Id int primary key identity(1,1),
    [Name] nvarchar(50) not null
)
go

-- 1-N
create table Book (
    Id int primary key identity(1,1),
    [Name] nvarchar(255) not null,
	Author nvarchar(255) not null,
    [Description] nvarchar(max),
	IsAvailable bit not null,
    GenreId int not null foreign key references Genre(Id) on delete cascade
)
go

-- 1-N
create table [Location] (
    Id int primary key identity(1,1),
    [Name] nvarchar(255) not null
)
go

-- M-N bridge
create table BookLocation (
	Id int primary key identity(1,1),
	BookId int not null foreign key references [Book](Id) on delete cascade,
	LocationId int not null foreign key references [Location](Id) on delete cascade

)
-- 1-N
create table [User] (
    Id int primary key identity(1,1),
    UserName nvarchar(50) not null unique,
    PwdHash nvarchar(256) not null,
    PwdSalt nvarchar(256) not null,
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,
	IsAdmin bit not null,
	Email nvarchar(50) not null unique,
	Phone nvarchar(50) not null
)
go
-- M-N
create table Reservation (
    Id int primary key identity(1,1),
    UserId int not null foreign key references [User](Id) on delete cascade,
    BookId int not null foreign key references Book(Id) on delete cascade,
	LocationId int not null foreign key references [Location](Id) on delete cascade,
    ReservationDate datetime not null default getdate()
)
go

-- Logs
create table Log (
    Id int primary key identity(1,1),
    [Message] nvarchar(max) not null,
    [Level] int not null,
    [Timestamp] datetime not null default getdate()
)
go

/*-- Genres
insert into Genre([Name]) 
values
('Science Fiction'),
('Biography'),
('Mystery'),
('Romance'),
('Non-Fiction');

-- Books
insert into Book ([Name], [Author], [Description], [IsAvailable], [GenreID])
values
-- Science Fiction
('Foundation', 'Isaac Asimov', 'A classic science fiction series about a galactic empire.', 1, 1),
('Hyperion', 'Dan Simmons', 'A science fiction novel with interconnected tales.', 1, 1),
('Neuromancer', 'William Gibson', 'A novel that popularized cyberpunk.', 1, 1),
('Snow Crash', 'Neal Stephenson', 'A cyberpunk novel about virtual reality and hacking.', 1, 1),
('The Left Hand of Darkness', 'Ursula K. Le Guin', 'A science fiction novel exploring gender and society.', 1, 1),

-- Biography
('The Diary of a Young Girl', 'Anne Frank', 'The diary of Anne Frank during WWII.', 1, 2),
('Long Walk to Freedom', 'Nelson Mandela', 'The autobiography of Nelson Mandela.', 1, 2),
('Steve Jobs', 'Walter Isaacson', 'The biography of Apple co-founder Steve Jobs.', 1, 2),
('Einstein: His Life and Universe', 'Walter Isaacson', 'Biography of Albert Einstein.', 1, 2),
('The Glass Castle', 'Jeannette Walls', 'A memoir of resilience and hardship.', 1, 2),

-- Mystery
('The Hound of the Baskervilles', 'Arthur Conan Doyle', 'A Sherlock Holmes mystery.', 1, 3),
('And Then There Were None', 'Agatha Christie', 'A mystery novel by Agatha Christie.', 1, 3),
('The Silent Patient', 'Alex Michaelides', 'A psychological thriller about a silent woman.', 1, 3),
('The Girl on the Train', 'Paula Hawkins', 'A mystery involving memory and crime.', 1, 3),
('Sharp Objects', 'Gillian Flynn', 'A mystery novel about family secrets.', 1, 3),

-- Romance
('The Fault in Our Stars', 'John Green', 'A touching romance about young love.', 1, 4),
('Twilight', 'Stephenie Meyer', 'A supernatural romance involving vampires.', 1, 4),
('Eleanor & Park', 'Rainbow Rowell', 'A love story of two misfit teens.', 1, 4),
('The Time Traveler''s Wife', 'Audrey Niffenegger', 'A romance involving time travel.', 1, 4),
('Outlander', 'Diana Gabaldon', 'A historical romance with time travel elements.', 1, 4),

-- Non-Fiction
('The Immortal Life of Henrietta Lacks', 'Rebecca Skloot', 'The story behind HeLa cells.', 1, 5),
('Thinking, Fast and Slow', 'Daniel Kahneman', 'An exploration of human psychology.', 1, 5),
('Educated', 'Tara Westover', 'A memoir about overcoming adversity.', 1, 5),
('Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', 'A history of humanity.', 1, 5),
('Quiet', 'Susan Cain', 'The power of introverts in a loud world.', 1, 5);

-- Add Locations
insert into Location ([Name])
values
('Zagreb'),
('Dubrovnik'),
('Split'),
('Rijeka'),
('Osijek');

-- Assigning Books to Locations
insert into BookLocation ([BookId], [LocationId])
values
-- Science Fiction
(1, 1), (1, 2),
(2, 3), (2, 4),
(3, 5),
(4, 1), (4, 2),
(5, 3), (5, 4),
-- Biography
(6, 1), (6, 2),
(7, 3), (7, 4),
(8, 5),
(9, 1), (9, 2),
(10, 3), (10, 4),
-- Mystery
(11, 1), (11, 2),
(12, 3), (12, 4),
(13, 5),
(14, 1), (14, 2),
(15, 3), (15, 4),
-- Romance
(16, 1), (16, 2),
(17, 3), (17, 4),
(18, 5),
(19, 1), (19, 2),
(20, 3), (20, 4),
-- Non-Fiction
(21, 1), (21, 2),
(22, 3), (22, 4),
(23, 5),
(24, 1), (24, 2),
(25, 3), (25, 4);
*/
