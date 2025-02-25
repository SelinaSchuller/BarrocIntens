Code Conventies voor Barroc Intens

## Inhoudsopgave:
- Algemene Richtlijnen

- Naamgeving

- Mappenstructuur

- Gegevensbeheer

- Modules en Functionaliteiten

- Notificaties en Meldingen

- UI/UX Ontwerp

- Rechten en Rollen

- Codeerstandaarden

## 1. Algemene Richtlijnen
Gebruik duidelijke, logische namen voor alle functies, klassen en variabelen. Dit maakt de code makkelijker te begrijpen.
Schrijf je code in aparte stukken (modules) per afdeling, zoals Onderhoud, Sales, Inkoop, en Management.
Houd je aan de principes “DRY” (Don’t Repeat Yourself) en “KISS” (Keep It Simple, Stupid).
Voeg commentaar toe bij alle klassen en methoden om uit te leggen wat ze doen, welke gegevens ze nodig hebben, en wat ze teruggeven.

## 2. Naamgeving
Klassen: Schrijf in PascalCase (bijv. KlantServiceHandler, InventarisManager).
Functies en methoden: Gebruik PascalCase (bijv. MaakNieuweOrder, StuurHerinnering).
Variabelen: Gebruik voor interne variabelen en PascalCase voor publieke variabelen (bijv.HuidigeKlant).
Contexten voor naamgeving:

```
3. Mappenstructuur
Maak de volgende mappen aan in de hoofdmap van het project:
```
```
bash
Copy code
project_root/
│
├── inkoop/          # Voor voorraadbeheer en inkoop
├── sales/               # Voor verkoopprocessen, klantcontact en planning
├── onderhoud/           # Voor onderhoudsplanning en voortgangsupdates
├── hoofdonderhoud/          # Voor rapportages, overzichten, en datatoegang
├── ...storingen/        # Voor notificatietemplates en meldingenbeheer
```
## 3. Gegevensbeheer/Database
Klantgegevens: Bewaar klantinformatie (naam, contact, adres, betalingsgeschiedenis) in een database, zodat elke afdeling deze data kan bekijken.
Inventarisgegevens: Houd het aantal producten, lage voorraadniveaus, en herbevoorrading bij.
Salesgegevens: Sla openstaande orders, klantverzoeken en afspraken op.
Contractgegevens: Bewaar de status van contracten, vervaldatums, en stuur automatisch meldingen voor vervaldatums.

## 4. Modules en Functionaliteiten
Klantservice:

Toegang voor Sales en Klantenservice; beheert klantinformatie en houdt communicatiegeschiedenis bij.
Kolommen voor betalingsstatus, herinneringen, en klantcommunicatie.

## Inkoop:
```
Meldingen bij lage voorraad en automatische herbestelling.
Inkoop en Sales hebben toegang om voorraad te bekijken en bestellingen te plaatsen.
```

## HoofdOnderhoud:
```
Interface voor Onderhoud en Sales om klanten op de hoogte te houden van onderhoud.
```
## Storingen systeem:
```
Automatische meldingen voor lage voorraad, openstaande betalingen en contractverlengingen.
Ongelezen berichten bijhouden en herinneringen sturen als nodig.
```
## Onderhoud:
```
Beheer van openstaande onderhoudstaken.
Interface voor planning en updates voor klanten.
```
## 5. Notificaties en Meldingen
Klantherinneringen: Herinner klanten aan openstaande betalingen en informeer over gepland onderhoud.
Interne Meldingen:
```
Waarschuw Inkoop bij lage voorraad.
Informeer Sales als een product niet op voorraad is, zodat zij alternatieven met de klant kunnen bespreken.
Stuur meldingen naar teamleden bij contractondertekening en naderende contractverlopen.
```
## 6. UI/UX Ontwerp
Thema: Huisstijl van het bedrijf – vooral zwart met gele accenten.
Toegangsrechten: Rolgebaseerde weergaven; afdelingen zien enkel de data die voor hen relevant is.

## 7. Rechten en Rollen
Alleen-lezen: Alle afdelingen mogen kerngegevens (klanten, inventaris) bekijken.
Bewerkingsrechten:
Sales kan klantgegevens wijzigen.
Inkoop kan inventarisgegevens aanpassen.
Management heeft beheerdersrechten.
Onderhoud heeft toegang tot planning en onderhoudstaken.
Financien kan contracten en vacatures bewerken of aanpassen.

## 8. Codeerstandaarden
Foutafhandeling:
```
Zorg ervoor dat het systeem netjes omgaat met fouten bij ontbrekende of foutieve data.
Foutmeldingen moeten duidelijk zijn om snel te kunnen oplossen.
```
Testing:
```
Unit tests voor elke module, vooral voor gegevensbeheer en meldingen.
Integratietests voor functies die meerdere afdelingen aangaan.
```
Documentatie:
```
Documenteer functies en modules met uitleg en voorbeelden.
Geef extra uitleg bij complexe modules zoals NotificatieHandler en InventarisHerbestelling.
```
