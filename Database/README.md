# Tietokanta (vn)

Sovellus käyttää paikallista MySQL-kantaa nimeltä `vn`. Data ei kulje GitHubin
kautta automaattisesti, joten kannan rakenne ja data on tallennettu tiedostoon
`vn.sql` (mysqldump). Tiedosto luo kannan `vn`, kaikki taulut ja nykyisen datan
(alueet, asiakkaat, mökit, postinumerot).

## Kannan tuonti omaan MySQL:ään

Vaihtoehto A, MySQL Workbench:

1. Avaa yhteys omaan paikalliseen MySQL:ään.
2. File > Open SQL Script... ja valitse `Database/vn.sql`.
3. Aja koko skripti (salamanappi). Päivitä Schemas-lista, `vn` näkyy siellä.

Vaihtoehto B, komentorivi (repon juuresta):

```bash
mysql -u root -p < Database/vn.sql
```

Huom: skripti poistaa ja luo taulut uudelleen, joten jos sinulla on jo oma
`vn`-kanta, sen data korvautuu.

## Connection string

Yhteys on tiedostossa `DatabaseConfig.cs`:

```
server=localhost;port=3306;database=vn;user=root;password=OMA_SALASANASI;
```

Vaihda `user` ja `password` oman MySQL-asennuksesi tunnuksiin. Älä commitoi
omaa salasanaasi.

## Dumpin päivittäminen

Kun kantaan tulee muutoksia, joiden pitää näkyä muillekin, aja uusi dump ja
commitoi `vn.sql`:

```bash
mysqldump -u root -p --databases vn --routines --triggers --no-tablespaces --set-gtid-purged=OFF --result-file=Database/vn.sql
```
