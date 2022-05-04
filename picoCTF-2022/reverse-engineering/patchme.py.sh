#/bin/env

# link: https://play.picoctf.org/practice/challenge/287?category=3&originalEvent=70&page=1&search=

wget -q https://artifacts.picoctf.net/c/386/patchme.flag.py
wget -q https://artifacts.picoctf.net/c/386/flag.txt.enc

echo "ak98-=90adfjhgj321sleuth9000" | python3 patchme.flag.py | grep -Eo "picoCTF{\w+}"

rm ./patchme.flag.py
rm ./flag.txt.enc