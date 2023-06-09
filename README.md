# Bankovní systém

## Úvod

### Účel dokumentu

Cílem tohoto dokumentu je nastínit koncept a upřesnit požadavky aplikace pro zobecněný bankovní systém.  

V první části tohoto dokumentu je popsán rozsah a kontext systému, obecný přehled jeho funkcí, charakteristika jeho cílové domény, omezení, předpoklady a závislosti

V druhé části pak vnější rozhraní, podrobnější funkce a popis databáze, softwarové i hardwarové požadavky systému, detailnější rozbor rizik. 

### Účel systému

Systém nabízí zjednodušenou aplikaci internetového bankovnictví skrze webové rozhraní s možností dvoufázového přihlašování a multiměnového bankovního účtu, kde bude uživateli umožněno plaby přijímat a odesílat, procházet zůstatky pro jednotlivé měny jeho účtu, případně měny přidávat. 

## Obecné požadavky na systém

### Rozsah projektu - obecné funkcionality 

- aplikace s webovým rozhraním 
- dvoufázové přihlašování 
- podpora multiměnového účtu 
- tlačítka "plus" a "minus" pro "placení" a "nabývání peněz" 
- automatizované převádění měny v případě nedostatečného zůstatku na účtu v cizí měně na měnu místní (CZK)
    - podporované měny: 
        - CZK - Česká koruna 
        - EUR - Euro 
        - USD - Americký dolar
        - GBP - Libra šterlinků 
        - CAD - Kanadský dolar 
        - KRW - Jihokorejský won 
        - NOK - Norská koruna 
        - JPY - Japonský jen
        - EGP - Egyptrská libra 
        - DKK - Dánská koruna 
- možnost přidávání cizích měn (v omezeném rozsahu na podporované měny) 
- základní zabezpečení proti chybám (viz níže)
- užvatel si účet nemůže vytvořit sám, účty jsou vytvářeny manuálně vývojářem 

### Uživatelské grafické rozhraní 
- vtupním bodem aplikace je přihlašovací stránka s přihlašovacím formulářem
    - zde se uživatel přihlásí do aplikace s využitím dvoufázového ověření - po zadání přihlašovacích údajů, které sestávají z emailové adresy a hesla, se uživateli zobrazí stránka s QR kódem a Manuálním Setup Kódem. Uživatel nyní musí využít 

- hlavní stránka bankovního účtu: 
    - výpis základních informací o účtu uživatele 
        - jmenovitě: číslo účtu, emailová adresa
    -  2 tlačítka pro odesílání a přijímání finančních prostředků - stlačení jednoho nebo druhého je vyvolána akce, která náhodně zvolí měnu a částku (v definovaném rozsahu 10 až 100 000 jednotek dané měny), která má být odeslána, či přijata. Měny jsou voleny zcela náhodně ze seznamu aplikací podporovných měn, nezávisle na měnách, ve kterých uživatel jeho účet vede
        - v případě, že je pro vklad vybrána částka v měně, ve které uživatel nevede svůj účet, vklad není proveden
        - v případě, že je vybrána pro platbu měna, ve ketré uživatel nevede účet, nebo pokud na účtu dané měny není dostatečný zůstatek, aplikace v závislosti na aktuálním kurzovním lístku České Národní Banky (lístek je aktuální pouze v rámci možností - ve chvíli, kdy ČNB vydává nový kurzovní lístek se aplikace snaží o aktualizaci svého současného lístku, ale s největší pravděpodobností se nepodaří získat aktuální lístek přesně v tu danou chvíli, kdy ho ČNB vydá, nebo ČNB změní formát svého lístku a v tu chvíli aplikace ztrácí možnost aktualizace svého definitivně), je částka v cizí měně převedena na hlavní měnu účtu (kterou je CZK) a daná částka je stržena z účtu v CZK, nebo pokud na tomto účtu není dostatečný zůstatek, platba není provedena. 
    - zůstatek CZK na účtu - hlavní měna účtu je vždy CZK, ne jiná, proto se na hlavní stránce vždy zobrazován zůstatek této měny 
    - výpis z účtu CZK
    - select box přepínání měn - zobrazuje měny ve kterých uživatel vede svůj účet, výběrem jiné měny je přepsána část stránky se zůstatkem v hlavní měně a vypíše pohyby a zůstatek v měně jiné 
    - tlačítko přidání měny - stlačení - zobrazení select boxu s výběrem z měn, ve kterých uživatel zatím účet nevede, toto tlačítko se přestane zobrazovat v okamžiku, kdy uživatel již vede účet ve všech nabízených měnách 
