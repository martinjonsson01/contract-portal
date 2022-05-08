# Individuella reflektioner för Martin
## Läsvecka 6

## Vad vill jag lära mig eller förstå bättre?
Jag funderar fortfarande kring bugghantering i Scrum, och imorgon kommer jag att ta upp detta under våran sprint retrospective, så hoppas jag att jag kan lära mig något från de andra angående detta.

## Hur kan jag hjälpa någon annan eller hela laget att lära sig något nytt?
Jag har hjälpt de andra i laget att lära sig om vårat databashanteringssystem (Entity Framework Core), då jag har tidigare erfarenhet av detta. Det har varit många grejer som är lite kluriga när man aldrig använt verktygen innan, men då har jag funnits tillgänglig för att hjälpa till. I fortsättningen tänker jag fortsätta vara öppen för den här typen av frågor och problem.

## Vad har jag bidragit med till vårt användande av Scrum?
Hittills har det faktiskt inte varit särskilt farligt att baka in UI-poleringen i våra user stories (åtminstone de jag arbetat på), det har oftast varit lite småjusteringar här och där som inte tagit särskilt lång tid. Det märks redan nu efter en sprint att den visuella kvalitén på webbplatsen är nu mycket bättre, det är mycket snyggare. I fortsättningen kommer jag att göra gränssnittsdesignen rätt redan från början, när jag implementerar nya delar av gränssnittet.


## Vad har jag bidragit med till lagets leveranser?
Denna sprinten, utöver det vanliga med PR-reviews och user stories, har jag löst ett pågående problem vi haft med våran Continuous Integration-pipeline. Våra tester har haft det jobbiga problemet att de oftast misslyckas när de körs på GitHub (fastän de fungerar lokalt), men om man kör om de några gånger så lyckas de till slut. Detta kunde jag lösa, och fastän jag lade mycket tid på detta tror jag att det lönar sig i längden tack vare den säkerhet man får i att veta att testerna fortfarande passerar som de ska (efter alla merge-conflicter som kan ske osv...).