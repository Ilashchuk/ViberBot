��� ��������� ����� ��� �� ���������� ����� �������:
1) ����������� ���� �����:
	- �������� ���� ���� �����
	- ��������� ������ � SQL_Tables
	- ��������� ������ � SQL_Functions
	- ��������� ������� � SQL_Procedures
	- ������ ConnectionStrings � ���� appsettings.json
2) ��������� ��������� ����� � ������ ������� ngrok http 5085
3) ����������� API
4) ��������� ��� ������� ngrok �������� � viber.json �� Program.cs
5) ��������� ��������� ����� � ����� � ��� ��������� viber.json
6) ��������� �������: 
curl -# -i -g -H "X-Viber-Auth-Token:50e3edf02727e768-f7acbe1bb3344977-76094c86e1902170" -d @viber.json -X POST https://chatapi.viber.com/pa/set_webhook -v