- chybové stavy
    - ztráta internetového připojení - aplikace se uživateli nezobrazí, proto nemusí být tato informace uživateli nijak sdělována 
    - zadání chybných hodnot v přihlašovacím formuláři - zobrazení hlášky o zadání chybných hodnot a umožnění zadání hodnot znovu
        - za chybné hodnoty je považována emailová adresa, pro kterou neexistuje číslo účtu, zadání chybného hesla
        - neúspěšně odeslaný email dvoufázového ověření - zobrazení hlášky, uživatel se může pokusit o opětovné odeslání emailu
    - pokud se nepodařilo získat aktuální kurzovní lístek - zobrazení hlášky informující uživatele, že pohyby na účtech mezi měnami nyní probíhají dle zastaralého kurzovního lístku (podrobnější informace o získávání kurzovního lístku - viz "Další poznámky")
    - chyba komunikace se serverem - zobrazení chybové stránky pokud nastala chyba na straně hostitelského serveru 

### kontext projektu 
- aplikace s webovým rozhraním určená pro použití na mobilních telefonech a desktopech, nezávislá na operačním systému  
- aplikace je určená pro individuální, základně poučené uživatele 

### omezení, předpoklady a závilosti 
- pro správnou funkci aplikace je nutný přístup k internetu 
- předpokladem je nainstalovný vhodný internetový prohlížeč - není zaručena správná funkce na starších a jiných prohlížečích, než jsou prohlížeče s jádrem Chromium verze 112.0.5615
- uživatel musí vlastnit a mít příytup k emailové schránkce 
- aplikace není vhodná pro uživatele se zrakovou, nebo jinou indispozicí znemožňující pohodlnou práci s běžným webovým rozhraním 


### využité technologie 
- ASP .NET 6.0 s Razor Pages 
- Databáze - MySql 
- Apache server

### popis databáze 
- 2 tabulky
    - klienti - uložení přihlašovacích údajů a čísla účtu 
    - účty - ukládání změny na každém účtě v každé měně 
        - id záznamu 
        - číslo účtu - cizí klíč z tabulky "klienti"
        - měna 
        - hodnota - aktuální zůstatek na účtu 

### softwarové a hardwarové požadavky systému na hostitelský server
    - 64-bit x86 CPU
    - 1 GB RAM
    - až 3 GB paměti na pevném disku 
    - operační systém preferovaný CentOS/RHEL 7.2+ nebo Ubuntu 16.04(.2) a vyšší

### další rizika 
- možné budoucí omezení rychlosti z důvodu přílišné velikosti tabulky "účty", jejíž množství řádků roste s přibývajícím množstvím uživatelů exponenciálně 
- nemožnost získání dat od ČNB, nebo případná změna jejich formátu - systém uživatele informuje, že není možno získat aktuální data 

### další poznámky 
- implementace získávání dat od ČNB skrze dotazování 
    - je známo, že ČNB v pravidelných intervalech - okolo 14:00 hodin vyjma svátků a víkendu, aktualizuje svůj kurzovní lístek, proto je třeba okolo této hodiny hlídat v daných intervalech (frekvence dotazování je podobná normálnímu rozdělení) aktualizaci kurzovního lístku pro získání aktuálních dat pro nutné převody měn. 
        - dotazování je běžně přerušeno v okamžiku nabytí nového kurzovného lístku, zde je vypsán kompletní plán dotazování v tom případě, pokud se jedná o víkend, nebo svátek, nebo z jiného důvodu nakonec program nenabyl nových dat 
        - 13:30 - počátek dotazování
        - do 13:55 - dotazování v intervalech po 5 minutálch 
        - 13:55 - 13:59 - dotazování v intervalech po 1 minutě
        - 13:59 - 14:01 - dotazování v intervalech po 15 sekundách
        - 14:01 - 14:05 - dozazování v intervalech po 1 minutě
        - 14:05 - 14:30 - dotazování v intervalech po 5 minutách
        - 14:30 - konec dotazování 
      

### časové rozvržení 
- definice problematiky a definice prostředků   - 2 týdny 
- prototyp                                      - 1 týden 
- sestavení apliace                             - 2 týdny
- testování aplikace                            - 2 týdny
- nasazení aplikace + případné úravy            - 1 až 2 týdny (dle rozsahu případných úprav)
