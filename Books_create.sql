-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-05-06 11:46:50.815

-- tables
-- Table: authors
CREATE TABLE authors (
    PK int  NOT NULL IDENTITY,
    first_name nvarchar(50)  NULL,
    last_name nvarchar(100)  NOT NULL,
    CONSTRAINT authors_pk PRIMARY KEY  (PK)
);

-- Table: books
CREATE TABLE books (
    PK int  NOT NULL IDENTITY,
    title nvarchar(100)  NOT NULL,
    CONSTRAINT books_pk PRIMARY KEY  (PK)
);

-- Table: books_authors
CREATE TABLE books_authors (
    FK_book int  NOT NULL,
    FK_author int  NOT NULL,
    CONSTRAINT books_authors_pk PRIMARY KEY  (FK_book,FK_author)
);

-- Table: books_editions
CREATE TABLE books_editions (
    PK int  NOT NULL IDENTITY,
    FK_publishing_house int  NOT NULL,
    FK_book int  NOT NULL,
    edition_title nvarchar(100)  NOT NULL,
    release_date datetime  NOT NULL,
    CONSTRAINT books_editions_pk PRIMARY KEY  (PK)
);

-- Table: books_genres
CREATE TABLE books_genres (
    FK_book int  NOT NULL,
    FK_genre int  NOT NULL,
    CONSTRAINT books_genres_pk PRIMARY KEY  (FK_book,FK_genre)
);

-- Table: genres
CREATE TABLE genres (
    PK int  NOT NULL IDENTITY,
    name nvarchar(100)  NOT NULL,
    CONSTRAINT genres_pk PRIMARY KEY  (PK)
);

-- Table: publishing_houses
CREATE TABLE publishing_houses (
    PK int  NOT NULL IDENTITY,
    name nvarchar(100)  NOT NULL,
    owner_first_name nvarchar(50)  NULL,
    owner_last_name nvarchar(100)  NOT NULL,
    CONSTRAINT publishing_houses_pk PRIMARY KEY  (PK)
);

-- foreign keys
-- Reference: books_authors_authors (table: books_authors)
ALTER TABLE books_authors ADD CONSTRAINT books_authors_authors
    FOREIGN KEY (FK_author)
    REFERENCES authors (PK);

-- Reference: books_authors_books (table: books_authors)
ALTER TABLE books_authors ADD CONSTRAINT books_authors_books
    FOREIGN KEY (FK_book)
    REFERENCES books (PK);

-- Reference: books_editions_books (table: books_editions)
ALTER TABLE books_editions ADD CONSTRAINT books_editions_books
    FOREIGN KEY (FK_book)
    REFERENCES books (PK);

-- Reference: books_editions_publishing_houses (table: books_editions)
ALTER TABLE books_editions ADD CONSTRAINT books_editions_publishing_houses
    FOREIGN KEY (FK_publishing_house)
    REFERENCES publishing_houses (PK);

-- Reference: books_genres_books (table: books_genres)
ALTER TABLE books_genres ADD CONSTRAINT books_genres_books
    FOREIGN KEY (FK_book)
    REFERENCES books (PK);

-- Reference: books_genres_genres (table: books_genres)
ALTER TABLE books_genres ADD CONSTRAINT books_genres_genres
    FOREIGN KEY (FK_genre)
    REFERENCES genres (PK);

-- End of file.

