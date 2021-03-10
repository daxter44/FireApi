# FireApi
Jest to API do aplikacji piece. 

Do poprawnego działania wymagane jest: 
- MongoDB
- MySQL 

Solucja Api składa się z :
-FireApi - API 
- FireApi - Database - Projekt .dll z odpowiedzialny za konfig bazy danych, jest tam DataContext zwykłej bazy danych, ustawienia MONGO DB, migracje 
- FireApi - Database - Entity. - Projekt .dll z encjami
- FireApi - Workers - MQTTSync - Projekt worker .exe, który nasłuchuje na kanale MQTT i przyjmuje dane z pieców do zapisania w bazie danych. 

Omówienie encji : 
 - Device - dane urządzenia 
 - MongoDoc - encje wymagane do tworzenia dokumentów Mongo
 - Adress, Client, Firm, User - jak nazwa wskazuje
 
 Konfiguracja 
 - SharedSettings.json w głównym katalogu - Dane do maila, i connection string do baz
 
 Aby uruchomić projekt należy najpierw skonfigurować Mysql, MongoDB oraz utworzyć bazy takie jak wskazuje SharedSettings.json
 
