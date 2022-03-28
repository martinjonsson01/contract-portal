# Git Workflow
* Commit style: https://cbea.ms/git-commit/ 
* Commits ska vara [atomiska](https://www.freshconsulting.com/insights/blog/atomic-commits/):
  * En icke-atomisk commit lämnar koden i en icke-kompilerbar stat.
  * Commit:a inte ofärdiga rader kod.
  * En atomisk commit är så liten som möjligt (atom => minsta beståndsdel):
    * T.ex. är *“Create new database class”* atomisk
    * *“Add authentication to database class”* är atomisk
    * *“Refactor database to use SQL queries instead of ORM”* är **INTE** atomisk eftersom här ändras troligen mycket kod och på flera ställen.
  * Gör commits ofta och små.
  * Man ska kunna läsa din commit-historik som en berättelse av vilka ändringar du introducerat för att utveckla en feature, detta är viktigt för att underlätta för PR-reviews.
* Branches: 
  * Namn: Issue ID + story/task name i gemener och med bindestreck, t.ex. *18-pawn-inventories* (*18* är issue ID från GitHub, *Pawn Inventories* är namnet på task:en).
  * Varje task görs på en separat branch och mergas tillbaka in i user story-branchen som sedan till slut mergas tillbaka in i master. Det behövs bara reviews för varje task, inte för story:n. 
  * När storyn är klar enligt DoD då har den mergats in i master igen.
* Alla stories, buggar och epics lagras på GitHub inuti vårt Project, och är därmed länkade till koden och alla PRs
* Master och alla user story-branches är skyddade och kan endast ändras genom review:ade PRs. När en user story ska merga:s in i master behövs ingen riktigt review, bara att någon annan i laget har konfirmerat att den är klar.

