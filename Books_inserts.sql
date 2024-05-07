INSERT INTO genres
VALUES ('High Fantasy');
INSERT INTO genres
VALUES ('Classic');
INSERT INTO genres
VALUES ('Adventure');
INSERT INTO genres
VALUES ('Fiction');

INSERT INTO publishing_houses
VALUES ('Kremówka', NULL, 'Brak');
INSERT INTO publishing_houses
VALUES ('Świetne wydawnictwo', NULL, 'Brak');

INSERT INTO authors
VALUES ('John', 'Tolkien');

INSERT INTO books
VALUES ('The Hobbit');

INSERT INTO books_authors
VALUES (1, 1);

INSERT INTO books_genres
VALUES (1, 1);
INSERT INTO books_genres
VALUES (1, 3);
INSERT INTO books_genres
VALUES (1, 4);

INSERT INTO books_editions
VALUES (1, 1, 'The Hobbit', '2017-09-08T19:04:14.480Z');
INSERT INTO books_editions
VALUES (1, 1, 'Hobbit, czyli tam i z powrotem', '2017-09-09T19:04:14.480Z');