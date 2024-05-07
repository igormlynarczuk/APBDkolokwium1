-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-05-06 11:46:50.815

-- foreign keys
ALTER TABLE books_authors DROP CONSTRAINT books_authors_authors;

ALTER TABLE books_authors DROP CONSTRAINT books_authors_books;

ALTER TABLE books_editions DROP CONSTRAINT books_editions_books;

ALTER TABLE books_editions DROP CONSTRAINT books_editions_publishing_houses;

ALTER TABLE books_genres DROP CONSTRAINT books_genres_books;

ALTER TABLE books_genres DROP CONSTRAINT books_genres_genres;

-- tables
DROP TABLE authors;

DROP TABLE books;

DROP TABLE books_authors;

DROP TABLE books_editions;

DROP TABLE books_genres;

DROP TABLE genres;

DROP TABLE publishing_houses;

-- End of file.

