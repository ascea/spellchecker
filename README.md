# spellchecker

Консольное приложение (C#) по замене слов в соответствии со словарём в пределах двух исправлений

В словарь входят строки до первого вхождения строки "==="

Пример входных данных:
"rain spain plain plaint pain main mainly"
"the in on fall falls his was"
"==="
"hte rame in pain fells"
"mainy oon teh lain"
"was hints pliant"
"==="

Пример выходных данных:
"the {rame?} in pain falls"
"{main mainly} on the plain"
"was {hints?} plaint"
