# Web API endpoint naming conventions
- [ ] Use nouns, not verbs (e.g. `localhost/users` instead of `localhost/getUsers`)
- [ ] Use plural form if the resource is not explicitly defined as singular (e.g. `localhost/users` instead of `localhost/user`)
- [ ] Use clear names with a single interpretation, do not use abbreviations (e.g. `localhost/admin-click-analytics` instead of `localhost/analtcs`)
- [ ] Use forward slashes to denote hierarchy (e.g. a resource that is considered a child to another should be located under it: `localhost/users/123456/profile-image` instead of `localhost/user-profile-image`)
- [ ] Separate words with hypens (e.g. `localhost/admin-click-analytics` instead of `localhost/adminClickAnalytics`)
- [ ] Use lowercase letters (even though ASP.NET is case-insensitive, URL standards dictate that they are case-sensitive)
- [ ] Avoid non-standard characters (e.g. `localhost/drinks/beer` instead of `localhost/drinks/öl`)
