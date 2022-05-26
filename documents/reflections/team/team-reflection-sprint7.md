# Team Reflection Sprint 7
Närvarande: Emil, Mathias, Pawel, Martin, Jacob, Jonathan
## Customer Value and Scope

### User stories
Vi har haft två stycken enorma pull requests de senaste sprintarna, bägge har tagit 2 sprints att slutföra, vilket är fel. De har även varit väldigt svåra och tidskrävande att review:a, då många ändringar har gjorts i dem.
Vi hade velat att varje pull request är liten och varje story görs klart snabbt, inom en sprint. Man ska kunna göra klart flera user stories på en sprint. 
I framtiden kommer vi att tänka på att grundläggande koncept som, användare i detta fallet, borde implementeras tidigt och i rätt ordning så att det inte krävs stora refactors i koden för att introducera dem. Om det märks att en story kommer kräva en refactor redan innan arbete påbörjas så bör detta troligtvis brytas ut i en separat task eller en helt annan user story, för att låta detta bli klart snabbare. 

## Design decisions and product structure

### Code quality and standards
Vi har haft ett robust arbetsflöde när det gäller kodkvalitet, då vi har automatisk style-kontrollering och automatisk testkörning innan kod merge:as till master. Vi har även bestämt i vårt sociala kontrakt att en pull request inte får merge:as förrän två andra i laget har review:at den, och detta har upprätthållits genom att GitHub inte tillåter en merge förrän dessa kriterier är uppfyllda. Detta arbetssätt har varit väldigt robust, fastän det har saktat ned oss har den låtit oss upprätthålla hög kodkvalitet. Det har även gett oss insikt i hur andra bitar av koden är skriven. 

Problem uppstod denna sprinten då vi märkte att testerna gick igenom fastän de inte borde, vilket berodde på ett problem i konfigurationen av GitHub. Detta innebär att de senaste 9 dagarna har pull requests blivit godkända fastän testerna inte nödvändigtvis varit det. Som tur är har vi kört testerna lokalt ändå (för det mesta) och löst problem när de uppstod. Problemet märktes först nu för att ett misstag skedde vilket ledde till att trasig kod blev merge:ad till master.
Önskvärt hade varit att testerna körs och kod som inte passerar testerna spärras från master. För att uppnå detta har vi fixat konfigurationen på GitHub. Vi kommer även att försöka (fastän det inte är ett krav, för att inte sakta ned oss) dra ned koden från pull requests och testa lokalt när vi gör code reviews.

### Documentation
Vi har inte haft några detaljerade designskisser av hur den slutgiltiga webbplatsen ska se ut, och vi har därför behövt improvisera under projektets gång. Detta har lett till ihopslängd och sisådär design, vilket har kommenterats på ett flertal under våra reviews med stakeholder. En konsekvens av detta är att vi har behövt göra om våran styling flera gånger, för att göra det snyggare i efterhand. 
Vi borde försökt få tag i en bestämd gränssnittsdesign från stakeholders redan från början, och om detta inte lyckades göra våran egen. På detta vis hade vi kunnat integrera designarbetet i vartenda story redan från början, vilket hade minskat arbete nu i efterhand. 
För att åtgärda detta kommer vi nu att arbeta ikapp oss när det gäller styling, genom att integrera styling i varje story vi arbetar på just nu. All våran styling utgår nu mer explicit från stylingen på stakeholders officiella webbplats. 
