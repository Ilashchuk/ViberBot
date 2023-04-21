Щоб запустити даний бот на локальному порту потрібно:
1) Налаштувати базу даних:
	- Створити нову базу даних
	- Запустити скрипт з SQL_Tables
	- Запустити скрипт з SQL_Functions
	- Запустити скрипти з SQL_Procedures
	- Змінити ConnectionStrings у файлі appsettings.json
2) Викликати командний рядок і ввести команду ngrok http 5085
3) Скомпілювати API
4) Посилання яке видасть ngrok записати в viber.json та Program.cs
5) Запустити командний рядок з папки в якій розміщений viber.json
6) Викликати команду: 
curl -# -i -g -H "X-Viber-Auth-Token:50e3edf02727e768-f7acbe1bb3344977-76094c86e1902170" -d @viber.json -X POST https://chatapi.viber.com/pa/set_webhook -v