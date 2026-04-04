# Currency Converter with API (WPF)

Tämä projekti on tehty kurssilla **“Olio-ohjelmointi ja WPF-sovellukset”**. Sovelluksen tarkoitus on harjoitella WPF-sovelluksen perustoimintoja, erityisesti **JSON-muotoisen datan noutamista verkkopalvelusta rajapinnan avulla asynkronisesti**.

Sovellus hakee valuuttakurssit palvelusta **freecurrencyapi.com**, joka vaatii käyttäjältä oman henkilökohtaisen API-avaimen.

## Ominaisuudet

- Valuuttakurssien haku ulkoisesta rajapinnasta (JSON)
- Asynkroninen tiedonhaku (UI pysyy responsiivisena)
- Käyttäjän API-avaimen tallennus paikallisesti `setup.ini`-tiedostoon
  - Jos avainta ei löydy sovellusta käynnistettäessä, käyttäjää pyydetään syöttämään se (tai sovellus muuten ohjaa avaimen lisäämiseen projektin toteutuksen mukaisesti)

## Teknologiat

- WPF-sovellus
- .NET 8
- HTTP-rajapintakutsut ja JSON-datan käsittely
- INI-tiedoston käyttö asetusten tallettamiseen (`setup.ini`)

## Käyttöönotto

### 1) Hanki API-avain

1. Rekisteröidy palveluun: http://freecurrencyapi.com
2. Luo henkilökohtainen API-avain

### 2) Lisää API-avain sovellukselle

Sovellus käyttää `setup.ini`-tiedostoa API-avaimen tallennukseen.

- Jos `setup.ini` puuttuu, sovellus luo sen / pyytää avaimen (projektin toteutuksen mukaan).
- Jos `setup.ini` löytyy, sovellus lukee avaimen siitä käynnistyksen yhteydessä.

> Suositus: **Älä committaa `setup.ini`-tiedostoa GitHubiin**, koska se sisältää henkilökohtaisen avaimen. Lisää se tarvittaessa `.gitignore`-tiedostoon.

## Ajaminen (Visual Studio)

1. Avaa ratkaisu Visual Studiossa
2. Palauta NuGet-paketit (tarvittaessa)
3. Käynnistä projekti (F5)

## Oppimistavoitteet (miksi tämä projekti tehtiin)

Projektin tavoitteena on harjoitella erityisesti:

- WPF-sovelluksen perusrakennetta ja käyttöliittymän toimintaa
- Asynkronisia verkkokutsuja (`async/await`) ilman käyttöliittymän jäätymistä
- JSON-datan hakemista ja käsittelyä käytännössä
- Asetusten pysyväistallennusta yksinkertaisesti (API-avain `setup.ini`-tiedostoon)
- Pienimuotoisen sovelluksen kokonaisuuden hallintaa (UI + logiikka + integraatio)

## Tulevaisuuden kehitysideoita

- Virheenkäsittelyn laajentaminen (esim. selkeämmät ilmoitukset: virheellinen avain, verkkovirhe, rajoitukset)
- Välimuisti / viimeisimmän onnistuneen kurssidatan tallennus (offline-ystävällisyys)
- Historiatiedot ja kurssien trendinäkymä (esim. graafi)
- Valuuttalistan haku ja hakutoiminto (suodatus / suosikit)
- MVVM-rakenteen selkeyttäminen ja testattavuuden parantaminen (yksikkötestit logiikalle)
- Asetusten tallennus turvallisemmin (esim. Windows Credential Manager / DPAPI), jos avainta halutaan suojata paremmin

## Lisenssi

Tämä projekti on opintoprojekti. Lisää lisenssi tarvittaessa (esim. MIT), jos julkaiset sen avoimena lähdekoodina.