# HCI-PZ2

WireFrame:  https://app.uizard.io/p/96c2d030

MeteringSimulator - T3 and NetworkEntitiesViewModel:

Primjer: 
• Omogućava dodavanje i brisanje entiteta za monitoring.

• Čuvanje osnovnih podataka o entitetima u vidu tabele, pri čemu nova dobijena brojevna vrednost merenja se prikazuje u jednoj od kolona tabele.

• Prilikom dodavanja entiteta, potrebno je dodeliti vrednosti svim svojstvima entiteta.

• Omogućiti pretragu ili filtriranje prikaza u tabeli. Pretraga se vrši na osnovu naziva ili tipa entiteta.

![Prvi prozor](https://github.com/MastilovicRadoslav/HCI-PZ2/assets/122049689/6c74ca5a-8a83-425e-aad3-6e34ac08e8ff)

![Prvi prozor_1](https://github.com/MastilovicRadoslav/HCI-PZ2/assets/122049689/c7eb0250-6895-418e-8d35-0538b54214f9)

NetworkDisplayViewModel:

Primjer:
• Sadrži prostor za vizuelni prikaz entiteta i simuliranje njihovog rasporeda u sistemu/mreži.
• Omogućiti korisniku da pomeranjem entiteta Drag&Drop tehnikom definiše njihov raspored na mreži.
• Vizuelno izmeniti prikaz entiteta ukoliko simulirana novoizmerena vrednost bude ispod ili iznad zadate granice opasnosti.
• Omogućiti prevlačenje entiteta sa Drag&Drop mreže nazad u TreeView kontrolu.
• Omogućiti spajanje entiteta linijama koje će se pomerati zajedno sa entitetima prilikom pomeranja na Drag&Drop mreži.

![Drugi prozor](https://github.com/MastilovicRadoslav/HCI-PZ2/assets/122049689/4a1d46cb-bb67-46e4-9a9d-a28e3d8cd435)

MeasurementGraphViewModel:

Primjer:
• Prikazivanje istorije stanja entiteta pomoću grafikona.
• Grafikoni se konstantno menjaju na osnovu novih informacija, prikazujući poslednjih pet merenja.
• Omogućiti korisniku izbor entiteta za koji se iscrtava grafikon (istorija merenja).
• Grafikoni se crtaju programski, bez korišćenja gotovih Chart kontrola.

![Treci prozor](https://github.com/MastilovicRadoslav/HCI-PZ2/assets/122049689/2d953a5d-8a5c-4393-ae7f-c82709e3334e)


        


